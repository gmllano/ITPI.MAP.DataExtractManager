using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Class responsible to store assignment information for meetings.
	/// </summary>
	public class JsonAssignments
	{
		[JsonProperty("instructorname")]
		public string InstructorName { get; set; }

		[JsonProperty("classcomponent")]
		public string ClassComponent { get; set; }

		[JsonProperty("ftefcontractual")]
		public decimal? FtefContractual { get; set; }

		[JsonProperty("ftefoverload")]
		public decimal? FtefOverload { get; set; }

		[JsonProperty("ftefadjunct")]
		public decimal? FtefAdjunct { get; set; }

		[JsonProperty("datebegin")]
		public string DateBegin { get; set; }

		[JsonProperty("dateend")]
		public string DateEnd { get; set; }

		[JsonProperty("hoursperweek")]
		public double? HoursPerWeek { get; set; }
	}
}
