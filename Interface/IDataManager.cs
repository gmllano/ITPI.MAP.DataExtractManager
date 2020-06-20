using log4net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Interface responsible to retrieve, update, and insert data to database server.
	/// </summary>
	public interface IDataManager
	{
		/// <summary>
		/// Get active semester data ordered by the semester field.
		/// </summary>
		/// <returns>Returns a list of semesters.</returns>
		List<Semesters> GetSemesters();

		/// <summary>
		/// Insert course information into Sections Extract table.
		/// </summary>
		/// <param name="sectionsExtractData">A list of courses to insert.</param>
		/// <param name="courseCnt">Number of meetings inserted per file.</param>
		/// <returns>A value indicating if the operation succeeded.</returns>
		bool InsertSectionsExtract(SectionsExtract sectionsExtractData, ref int courseCnt);
	}
}
