using System;

namespace SatTrack.Data
{
	class Program
	{
		public static void Main()
		{
			const string line1 = "1 25544U 98067A   08264.51782528 -.00002182  00000-0 -11606-4 0  2927";
			const string line2 = "2 25544  51.6416 247.4627 0006703 130.5360 325.0288 15.72125391563537";
			var tleFormat = new TleFormat(line1, line2);

			Console.WriteLine($"Line 1 LineNumber: {tleFormat.TleLine1.LineNumber}");
			Console.WriteLine($"Line 1 SatelliteCatalogNumber: {tleFormat.TleLine1.SatelliteCatalogNumber}");
			Console.WriteLine($"Line 1 Classification: {tleFormat.TleLine1.Classification}");
			Console.WriteLine($"Line 1 CosparID LaunchYear: {tleFormat.TleLine1.CosparID.LaunchYear}");
			Console.WriteLine($"Line 1 CosparID LaunchNumber: {tleFormat.TleLine1.CosparID.LaunchNumber}");
			Console.WriteLine($"Line 1 CosparID LaunchPiece: {tleFormat.TleLine1.CosparID.LaunchPiece}");
			Console.WriteLine($"Line 1 EpochYear: {tleFormat.TleLine1.EpochYear}");
			Console.WriteLine($"Line 1 Epoch: {tleFormat.TleLine1.Epoch}");
			Console.WriteLine($"Line 1 MeanMotion Derivative 1: {tleFormat.TleLine1.MeanMotionD1}");
			Console.WriteLine($"Line 1 MeanMotion Derivative 2: {tleFormat.TleLine1.MeanMotionD2}");
			Console.WriteLine($"Line 1 Bstar: {tleFormat.TleLine1.Bstar}");
			Console.WriteLine($"Line 1 EphemerisType: {tleFormat.TleLine1.EphemerisType}");
			Console.WriteLine($"Line 1 ElementSetNumber: {tleFormat.TleLine1.ElementSetNumber}");
			Console.WriteLine($"Line 1 Checksum: {tleFormat.TleLine1.Checksum}");

			Console.WriteLine($"Line 2 LineNumber: {tleFormat.TleLine2.LineNumber}");
			Console.WriteLine($"Line 2 SatelliteCatalogNumber: {tleFormat.TleLine2.SatelliteCatalogNumber}");
			Console.WriteLine($"Line 2 Inclination: {tleFormat.TleLine2.Inclination}");
			Console.WriteLine($"Line 2 RightAscensionOfAscendingNode: {tleFormat.TleLine2.RightAscensionOfAscendingNode}");
			Console.WriteLine($"Line 2 Eccentricity: {tleFormat.TleLine2.Eccentricity}");
			Console.WriteLine($"Line 2 ArgumentOfPerigee: {tleFormat.TleLine2.ArgumentOfPerigee}");
			Console.WriteLine($"Line 2 MeanAnomaly: {tleFormat.TleLine2.MeanAnomaly}");
			Console.WriteLine($"Line 2 MeanMotion: {tleFormat.TleLine2.MeanMotion}");
			Console.WriteLine($"Line 2 Checksum: {tleFormat.TleLine2.Checksum}");
		}
	}
}
