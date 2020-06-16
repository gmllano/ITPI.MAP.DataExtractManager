using System.Collections.Generic;
using System.IO;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Interface responsible loading files from file directory and inserting data into sql server.
	/// </summary>
	public interface IOrchestrationManager
	{

		/// <summary>
		/// Get a list of files from the extract directory folder.
		/// </summary>
		/// <param name="directoryPath">The directory path where files are located.</param>
		/// <returns>A list of files.</returns>
		List<FileInfo> GetFiles();

		/// <summary>
		/// Extract the courses in the semester json file.
		/// </summary>
		/// <param name="file">The semester file.</param>
		/// <returns>Return a list of sections extract objects.</returns>
		List<SectionsExtract> MapFile(FileInfo file);
		

		/// <summary>
		/// Insert the course information.
		/// </summary>
		/// <param name="sectionsExtract">The sections extract object.</param>
		/// <param name="courseCnt">Number of meetings inside term file.</param>
		/// <returns>A value indicating if the insert succeeded.</returns>
		bool InsertCourseInformation(SectionsExtract sectionsExtract, ref int courseCnt);
	}
}
