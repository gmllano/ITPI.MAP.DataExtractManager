﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Class responsible to extract and load Section Term data.
	/// </summary>
	public class OrchestrationManager : IOrchestrationManager
	{
		#region variables
		
		private readonly string sourcePath = string.Empty;
		private readonly IDataExtractManager dataExtractManager;

		#endregion

		#region constructor

		/// <summary>
		/// The contructor for the Orchestration Manager.
		/// </summary>
		/// <param name="sourcePath">The source path of the files.</param>
		/// <param name="dataExtractManager">The database extract manager.</param>
		/// <param name="log">The log.</param>
		public OrchestrationManager(string sourcePath, IDataExtractManager dataExtractManager, ILogger log)
		{
			this.sourcePath = sourcePath;
			this.dataExtractManager = dataExtractManager;
			this.Log = log;
		}

		#endregion

		#region public properties
		public ILogger Log { get; set; }

		#endregion

		#region public methods.

		/// <summary>
		/// Get available files from the source directory.
		/// </summary>
		/// <returns>Return an ordered list of files to be extracted and loaded.</returns>
		public List<FileInfo> GetFiles()
		{
			List<FileInfo> files = new List<FileInfo>();

			try
			{
				if (string.IsNullOrEmpty(this.sourcePath))
				{
					throw new ApplicationException("Source path is missing.");
				}

				if (!Directory.Exists(this.sourcePath))
				{
					throw new ApplicationException("Directory does not exists.");
				}

				// Get active semesters.
				Log.Info("Get semesters from database.");
				var semesters = this.dataExtractManager.GetSemesters();

				// Get directory files.
				Log.Info($"Get files from folder {this.sourcePath}");
				var dirFiles = Directory.GetFiles(this.sourcePath);

				// Order the files in descending order by semester.
				foreach (var smster in semesters)
				{
					var file = dirFiles?.FirstOrDefault(a => a.Contains(smster.Semester));
					
					if (file != null)
					{
						var fileInfo = new FileInfo(file);
						files.Add(fileInfo);
					}
				}
			}
			catch (Exception exp)
			{
				Log.Error(exp.Message);	
			}

			return files;
		}

		/// <summary>
		/// Deserialize json file and load into a sections extract object.
		/// </summary>
		/// <param name="file">file information.</param>
		/// <returns>Return a sections extract object.</returns>
		public List<SectionsExtract> MapFile(FileInfo file)
		{
			List<SectionsExtract> sectionsExtract = new List<SectionsExtract>();

			try
			{
				if (file != null)
				{
					string fileContents = File.ReadAllText(file.FullName);
					sectionsExtract = JsonConvert.DeserializeObject<List<SectionsExtract>>(fileContents);
					Log.Info($"The file {file.Name} will be loaded.");
				}
				else
				{
					throw new ApplicationException("file is missing.");
				}

				return sectionsExtract;
			}
			catch (Exception exp)
			{
				Log.Error($"The File {file.FullName} failed to deserialize, Exception: {exp.Message}. Process will continue.");
				return null;
			}
		}

		/// <summary>
		/// Insert course.
		/// </summary>
		/// <param name="sectionsExtract">The sections extract object.</param>
		/// <param name="courseCnt">Number of meetings loaded.</param>
		/// <returns>A value indicating whether it succeeded.</returns>
		public bool InsertCourseInformation(SectionsExtract sectionsExtract, ref int courseCnt)
		{
			try
			{
				return this.dataExtractManager.InsertSectionsExtract(sectionsExtract, ref courseCnt);
			}
			catch (Exception exp)
			{
				Log.Error($"course number {sectionsExtract.CourseNumber} " +
					$"course title {sectionsExtract.CourseTitle} " +
					$"termid {sectionsExtract.TermID}. Exception {exp.Message}");
				return false;
			}
		}

		#endregion
	}
}
