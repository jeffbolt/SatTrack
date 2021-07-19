using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SatTrack.Data
{
	// https://en.wikipedia.org/wiki/Two-line_element_set
	public class TleFormat
	{
		[MaxLength(69)]
		public string Line1 { get; set; }
		[MaxLength(69)]
		public string Line2 { get; set; }
	}

	public class InternationalDesignator
	{
		// aka COSPAR ID https://en.wikipedia.org/wiki/International_Designator
		public ushort LaunchYear { get; set; }      // Field: 4, Columns: 10–11	(last two digits of launch year)
		public ushort LaunchNumber { get; set; }    // Field: 5, Columns: 12–14 (launch number of the year)
		[MaxLength(3)]
		public string LaunchPiece { get; set; }     // Field: 6, Columns: 15–17 (piece of the launch)
	}

	public class TleFormatter
	{
		private readonly TleFormat _tle;

		#region Line 1 Properties
		public ushort LineNumber { get; set; }                  // Field: 1, Columns: 01–01
		public ushort SatelliteCatalogNumber { get; set; }      // Field: 2, Columns: 03–07 https://en.wikipedia.org/wiki/Satellite_Catalog_Number
		public char Classification { get; set; }                // Field: 3, Columns: 08–08 (U=Unclassified, C=Classified, S=Secret)
		public InternationalDesignator CosparID { get; set; }
		public ushort EpochYear { get; set; }                   // Field: 7, Columns: 19–20 (last two digits of year)
		public double Epoch { get; set; }                       // Field: 8, Columns: 21–32 (day of the year and fractional portion of the day)
		public double MeanMotionD1 { get; set; }                // Field: 9, Columns: 34–43 (First Derivative of Mean Motion aka the Ballistic Coefficient)
		public double MeanMotionD2 { get; set; }                // Field: 10, Columns: 45–52 (Second Derivative of Mean Motion (decimal point assumed))
		public double Bstar { get; set; }                       // Field: 11, Columns: 54–61 (Drag Term aka Radiation Pressure Coefficient or BSTAR (decimal point assumed))
		public ushort EphemerisType { get; set; }               // Field: 12, Columns: 63–63 (internal use only - always zero in distributed TLE data)
		public ushort ElementSetNumber { get; set; }            // Field: 13, Columns: 65–68 (Incremented when a new TLE is generated for this object)
		public ushort Checksum { get; set; }                    // Field: 14, Columns: 69–69 (modulo 10)
		#endregion

		#region Line 2 Properties

		#endregion

		public TleFormatter(TleFormat tle)
		{
			_tle = tle;

			LineNumber = ushort.Parse(_tle.Line1.Substring(0, 1));
			SatelliteCatalogNumber = ushort.Parse(_tle.Line1.Substring(2, 5));
			Classification = _tle.Line1.ElementAt(7);
			CosparID = new InternationalDesignator
			{
				LaunchYear = ushort.Parse(_tle.Line1.Substring(9, 2)),
				LaunchNumber = ushort.Parse(_tle.Line1.Substring(11, 3)),
				LaunchPiece = _tle.Line1.Substring(14, 3).Trim()
			};
			EpochYear = ushort.Parse(_tle.Line1.Substring(18, 2).Trim());
			Epoch = double.Parse(_tle.Line1.Substring(20, 11).Trim());
			MeanMotionD1 = double.Parse(_tle.Line1.Substring(33, 10).Trim());
			MeanMotionD2 = double.Parse(_tle.Line1.Substring(44, 6).Trim()) * Math.Pow(10, double.Parse(_tle.Line1.Substring(51, 1).Trim()));
			Bstar = double.Parse(_tle.Line1.Substring(53, 5).Trim()) * Math.Pow(10, double.Parse(_tle.Line1.Substring(60, 1).Trim()));
			EphemerisType = ushort.Parse(_tle.Line1.Substring(62, 1).Trim());
			ElementSetNumber = ushort.Parse(_tle.Line1.Substring(65, 3).Trim());
			Checksum = ushort.Parse(_tle.Line1.Substring(68, 1).Trim());
		}

		//public TleFormatter(TleFormat tle)
		//{
		//	_tle = tle;
		//	var list1 = tle.Line1.ToCharArray().ToList();

		//	LineNumber = ushort.Parse(GetSegment(list1, 0, 1));
		//	SatelliteCatalogNumber = ushort.Parse(GetSegment(list1, 2, 5));
		//	LineNumber = ushort.Parse(GetSegment(list1, 0, 1));
		//	SatelliteCatalogNumber = ushort.Parse(GetSegment(list1, 2, 5));
		//	Classification = _tle.Line1.ElementAt(7);
		//	CosparID = new InternationalDesignator
		//	{
		//		LaunchYear = ushort.Parse(GetSegment(list1, 9, 2)),
		//		LaunchNumber = ushort.Parse(GetSegment(list1, 11, 3)),
		//		LaunchPiece = GetSegment(list1, 14, 3)
		//	};
		//	EpochYear = ushort.Parse(GetSegment(list1, 18, 2));
		//	Epoch = double.Parse(GetSegment(list1, 20, 11));
		//	MeanMotionD1 = double.Parse(GetSegment(list1, 33, 10));
		//	MeanMotionD2 = double.Parse(GetSegment(list1, 44, 6)) * Math.Pow(10, double.Parse(GetSegment(list1, 51, 1)));
		//	Bstar = double.Parse(GetSegment(list1, 53, 5)) * Math.Pow(10, double.Parse(GetSegment(list1, 60, 1)));
		//	EphemerisType = ushort.Parse(GetSegment(list1, 62, 1));
		//	ElementSetNumber = ushort.Parse(GetSegment(list1, 65, 3));
		//	Checksum = ushort.Parse(GetSegment(list1, 68, 1));
		//}

		//private static string GetSegment(List<char> list, int start, int length)
		//{
		//	if (length == 1)
		//		return list.ElementAt(start).ToString();
		//	else
		//		return string.Concat(list.GetRange(start, length)).Trim();
		//}

		//private T GetSegment<T>(List<char> list, int start, int length)
		//{
		//	//https://www.c-sharpcorner.com/uploadfile/40e97e/generic-method-for-parsing-value-type-in-C-Sharp/
		//	return typeof(T).Parse<T>(string.Concat(list.GetRange(start, length)).Trim());
		//}
	}
}
