using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Linq;


namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Class responsible to communicate with the sql server database.
	/// </summary>
	public class DataExtractManager : IDataExtractManager
	{
		#region variables

		/// <summary>
		/// The sql server connection object.
		/// </summary>
		private readonly string connectionStr = string.Empty;
		private readonly ILog log = null;

		#endregion

		#region constructors

		/// <summary>
		/// The constructor for the data extract manager.
		/// </summary>
		/// <param name="Connection"></param>
		public DataExtractManager(string connectionStr, ILog log)
		{
			this.connectionStr = connectionStr;
			this.log = log;
		}

		#endregion

		#region public methods 
		
		/// <summary>
		/// get active semesters.
		/// </summary>
		/// <returns>A list of semesters.</returns>
		public List<Semesters> GetSemesters()
		{
			IEnumerable<Semesters> semesters;

			try
			{
				using (var connection = new SqlConnection(this.connectionStr))
				{
					connection.Open();
					var query = "dbo.SemestersByActiveStatus_Get";
					semesters = connection.Query<Semesters>(query);
				}
			}
			catch (Exception exp)
			{
				log.Error($"Failed to get semesters from database. Exception {exp.Message}");
				throw exp;
			}

			return semesters.ToList();
		}

		/// <summary>
		/// Insert course data into the Sections Extract table.
		/// </summary>
		/// <param name="SectionsExtractData">A list of courses.</param>
		/// <returns></returns>
		public bool InsertSectionsExtract(SectionsExtract SectionsExtractData, ref int courseCnt)
		{
			bool result = false;
			
			if (SectionsExtractData != null)
			{
				try
				{
					var sectionExtCourses = this.LoadSectionExtract(SectionsExtractData);  // Prepare data table.
					courseCnt += sectionExtCourses.Rows.Count;
					var connection = new SqlConnection(this.connectionStr);

					// Insert data.
					using (var conn = connection)
					{
						conn.Open();
						var sproc = "dbo.SectionsExtract_Ins";
						var value = connection.Execute(sproc, 
							new { SectionsExtractTemp = sectionExtCourses.AsTableValuedParameter("dbo.udSectionsExtractTemp") }, 
							commandType: CommandType.StoredProcedure);

						result = true;	
					}
				}
				catch (Exception exp)
				{
					log.Error($"Failed to insert course into database. {exp.Message}");
				}
			}

			return result;
		}

		#endregion

		#region private methods

		/// <summary>
		/// Prepare and load section extract data.
		/// </summary>
		/// <param name="SectionsExtractData">The Section extract object.</param>
		/// <returns>Returns a data table with courses.</returns>
		private DataTable LoadSectionExtract(SectionsExtract SectionsExtractData)
		{
			var extractData = this.PrepareSectionExtractTbl();

			foreach (var meeting in SectionsExtractData.jsonmeetings)
			{
				foreach (var assign in meeting.jsonassignments)
				{
					var row = extractData.NewRow();
					//todo: need to format create date here.
					row["DateCreated"] = (object)this.UnixTimeStampToDateTime(SectionsExtractData.DateCreated, true) ?? DBNull.Value;
					row["TermID"] = (object)SectionsExtractData.TermID ?? DBNull.Value;
					row["SectionStatus"] = SectionsExtractData.SectionStatus;
					row["CourseVersionID"] = (object)SectionsExtractData.CourseVersionId ?? DBNull.Value;
					row["CourseDiscipline"] = SectionsExtractData.CourseDiscipline;
					row["CourseNumber"] = SectionsExtractData.CourseNumber;
					row["SectionUnits"] = (object)SectionsExtractData.SectionUnits ?? DBNull.Value;
					row["CourseTitle"] = SectionsExtractData.CourseTitle;
					row["SectionNumber"] = SectionsExtractData.SectionNumber;
					row["CombinedSectionID"] = (object)SectionsExtractData.CombinedSectionId ?? DBNull.Value;
					row["MethodOfInstruction"] = SectionsExtractData.MethodOfInstruction;
					row["BasicSkillsFlag"] = SectionsExtractData.BasicSkills;
					row["DayEvening"] = SectionsExtractData.DayEvening;
					row["Responsibility"] = (object)SectionsExtractData.AccountClassResponsibility ?? DBNull.Value;
					row["AcctClassLocation"] = SectionsExtractData.AccountClassLocation;
					row["ClassWeeks"] = (object)SectionsExtractData.ClassWeeks ?? DBNull.Value;
					row["DateClassBegin"] = this.UnixTimeStampToDateTime(SectionsExtractData.DateClassBegin);
					row["DateClassCensus"] = this.UnixTimeStampToDateTime(SectionsExtractData.DateFirstCensus);
					row["DateClassEnd"] = this.UnixTimeStampToDateTime(SectionsExtractData.DateClassEnd);
					row["ClassSizeMax"] = (object)SectionsExtractData.ClassSizeMax ?? DBNull.Value;
					row["CurrentEnrollment"] = (object)SectionsExtractData.CurrentEnrollment ?? DBNull.Value;
					row["WaitList"] = (object)SectionsExtractData.WaitList ?? DBNull.Value;
					row["CensusEnrollment"] = (object)SectionsExtractData.CensusEnrollment ?? DBNull.Value;
					row["TotalHoursAttendance"] = (object)SectionsExtractData.TotalHoursAttendance ?? DBNull.Value;
					row["TBAHours"] = (object)SectionsExtractData.HoursTba ?? DBNull.Value;
					row["OnlineComponent"] = SectionsExtractData.OnlineComponent;
					row["Instructor"] = assign.InstructorName;
					row["ClassComponent"] = assign.ClassComponent;
					row["FTEFContractual"] = (object)assign.FtefContractual?? DBNull.Value;
					row["FTEFOverload"] = (object)assign.FtefOverload ?? DBNull.Value;
					row["FTEFAdjunct"] = (object)assign.FtefAdjunct ?? DBNull.Value;
					row["Building"] = meeting.Building;
					row["Room"] = meeting.Room;
					row["ApportionmentType"] = (object)meeting.ApportionmentType ?? DBNull.Value;
					row["NumberOfMeetings"] = (object)meeting.NumberOfMeetings ?? DBNull.Value;
					row["Day"] = meeting.Days;
					row["DaysPerWeek"] = (object)meeting.DaysPerWeek ?? DBNull.Value;
					row["StartDate"] = this.UnixTimeStampToDateTime(meeting.StartDate);
					row["StartTime"] = this.UnixTimeToReadableTime(meeting.StartTime);
					row["EndDate"] = this.UnixTimeStampToDateTime(meeting.EndDate);
					row["EndTime"] = this.UnixTimeToReadableTime(meeting.EndTime);
					row["TotalApportionmentHours"] = (object)meeting.TotalApportionmentHours ?? DBNull.Value;
					row["MeetingID"] = (object)meeting.MeetingId ?? DBNull.Value;
					row["ContactIncrement"] = (object)SectionsExtractData.ContactIncrement ?? DBNull.Value;
					row["FTESPerEnrollment"] = SectionsExtractData.FtesPerEnrollment;
					row["MeetingMethodofInstruction"] = meeting.MethodOfInstruction;
					row["ClassSizeMaxAdj"] = (object)SectionsExtractData.ClassSizeMaxAdj ?? DBNull.Value;
					row["HoursContactTotal"] = SectionsExtractData.HoursContactTotal;
					row["HoursLectureSchedTotal"] = SectionsExtractData.HoursLectureScheduledTotal;
					row["HoursLabScheTotal"] = SectionsExtractData.HoursLabScheduledTotal;
					row["SAMCode"] = (object)SectionsExtractData.SamCode ?? DBNull.Value;
					row["LabTier"] = (object)SectionsExtractData.LabTier ?? DBNull.Value;
					row["RoomCapacity"] = (object)meeting.RoomCapacity ?? DBNull.Value;

					extractData.Rows.Add(row);
				}
			}

			return extractData;
		}

		/// <summary>
		/// Prepare data table with the course metadata fields.
		/// </summary>
		/// <returns>Returns a data table with course columns.</returns>
		private DataTable PrepareSectionExtractTbl()
		{
			DataTable sectionTermData = new DataTable();
			sectionTermData.TableName = "SectionsExtractTempData";
			sectionTermData.Columns.Add("DateCreated", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("TermID", Type.GetType("System.Double"));
			sectionTermData.Columns.Add("SectionStatus", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("CourseVersionID", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("CourseDiscipline", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("CourseNumber", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("SectionUnits", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("CourseTitle", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("SectionNumber", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("CombinedSectionID", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("MethodOfInstruction", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("BasicSkillsFlag", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("DayEvening", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("Responsibility", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("AcctClassLocation", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("ClassWeeks", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("DateClassBegin", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("DateClassCensus", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("DateClassEnd", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("ClassSizeMax", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("CurrentEnrollment", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("WaitList", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("CensusEnrollment", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("TotalHoursAttendance", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("TBAHours", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("OnlineComponent", System.Type.GetType("System.Int32"));
			sectionTermData.Columns.Add("Instructor", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("ClassComponent", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("FTEFContractual", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("FTEFOverload", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("FTEFAdjunct", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("Building", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("Room", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("ApportionmentType", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("NumberOfMeetings", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("Day", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("DaysPerWeek", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("StartDate", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("StartTime", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("EndDate", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("EndTime", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("TotalApportionmentHours", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("MeetingID", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("ContactIncrement", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("FTESPerEnrollment", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("MeetingMethodofInstruction", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("DuplicateFlag", System.Type.GetType("System.String"));
			sectionTermData.Columns.Add("ClassSizeMaxAdj", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("HoursContactTotal", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("HoursLectureSchedTotal", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("HoursLabScheTotal", System.Type.GetType("System.Decimal"));
			sectionTermData.Columns.Add("SAMCode", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("LabTier", System.Type.GetType("System.Double"));
			sectionTermData.Columns.Add("RoomCapacity", System.Type.GetType("System.Double"));

			return sectionTermData;
		}

		/// <summary>
		/// Returns a readable date time stamp.
		/// </summary>
		/// <param name="unixTimeStamp">Unix date time stamp.</param>
		/// <returns>A value indicating a date time stamp.</returns>
		private string UnixTimeStampToDateTime(string unixTimeStamp, bool isCreateDate = false)
		{
			try
			{
				if (string.IsNullOrEmpty(unixTimeStamp))
					return string.Empty;

				var result = CleanDate(unixTimeStamp);

				DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				var date = epoch.AddMilliseconds(result);

				if (date.Year > 1972)
				{
					if (!isCreateDate)
					{
						return date.ToString("MM/dd/yyyy");
					}
					else
					{
						return date.ToString("MMddyyyy");
					}
					
				}
				else
				{
					return string.Empty;
				}
			}
			catch (Exception exp)
			{
				log.Warn($"Failed to convert epoch time stamp, {unixTimeStamp}. exception {exp.Message}");
				return string.Empty;
			}
		}

		/// <summary>
		/// Convert unix time to human readable time.
		/// </summary>
		/// <param name="unixTimeOnly">The unix time in milliseconds.</param>
		/// <returns>Return the time in a readable format.</returns>
		private string UnixTimeToReadableTime(string unixTimeOnly)
		{
			try
			{
				string timeVal = string.Empty;

				if (string.IsNullOrEmpty(unixTimeOnly))
					return timeVal;

				var result = CleanDate(unixTimeOnly);

				DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(result);

				var timeOfDay = dateTimeOffset.ToLocalTime().ToString().Split(' ');

				if (timeOfDay?.Length <= 0)
					return string.Empty;

				var timeOnly = timeOfDay[1].Substring(0, 5).TrimEnd(':');
				var midDayOffset = timeOfDay[2].ToString();

				return string.Concat(timeOnly, " ", midDayOffset).Trim(' ');
			}
			catch (Exception exp)
			{
				log.Error($"Unable to convert time {unixTimeOnly}. Exception {exp.Message}");
				return string.Empty;
			}
		}

		/// <summary>
		/// Parse out some information for date fields in the json file.
		/// </summary>
		/// <param name="epochDt">The epoch date.</param>
		/// <returns>Return the epoch date in the correct format.</returns>
		private long CleanDate(string epochDt)
		{
			long epochValue;
			StringBuilder epochDate = new StringBuilder(epochDt);
			
			epochDate.Replace("/Date(", "");
			epochDate.Replace(")/", "");

			var result = long.TryParse(epochDate.ToString(), out epochValue);

			return epochValue;
		}

		#endregion
	}
}
