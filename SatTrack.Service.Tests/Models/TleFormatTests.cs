using FluentAssertions;

using SatTrack.Service.Models;

using Xunit;

namespace SatTrack.Service.Tests.Models
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
				Line1 = new TleLine1
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
				Line2 = new TleLine2
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

			long checksum1 = TleFormat.CalculateCheckSum(line1);
			Assert.Equal(actual.Line1.Checksum, checksum1);
			long checksum2 = TleFormat.CalculateCheckSum(line2);
			Assert.Equal(actual.Line2.Checksum, checksum2);

			Assert.True(actual.ChecksumIsValid(1));
			Assert.True(actual.ChecksumIsValid(2));
		}
	}
}
