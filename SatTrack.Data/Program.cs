using System;

namespace SatTrack.Data
{
	class Program
	{
		public static void Main()
		{
			var tle = new TleFormat
			{
				Line1 = "1 25544U 98067A   08264.51782528 -.00002182  00000-0 -11606-4 0  2927",
				Line2 = "2 25544  51.6416 247.4627 0006703 130.5360 325.0288 15.72125391563537"
			};

			var formatter = new TleFormatter(tle);
			Console.WriteLine($"LineNumber: {formatter.LineNumber}");
			Console.WriteLine($"SatelliteCatalogNumber: {formatter.SatelliteCatalogNumber}");
			Console.WriteLine($"Classification: {formatter.Classification}");
			Console.WriteLine($"CosparID LaunchYear: {formatter.CosparID.LaunchYear}");
			Console.WriteLine($"CosparID LaunchNumber: {formatter.CosparID.LaunchNumber}");
			Console.WriteLine($"CosparID LaunchPiece: {formatter.CosparID.LaunchPiece}");
			Console.WriteLine($"EpochYear: {formatter.EpochYear}");
			Console.WriteLine($"Epoch: {formatter.Epoch}");
			Console.WriteLine($"MeanMotion Derivative 1: {formatter.MeanMotionD1}");
			Console.WriteLine($"MeanMotion Derivative 2: {formatter.MeanMotionD2}");
			Console.WriteLine($"Bstar: {formatter.Bstar}");
			Console.WriteLine($"EphemerisType: {formatter.EphemerisType}");
			Console.WriteLine($"ElementSetNumber: {formatter.ElementSetNumber}");
			Console.WriteLine($"Checksum: {formatter.Checksum}");
		}
	}
}
