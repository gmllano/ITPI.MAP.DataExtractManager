using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Class responsible to store meeting information for section extract.
	/// </summary>
	public class JsonMeetings
	{
		[JsonProperty("building")]
		public string Building { get; set; }

		[JsonProperty("room")]
		public string Room { get; set; }

		[JsonProperty("apportionmenttype")]
		public double? ApportionmentType { get; set; }

		[JsonProperty("numberofmeetings")]
		public double? NumberOfMeetings { get; set; }

		[JsonProperty("days")]
		public string Days { get; set; }

		[JsonProperty("daysperweek")]
		public double? DaysPerWeek { get; set; }

		[JsonProperty("startdate")]
		public string StartDate { get; set; }

		[JsonProperty("starttime")]
		public string StartTime { get; set; }

		[JsonProperty("enddate")]
		public string EndDate { get; set; }

		[JsonProperty("endtime")]
		public string EndTime { get; set; }

		[JsonProperty("meetingid")]
		public double? MeetingId { get; set; }

		[JsonProperty("methodofinstruction")]
		public string MethodOfInstruction { get; set; }

		[JsonProperty("totalapportionmenthours")]
		public double? TotalApportionmentHours { get; set; }

		[JsonProperty("roomcapacity")]
		public double? RoomCapacity { get; set; }

		[JsonProperty("jsonassignments")]
		public JsonAssignments[] jsonassignments { get; set; }
	}
}
