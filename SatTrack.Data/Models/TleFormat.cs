using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SatTrack.Data.Models
{
	// Two-line element set https://en.wikipedia.org/wiki/Two-line_element_set
	public class TleFormat
	{
		public TleLine1 TleLine1 { get; set; }
		public TleLine2 TleLine2 { get; set; }

		public TleFormat() { }

		public TleFormat([MaxLength(69)] string line1, [MaxLength(69)] string line2)
		{
			MapFields(line1, line2);
		}

		private void MapFields(string line1, string line2)
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