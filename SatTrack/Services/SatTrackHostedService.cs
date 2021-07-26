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
		private readonly object _lockObject = new();
		private readonly ISatTrackService _satTrackService;
		private readonly ISatTrackConfig _config;
		private readonly ILogger<SatTrackHostedService> _logger;

		public SatTrackHostedService(ISatTrackService satTrackService, ISatTrackConfig config, ILogger<SatTrackHostedService> logger)
		{
			_satTrackService = satTrackService;
			_config = config;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			if (Monitor.TryEnter(_lockObject))
			{
				try
				{
					RunStartupTasks();
					_timer = new Timer(e => RunTimerTasks(), null, TimeSpan.Zero,
						TimeSpan.FromSeconds(_config.IssCurrentLocationPollRate));
				}
				catch (Exception ex)
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

		private void RunStartupTasks()
		{
			_satTrackService.GetPeopleInSpace();
		}

		private void RunTimerTasks()
		{
			_satTrackService.PlotSatellites();
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
