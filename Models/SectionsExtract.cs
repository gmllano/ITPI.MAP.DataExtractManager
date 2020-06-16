using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Class responsbile to store Sections Extract data
	/// </summary>
		public class SectionsExtract
		{
			[JsonProperty("datecreated")]
			public string DateCreated { get; set; }

			[JsonProperty("termid")]
			public double? TermID { get; set; }
			
			[JsonProperty("sectionstatus")]
			public string SectionStatus { get; set; }
				
			[JsonProperty("courseversionid")]
			public double? CourseVersionId { get; set; }
		
			[JsonProperty("coursediscipline")]
			public string CourseDiscipline { get; set; }
			
			[JsonProperty("coursenumber")]
			public string CourseNumber { get; set; }
			
			[JsonProperty("sectionunits")]
			public decimal? SectionUnits { get; set; }
			
			[JsonProperty("sectionnumber")]
			public string SectionNumber { get; set; }

			[JsonProperty("coursetitle")]
			public string CourseTitle { get; set; }
			
			[JsonProperty("methodofinstruction")]
			public string MethodOfInstruction { get; set; }

			[JsonProperty("basicskills")]
			public string BasicSkills { get; set; }
			
			[JsonProperty("dayevening")]
			public string DayEvening { get; set; }
			
			[JsonProperty("accountclassresponsibility")]
			public double? AccountClassResponsibility { get; set; }

			[JsonProperty("accountclasslocation")]
			public string AccountClassLocation { get; set; }

			[JsonProperty("labtier")]
			public double? LabTier { get; set; }

			[JsonProperty("classweeks")]
			public decimal? ClassWeeks { get; set; }

			[JsonProperty("dateclassbegin")]
			public string DateClassBegin { get; set; }

			[JsonProperty("dateclassend")]
			public string DateClassEnd { get; set; }

			[JsonProperty("datefirstcensus")]
			public string DateFirstCensus { get; set; }
			
			[JsonProperty("classsizemax")]
			public double? ClassSizeMax { get; set; }

			[JsonProperty("classsizemaxadj")]
			public double? ClassSizeMaxAdj { get; set; }

			[JsonProperty("currentenrollment")]
			public double? CurrentEnrollment { get; set; }

			[JsonProperty("waitlist")]
			public double? WaitList { get; set; }

			[JsonProperty("censusenrollment")]
			public double? CensusEnrollment { get; set; }

			[JsonProperty("totalhoursattendance")]
			public double? TotalHoursAttendance { get; set; }

			[JsonProperty("contactincrement")]
			public int? ContactIncrement { get; set; }

			[JsonProperty("ftesperenrollment")]
			public string FtesPerEnrollment { get; set; }
		
			[JsonProperty("hourscontacttotal")]
			public string HoursContactTotal { get; set; }

			[JsonProperty("hourslecturescheduledtotal")]
			public string HoursLectureScheduledTotal { get; set; }

			[JsonProperty("hourslabscheduledtotal")]
			public string HoursLabScheduledTotal { get; set; }

			[JsonProperty("hourstba")]
			public decimal? HoursTba { get; set; }

			[JsonProperty("onlinecomponent")]
			public string OnlineComponent { get; set; }

			[JsonProperty("samcode")]
			public double? SamCode { get; set; }

			[JsonProperty("combinedsectionid")]
			public double? CombinedSectionId { get; set; }

			[JsonProperty("jsonmeetings")]
			public JsonMeetings[] jsonmeetings { get; set; }
		}
}
