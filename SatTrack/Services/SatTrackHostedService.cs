using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SatTrack.Service.Services.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SatTrack.Service.Services
{
	public class SatTrackHostedService : IHostedService, IDisposable
	{
		private Timer _timer;
		//private readonly object _lockObject = new();
		private readonly ISatTrackService _satTrackService;
		private readonly IStationService _stationService;
		private readonly ISatTrackConfig _config;
		private readonly ILogger<SatTrackHostedService> _logger;

		public SatTrackHostedService(ISatTrackService satTrackService, IStationService stationService,
			ISatTrackConfig config, ILogger<SatTrackHostedService> logger)
		{
			_satTrackService = satTrackService;
			_stationService = stationService;
			_config = config;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			//if (Monitor.TryEnter(_lockObject))
			//{
				try
				{
					await RunStartupTasks();
					_timer = new Timer(async e => await RunTimerTasks(), null, TimeSpan.Zero,
						TimeSpan.FromSeconds(_config.IssCurrentLocationPollRate));
				}
				catch (Exception ex)
				{
					_logger.LogError($"StartAsync threw an exception.{Environment.NewLine}{ex}");
				}
				//finally
				//{
				//	Monitor.Exit(_lockObject);
				//}
			//}
		}

		private async Task RunStartupTasks()
		{
			await _satTrackService.GetPeopleInSpace();
			await _stationService.ReadNoradStations();
		}

		private async Task RunTimerTasks()
		{
			await _satTrackService.PlotSatellites(_config.IssCurrentLocationExportToFile);
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
