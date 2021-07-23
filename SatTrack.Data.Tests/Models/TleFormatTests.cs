using FluentAssertions;

using SatTrack.Data.Models;

using Xunit;

namespace SatTrack.Data.Tests.Models
{
	public class TleFormatTests
	{
		private const string line1 = "1 25544U 98067A   08264.51782528 -.00002182  00000-0 -11606-4 0  2927";
		private const string line2 = "2 25544  51.6416 247.4627 0006703 130.5360 325.0288 15.72125391563537";

		[Fact]
		public static void TleFormat_ParseSuccess()
		{
			var expected = new TleFormat
			{
				TleLine1 = new TleLine1
				{
					LineNumber = 1,
					SatelliteCatalogNumber = 25544,
					Classification = 'U',
					CosparID = new InternationalDesignator
					{
						LaunchNumber = 67,
						LaunchPiece = "A",
						LaunchYear = 98
					},
					EpochYear = 8,
					Epoch = 264.5178252,
					MeanMotionD1 = -2.182E-05,
					MeanMotionD2 = 0,
					Bstar = -11600000,
					EphemerisType = 0,
					ElementSetNumber = 292,
					Checksum = 7
				},
				TleLine2 = new TleLine2
				{
					LineNumber = 2,
					SatelliteCatalogNumber = 25544,
					Inclination = 51.6416,
					RightAscensionOfAscendingNode = 247.4627,
					Eccentricity = 6703,
					ArgumentOfPerigee = 130.536,
					MeanAnomaly = 325.028,
					MeanMotion = 15.72125391,
					RevolutionNumberAtEpoch = 56353,
					Checksum = 7
				}
			};

			var actual = new TleFormat(line1, line2);
			actual.Should().BeEquivalentTo(expected);

			long checksum1 = GetCheckSum(line1);
			Assert.Equal(actual.TleLine1.Checksum, checksum1);
			long checksum2 = GetCheckSum(line2);
			Assert.Equal(actual.TleLine2.Checksum, checksum2);
		}

		private static long GetCheckSum(string line)
		{
			// The checksums for each line are calculated by adding all numerical digits on that line, including the line number.
			// One is added to the checksum for each negative sign (-) on that line. All other non-digit characters are ignored.
			var chars = line.Remove(line1.Length - 1, 1).Replace(" ", "").ToCharArray();
			long sum = 0;

			foreach (char ch in chars)
			{
				if (char.IsDigit(ch))
					sum += long.Parse(ch.ToString());
				else if (ch == '-')
					sum += 1;
			}

			return sum % 10;
		}
	}
}
