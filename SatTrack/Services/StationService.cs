using Microsoft.Extensions.Logging;

using SatTrack.Service.Models;
using SatTrack.Service.Services.Interfaces;

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SatTrack.Service.Services
{
	public class StationService : IStationService
	{
		private readonly ISatTrackConfig _config;
		private readonly ILogger<StationService> _logger;

		public StationService(ISatTrackConfig config, ILogger<StationService> logger)
		{
			_config = config;
			_logger = logger;
		}

		public async Task<bool> ReadNoradStations()
		{
			int i = 0;
			var sb = new StringBuilder();

			try
			{
				using var client = new WebClient();
				using var stream = client.OpenRead(_config.NoradStationsUri);
				using var reader = new StreamReader(stream);

				while (!reader.EndOfStream)
				{
					if (i % 3 == 0)
					{
						string line1 = await reader.ReadLineAsync();
						sb.Append($"\r\n\r\nStation #{(i / 3) + 1}\r\n\tName: {line1.Trim()}");
						i++;
					}
					else
					{
						string line2 = await reader.ReadLineAsync();
						string line3 = await reader.ReadLineAsync();
						var tle = new TleFormat(line2, line3);

						sb.Append($"\r\n\tLine 1 LineNumber: {tle.Line1.LineNumber}");
						sb.Append($"\r\n\tLine 1 SatelliteCatalogNumber: {tle.Line1.SatelliteCatalogNumber}");
						sb.Append($"\r\n\tLine 1 Classification: {tle.Line1.Classification}");
						sb.Append($"\r\n\tLine 1 CosparID LaunchYear: {tle.Line1.CosparID.LaunchYear}");
						sb.Append($"\r\n\tLine 1 CosparID LaunchNumber: {tle.Line1.CosparID.LaunchNumber}");
						sb.Append($"\r\n\tLine 1 CosparID LaunchPiece: {tle.Line1.CosparID.LaunchPiece}");
						sb.Append($"\r\n\tLine 1 EpochYear: {tle.Line1.EpochYear}");
						sb.Append($"\r\n\tLine 1 Epoch: {tle.Line1.Epoch}");
						sb.Append($"\r\n\tLine 1 MeanMotion Derivative 1: {tle.Line1.MeanMotionD1}");
						sb.Append($"\r\n\tLine 1 MeanMotion Derivative 2: {tle.Line1.MeanMotionD2}");
						sb.Append($"\r\n\tLine 1 Bstar: {tle.Line1.Bstar}");
						sb.Append($"\r\n\tLine 1 EphemerisType: {tle.Line1.EphemerisType}");
						sb.Append($"\r\n\tLine 1 ElementSetNumber: {tle.Line1.ElementSetNumber}");
						sb.Append($"\r\n\tLine 1 Checksum: {tle.Line1.Checksum}");
						sb.Append($"\r\n\tLine 1 Checksum Valid: {tle.ChecksumIsValid(1)}");

						sb.Append($"\r\n\tLine 2 LineNumber: {tle.Line2.LineNumber}");
						sb.Append($"\r\n\tLine 2 SatelliteCatalogNumber: {tle.Line2.SatelliteCatalogNumber}");
						sb.Append($"\r\n\tLine 2 Inclination: {tle.Line2.Inclination}");
						sb.Append($"\r\n\tLine 2 RightAscensionOfAscendingNode: {tle.Line2.RightAscensionOfAscendingNode}");
						sb.Append($"\r\n\tLine 2 Eccentricity: {tle.Line2.Eccentricity}");
						sb.Append($"\r\n\tLine 2 ArgumentOfPerigee: {tle.Line2.ArgumentOfPerigee}");
						sb.Append($"\r\n\tLine 2 MeanAnomaly: {tle.Line2.MeanAnomaly}");
						sb.Append($"\r\n\tLine 2 MeanMotion: {tle.Line2.MeanMotion}");
						sb.Append($"\r\n\tLine 2 Checksum: {tle.Line2.Checksum}");
						sb.Append($"\r\n\tLine 2 Checksum Valid: {tle.ChecksumIsValid(2)}");

						i += 2;
					}
				}

				_logger.LogInformation($"Station Information{sb}");
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError($"StationService.ReadNoradStationsAsync threw an exception.{Environment.NewLine}{ex}");
				return false;
			}
		}

		//public void ReadNoradStations()
		//{
		//	try
		//	{
		//		using var webClient = new WebClient();
		//		string data = webClient.DownloadString(_config.NoradStationsUri);
		//		string[] lines = data.Split("\r\n");
		//		var sb = new StringBuilder();

		//		int i = 0;
		//		while (i < lines.Length - 1)
		//		{
		//			string line = lines[i];
		//			if (i % 3 == 0)
		//			{
		//				sb.Append($"\r\n\r\nStation #{(i++ / 3) + 1}\r\n\tName: {line.Trim()}");
		//			}
		//			else
		//			{
		//				var tle = new TleFormat(lines[i++], lines[i++]);

		//				sb.Append($"\r\n\tLine 1 LineNumber: {tle.Line1.LineNumber}");
		//				sb.Append($"\r\n\tLine 1 SatelliteCatalogNumber: {tle.Line1.SatelliteCatalogNumber}");
		//				sb.Append($"\r\n\tLine 1 Classification: {tle.Line1.Classification}");
		//				sb.Append($"\r\n\tLine 1 CosparID LaunchYear: {tle.Line1.CosparID.LaunchYear}");
		//				sb.Append($"\r\n\tLine 1 CosparID LaunchNumber: {tle.Line1.CosparID.LaunchNumber}");
		//				sb.Append($"\r\n\tLine 1 CosparID LaunchPiece: {tle.Line1.CosparID.LaunchPiece}");
		//				sb.Append($"\r\n\tLine 1 EpochYear: {tle.Line1.EpochYear}");
		//				sb.Append($"\r\n\tLine 1 Epoch: {tle.Line1.Epoch}");
		//				sb.Append($"\r\n\tLine 1 MeanMotion Derivative 1: {tle.Line1.MeanMotionD1}");
		//				sb.Append($"\r\n\tLine 1 MeanMotion Derivative 2: {tle.Line1.MeanMotionD2}");
		//				sb.Append($"\r\n\tLine 1 Bstar: {tle.Line1.Bstar}");
		//				sb.Append($"\r\n\tLine 1 EphemerisType: {tle.Line1.EphemerisType}");
		//				sb.Append($"\r\n\tLine 1 ElementSetNumber: {tle.Line1.ElementSetNumber}");
		//				sb.Append($"\r\n\tLine 1 Checksum: {tle.Line1.Checksum}");
		//				sb.Append($"\r\n\tLine 1 Checksum Valid: {tle.ChecksumIsValid(1)}");

		//				sb.Append($"\r\n\tLine 2 LineNumber: {tle.Line2.LineNumber}");
		//				sb.Append($"\r\n\tLine 2 SatelliteCatalogNumber: {tle.Line2.SatelliteCatalogNumber}");
		//				sb.Append($"\r\n\tLine 2 Inclination: {tle.Line2.Inclination}");
		//				sb.Append($"\r\n\tLine 2 RightAscensionOfAscendingNode: {tle.Line2.RightAscensionOfAscendingNode}");
		//				sb.Append($"\r\n\tLine 2 Eccentricity: {tle.Line2.Eccentricity}");
		//				sb.Append($"\r\n\tLine 2 ArgumentOfPerigee: {tle.Line2.ArgumentOfPerigee}");
		//				sb.Append($"\r\n\tLine 2 MeanAnomaly: {tle.Line2.MeanAnomaly}");
		//				sb.Append($"\r\n\tLine 2 MeanMotion: {tle.Line2.MeanMotion}");
		//				sb.Append($"\r\n\tLine 2 Checksum: {tle.Line2.Checksum}");
		//				sb.Append($"\r\n\tLine 2 Checksum Valid: {tle.ChecksumIsValid(2)}");
		//			}
		//		}

		//		_logger.LogInformation($"Station Information{sb}");
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError($"StationService.ReadNoradStations threw an exception.{Environment.NewLine}{ex}");
		//	}
		//}
	}
}
