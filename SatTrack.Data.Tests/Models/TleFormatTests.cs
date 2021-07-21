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
					Bstar = -11600000,
					Checksum = 7,
					Classification = 'U',
					CosparID = new InternationalDesignator
					{
						LaunchNumber = 67,
						LaunchPiece = "A",
						LaunchYear = 98
					},
					ElementSetNumber = 292,
					EphemerisType = 0,
					Epoch = 264.5178252,
					EpochYear = 8,
					LineNumber = 1,
					MeanMotionD1 = -2.182E-05,
					MeanMotionD2 = 0,
					SatelliteCatalogNumber = 25544
				},
				TleLine2 = new TleLine2
				{
					ArgumentOfPerigee = 130.536,
					Checksum = 7,
					Eccentricity = 6703,
					Inclination = 51.6416,
					LineNumber = 2,
					MeanAnomaly = 325.028,
					MeanMotion = 15.72125391,
					RevolutionNumberAtEpoch = 56353,
					RightAscensionOfAscendingNode = 247.4627,
					SatelliteCatalogNumber = 25544
				}
			};

			var actual = new TleFormat(line1, line2);

			actual.Should().BeEquivalentTo(expected);
		}
	}
}
