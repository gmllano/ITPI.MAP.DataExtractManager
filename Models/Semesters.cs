﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITPI.MAP.DataExtractManager
{
	/// <summary>
	/// Class responsible to contain semester information.
	/// </summary>
	public class Semesters
	{
		public string Semester { get; set; }
		
		public string SemesterName { get; set; }
		
		public DateTime BeginningDate { get; set; }

		public DateTime EndingDate { get; set; }

		public string Status { get; set; }

		public short SpreadsheetLoaded { get; set; }

		public short TargetsLoaded { get; set; }

		public DateTime LastAdded { get; set; }

		public string LastAddedBy { get; set; }

		public DateTime LastUpdated { get; set; }

		public string LastUpdatedBy { get; set; }

		public string CatalogYear { get; set; }

		public double GlobalAdj { get; set; }
	}
}
