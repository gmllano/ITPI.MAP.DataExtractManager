using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Interface responsible to retrieve, update, and insert data to sql server.
	/// </summary>
	public interface IDataExtractManager
	{
		/// <summary>
		/// Get active semester data ordered by the semester field.
		/// </summary>
		/// <returns>Returns a list of semesters.</returns>
		List<Semesters> GetSemesters();

		/// <summary>
		/// Insert course information into Sections Extract table.
		/// </summary>
		/// <param name="SectionsExtractData">A list of courses to insert.</param>
		/// <param name="courseCnt">Number of meetings inserted per file.</param>
		/// <returns>A value indicating if the operation succeeded.</returns>
		bool InsertSectionsExtract(SectionsExtract SectionsExtractData, ref int courseCnt);
	}
}
