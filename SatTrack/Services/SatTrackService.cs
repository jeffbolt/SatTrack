using Microsoft.Extensions.Logging;

using SatTrack.Service.Services.Interfaces;

using System.Text;

namespace SatTrack.Service.Services
{
	public class SatTrackService : ISatTrackService
	{
		private readonly ApiService _apiService;
		private readonly ILogger<SatTrackService> _logger;

		public SatTrackService(ApiService apiService, ILogger<SatTrackService> logger)
		{
			_apiService = apiService;
			_logger = logger;
		}

		public void PlotSatellites()
		{
			var location = _apiService.GetIssLocationAsync().Result;
			if (location != null)
			{
				_logger.LogInformation($"GetIssPosition...\r\n\tCraft: {location.Craft}\r\n\tTimestamp: {location.Timestamp}\r\n\tDateTime: {location.DateTime}\r\n\t" +
					$"Latitude: {location.Location.Latitude}\r\n\tLongitude: {location.Location.Longitude}");
			}
			else
			{
				_logger.LogWarning("Invalid response.");
			}
		}

		public void GetPeopleInSpace()
		{
			var response = _apiService.GetPeopleInSpaceAsync().Result;
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
