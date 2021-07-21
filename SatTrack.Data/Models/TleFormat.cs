using System;
using System.Linq;

namespace SatTrack.Data.Models
{
	// https://en.wikipedia.org/wiki/Two-line_element_set
	public class TleFormat
	{
		public TleLine1 TleLine1 { get; }
		public TleLine2 TleLine2 { get; }

		public TleFormat(string line1, string line2)
		{
			TleLine1 = new TleLine1
			{
				LineNumber = ushort.Parse(line1.Substring(0, 1)),
				SatelliteCatalogNumber = ushort.Parse(line1.Substring(2, 5)),
				Classification = line1.ElementAt(7),
				CosparID = new InternationalDesignator
				{
					LaunchYear = ushort.Parse(line1.Substring(9, 2)),
					LaunchNumber = ushort.Parse(line1.Substring(11, 3)),
					LaunchPiece = line1.Substring(14, 3).Trim()
				},
				EpochYear = ushort.Parse(line1.Substring(18, 2).Trim()),
				Epoch = double.Parse(line1.Substring(20, 11).Trim()),
				MeanMotionD1 = double.Parse(line1.Substring(33, 10).Trim()),
				MeanMotionD2 = double.Parse(line1.Substring(44, 6).Trim()) * Math.Pow(10, double.Parse(line1.Substring(51, 1).Trim())),
				Bstar = double.Parse(line1.Substring(53, 5).Trim()) * Math.Pow(10, double.Parse(line1.Substring(60, 1).Trim())),
				EphemerisType = ushort.Parse(line1.Substring(62, 1).Trim()),
				ElementSetNumber = ushort.Parse(line1.Substring(65, 3).Trim()),
				Checksum = ushort.Parse(line1.Substring(68, 1).Trim())
			};

			TleLine2 = new TleLine2
			{
				LineNumber = ushort.Parse(line2.Substring(0, 1)),
				SatelliteCatalogNumber = ushort.Parse(line2.Substring(2, 6)),
				Inclination = double.Parse(line2.Substring(8, 8).Trim()),
				RightAscensionOfAscendingNode = double.Parse(line2.Substring(17, 8).Trim()),
				Eccentricity = double.Parse(line2.Substring(26, 7).Trim()),
				ArgumentOfPerigee = double.Parse(line2.Substring(34, 8).Trim()),
				MeanAnomaly = double.Parse(line2.Substring(43, 7).Trim()),
				MeanMotion = double.Parse(line2.Substring(52, 11).Trim()),
				RevolutionNumberAtEpoch = ulong.Parse(line2.Substring(63, 5).Trim()),
				Checksum = ushort.Parse(line2.Substring(68, 1).Trim())
			};
		}
	}

	//public class TleFormatter
	//{
	//	public TleFormat TleFormat { get; set; }

	//	public TleFormatter(TleFormat tleFormat)
	//	{
	//// Line 1
	//TleLine1.LineNumber = ushort.Parse(Line1.Substring(0, 1));
	//TleLine1.SatelliteCatalogNumber = ushort.Parse(Line1.Substring(2, 5));
	//TleLine1.Classification = Line1.ElementAt(7);
	//TleLine1.CosparID = new InternationalDesignator
	//{
	//	LaunchYear = ushort.Parse(Line1.Substring(9, 2)),
	//	LaunchNumber = ushort.Parse(Line1.Substring(11, 3)),
	//	LaunchPiece = Line1.Substring(14, 3).Trim()
	//};
	//TleLine1.EpochYear = ushort.Parse(Line1.Substring(18, 2).Trim());
	//TleLine1.Epoch = double.Parse(Line1.Substring(20, 11).Trim());
	//TleLine1.MeanMotionD1 = double.Parse(Line1.Substring(33, 10).Trim());
	//TleLine1.MeanMotionD2 = double.Parse(Line1.Substring(44, 6).Trim()) * Math.Pow(10, double.Parse(Line1.Substring(51, 1).Trim()));
	//TleLine1.Bstar = double.Parse(Line1.Substring(53, 5).Trim()) * Math.Pow(10, double.Parse(Line1.Substring(60, 1).Trim()));
	//TleLine1.EphemerisType = ushort.Parse(Line1.Substring(62, 1).Trim());
	//TleLine1.ElementSetNumber = ushort.Parse(Line1.Substring(65, 3).Trim());
	//TleLine1.Checksum = ushort.Parse(Line1.Substring(68, 1).Trim());

	//// Line 2
	//TleLine2.LineNumber = ushort.Parse(Line2.Substring(0, 1));
	//TleLine2.SatelliteCatalogNumber = ushort.Parse(Line2.Substring(2, 6));
	//TleLine2.Inclination = double.Parse(Line2.Substring(8, 6).Trim());
	//TleLine2.RightAscensionOfAscendingNode = double.Parse(Line2.Substring(17, 8).Trim());
	//TleLine2.Eccentricity = double.Parse(Line2.Substring(34, 8).Trim());
	//TleLine2.ArgumentOfPerigee = double.Parse(Line2.Substring(34, 8).Trim());
	//TleLine2.MeanAnomaly = double.Parse(Line2.Substring(43, 7).Trim());
	//TleLine2.MeanMotion = double.Parse(Line2.Substring(52, 11).Trim());
	//TleLine1.Checksum = ushort.Parse(Line1.Substring(68, 1).Trim());
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
	//}
