using Autofac;
using Autofac.Core;
using System;
using System.Configuration;

namespace ITPI.MAP.DataExtractManager
{
	public class Program
	{
		private static IContainer Container { get; set; }

		public static void Main(string[] args)
		{
			var builder = new ContainerBuilder();

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

				builder.RegisterType<Logger>().As<ILogger>();
				builder.RegisterType<DataManager>().As<IDataManager>()
				.WithParameter(new ResolvedParameter(
							   (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "connectionStr",
							   (pi, ctx) => connectionStr));
				builder.RegisterType<OrchestrationManager>().As<IOrchestrationManager>()
					.WithParameter(new ResolvedParameter(
							   (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "sourcePath",
							   (pi, ctx) => sourcePath));
				Container = builder.Build();

				Run();
			}
			catch (Exception exp)
			{
				throw exp;
			}
		}

		/// <summary>
		/// Gather the files from the source location to be deserialize and loaded into the database.
		/// </summary>
		private static void Run()
		{
			using (var scope = Container)
			{
				var orchestration = scope.Resolve<IOrchestrationManager>();

				try
				{
					orchestration.Log.Info("BEGIN - Extract and load all files from folder.");

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
									orchestration.Log.Warn($"course failed to insert. " +
										$"term {extract?.TermID}, course number {extract?.CourseNumber}, " +
										$"course title {extract?.CourseTitle}");
								}
							}

							orchestration.Log.Info($"The file {file?.Name} completed. Number of records loaded {courseCnt}");
						}
					}

					orchestration.Log.Info("DONE - Extract and load has completed.");
				}
				catch (Exception exp)
				{
					orchestration.Log.Error($"Exception {exp.Message}");
				}
			}
		}
	}
}
