using Microsoft.Extensions.Logging;

using SatTrack.Service.Services.Interfaces;

using System.Text;
using System.Threading.Tasks;

namespace SatTrack.Service.Services
{
	public class SatTrackService : ISatTrackService
	{
		private readonly IApiService _apiService;
		private readonly IExportService _exportService;
		private readonly ILogger<SatTrackService> _logger;

		public SatTrackService(IApiService apiService, IExportService exportService, ILogger<SatTrackService> logger)
		{
			_apiService = apiService;
			_exportService = exportService;
			_logger = logger;
		}

		public async Task PlotSatellites(bool export = false)
		{
			var location = await _apiService.GetIssLocation();
			if (location != null)
			{
				_logger.LogInformation($"GetIssPosition...\r\n\tCraft: {location.Craft}\r\n\tTimestamp: {location.Timestamp}\r\n\tDateTime: {location.DateTime}\r\n\t" +
					$"Latitude: {location.Location.Latitude}\r\n\tLongitude: {location.Location.Longitude}");
				if (export)
				{
					await _exportService.ExportIssLocationToCsv(location);
				}
			}
			else
			{
				_logger.LogWarning("Invalid response.");
			}
		}

		public async Task GetPeopleInSpace()
		{
			var response = await _apiService.GetPeopleInSpace();
			if (response != null)
			{
				StringBuilder sb = new();
				foreach (var people in response.People)
					sb.Append($"\r\n\tName: {people.Name}, Craft: {people.Craft}");
				_logger.LogInformation($"GetPeopleInSpace...\r\n\tMessage: {response.Message}\r\n\tNumber: {response.Number}" + sb.ToString());
			}
			else
			{
				_logger.LogWarning("Invalid response.");
			}
		}
	}
}
