using System;
using System.Configuration;
using log4net;

namespace ITPI.MAP.DataExtractManager
{
	class Program
	{
		private static readonly ILog log = 
			LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		static void Main(string[] args)
		{
			IDataExtractManager dataExtractManager = null;
			IOrchestrationManager orchestrationManager = null;
			
			try
			{
				string sourcePath = ConfigurationManager.AppSettings["SourcePath"];
				var connectionStr = ConfigurationManager.AppSettings["ConnectionStr"];

				if (string.IsNullOrEmpty(sourcePath))
				{
					throw new ApplicationException("Source directory path is missing from configuration.");
				}

				if (string.IsNullOrEmpty(connectionStr))
				{
					throw new ApplicationException("Connection string is missing from configuration.");
				}

				dataExtractManager = new DataExtractManager(connectionStr, log);

				orchestrationManager = new OrchestrationManager(sourcePath, dataExtractManager, log) ;

				Run(orchestrationManager, log);
			}
			catch (Exception exp)
			{
				log.Error(exp.InnerException.Message);
			}
		}

		/// <summary>
		/// Gather the files to deserialize and load into the database.
		/// </summary>
		/// <param name="orchestration">The orchestration object.</param>
		static void Run(IOrchestrationManager orchestration, ILog log)
		{
			try
			{
				if (orchestration != null)
				{
					log.Info("BEGIN - Extract and load all files from folder.");

					var files = orchestration.GetFiles();

					foreach (var file in files)
					{
						int courseCnt = 0;

						var sectionsExtract = orchestration.MapFile(file);
						
						if (sectionsExtract != null)
						{
							foreach (var extract in sectionsExtract)
							{
								var result = orchestration.InsertCourseInformation(extract, ref courseCnt);

								if (result == false)
								{
									log.Warn($"course failed to insert. " +
										$"term {extract.TermID} course number {extract.CourseNumber}" +
										$"course title {extract.CourseTitle}");
								}
							}

							log.Info($"THe file {file.Name} completed. Number of records loaded {courseCnt}");
						}
					}

					log.Info("DONE - Extract and load has completed.");
				}
			}
			catch (Exception exp)
			{
				log.Error(exp.InnerException.Message);
			}
		}
	}
}
