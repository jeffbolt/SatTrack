using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SatTrack.Service.Services.Interfaces;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SatTrack.Service.Services
{
	public class SatTrackHostedService : IHostedService, IDisposable
	{
		private Timer _timer;
		private object _lockObject = new();
		private readonly ApiService _apiService;
		private readonly ISatTrackConfig _config;
		private readonly ILogger<SatTrackHostedService> _logger;

		public SatTrackHostedService(ApiService apiService, ISatTrackConfig config, ILogger<SatTrackHostedService> logger)
		{
			_apiService = apiService;
			_config = config;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			if (Monitor.TryEnter(_lockObject))
			{
				try
				{
					_timer = new Timer(e => RunTasks(), null, TimeSpan.Zero,
						TimeSpan.FromSeconds(_config.RefreshRate));
				}
				catch (System.Exception ex)
				{
					_logger.LogError($"StartAsync{Environment.NewLine}{ex}");
				}
				finally
				{
					Monitor.Exit(_lockObject);
				}
			}

			return Task.CompletedTask;
		}

		private void RunTasks()
		{
			PlotSatellites();
			GetPeopleInSpace();
		}

		private void PlotSatellites()
		{
			var location = _apiService.GetIssLocation();
			if (location != null)
			{
				_logger.LogInformation($"GetIssPosition...\r\nCraft: {location.Craft}\r\n\tTimestamp: {location.Timestamp}\r\n\tDateTime: {location.DateTime}\r\n\t" +
					$"Latitude: {location.Latitude}\r\n\tLongitude: {location.Longitude}");
			}
			else
			{
				_logger.LogWarning("Invalid response.");
			}
		}

		private void GetPeopleInSpace()
		{
			var response = _apiService.GetPeopleInSpace();
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

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
