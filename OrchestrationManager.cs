using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Class responsbile to extract and load Section Term data.
	/// </summary>
	public class OrchestrationManager : IOrchestrationManager
	{
		#region variables
		
		private readonly string sourcePath = string.Empty;
		private readonly IDataExtractManager dataExtractManager;
		private readonly ILog log = null;

		#endregion

		#region constructor

		public OrchestrationManager(string sourcePath, IDataExtractManager dataExtractManager, ILog log)
		{
			this.sourcePath = sourcePath;
			this.dataExtractManager = dataExtractManager;
			this.log = log;
		}

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
				log.Info("Get semesters from database.");
				var semesters = this.dataExtractManager.GetSemesters();

				// Get directory files.
				log.Info($"Get files from folder {this.sourcePath}");
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
				log.Error(exp.Message);	
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
					String fileContents = File.ReadAllText(file.FullName);
					sectionsExtract = JsonConvert.DeserializeObject<List<SectionsExtract>>(fileContents);
					log.Info($"The file {file.Name} will be loaded.");
				}
				else
				{
					throw new ApplicationException("file is missing.");
				}

				return sectionsExtract;
			}
			catch (Exception exp)
			{
				log.Error(exp.InnerException.Message);
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
				log.Error($"course number {sectionsExtract.CourseNumber} " +
					$"course title {sectionsExtract.CourseTitle} " +
					$"termid {sectionsExtract.TermID}. Exception {exp.Message}");
				return false;
			}
		}

		#endregion
	}
}
