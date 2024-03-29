﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using RestSharp;

using SatTrack.Service.Services;
using SatTrack.Service.Services.Interfaces;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

using System;
using System.Threading.Tasks;

namespace SatTrack.Service
{
	internal static class Program
	{
		public static IConfiguration Configuration { get; } = new ConfigurationBuilder().Build();

		public static async Task Main()
		{
			if (!Uri.TryCreate(Environment.GetEnvironmentVariable("ISS_CURRENT_LOCATION_URL"), UriKind.Absolute, out Uri issCurrentLocation))
				throw new ArgumentException("Invalid ISS_CURRENT_LOCATION_URL.");

			if (!double.TryParse(Environment.GetEnvironmentVariable("ISS_CURRENT_LOCATION_POLL_RATE"), out double issPollRate))
				throw new ArgumentException("Invalid ISS_CURRENT_LOCATION_POLL_RATE.");

			if (!bool.TryParse(Environment.GetEnvironmentVariable("ISS_CURRENT_LOCATION_EXPORT_TO_FILE"), out bool issExportToFile))
				throw new ArgumentException("Invalid ISS_CURRENT_LOCATION_EXPORT_TO_FILE.");

			string issExportFileName = Environment.GetEnvironmentVariable("ISS_CURRENT_LOCATION_EXPORT_FILENAME")?.Trim();
			if (issExportToFile && string.IsNullOrEmpty(issExportFileName))
				throw new ArgumentException("Invalid ISS_CURRENT_LOCATION_EXPORT_FILENAME.");

			if (!Uri.TryCreate(Environment.GetEnvironmentVariable("PEOPLE_IN_SPACE_URL"), UriKind.Absolute, out Uri peopleInSpace))
				throw new ArgumentException("Invalid PEOPLE_IN_SPACE_URL.");

			if (!Uri.TryCreate(Environment.GetEnvironmentVariable("NORAD_STATIONS_URL"), UriKind.Absolute, out Uri noradStations))
				throw new ArgumentException("Invalid NORAD_STATIONS_URL.");

			var builder = new HostBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddScoped<ISatTrackConfig>(config => new SatTrackConfig
						{
							IssCurrentLocationUri = issCurrentLocation,
							IssCurrentLocationPollRate = issPollRate,
							IssCurrentLocationExportToFile = issExportToFile,
							IssCurrentLocationExportFileName = issExportFileName,
							PeopleInSpaceUri = peopleInSpace,
							NoradStationsUri = noradStations
						}
					);
					services.AddScoped<IAssemblyService, AssemblyService>();
					services.AddScoped<IRestClient, RestClient>();
					services.AddScoped<IApiService, ApiService>();
					services.AddScoped<ISatTrackService, SatTrackService>();
					services.AddScoped<IStationService, StationService>();
					services.AddScoped<IExportService, ExportService>();
					
					services.AddHostedService<SatTrackHostedService>();
					//services.AddDbContext<ISatTrackDbContext, SatTrackDbContext>();

					ConfigureLogging(services);
				});

			await builder.RunConsoleAsync();
		}

		#region Alternate Startup Method
		//private static void Main(string[] args)
		//{
		//	CreateHostBuilder(args).Build().Run();
		//}

		//private static IHostBuilder CreateHostBuilder(string[] args)
		//{
		//	return Host.CreateDefaultBuilder(args)
		//		.ConfigureServices((context, services) =>
		//		{
		//		});
		//}
		#endregion

		private static void ConfigureLogging(IServiceCollection services)
		{
			/* Switching to using "Serilog" log provider for everything
                NOTE: Call to ClearProviders() is what turns off the default Console Logging

                Output to the Console is now controlled by the WriteTo format below
                DevOps can control the Log output with environment variables
                    LOG_MINIMUMLEVEL - values like INFORMATION, WARNING, ERROR
                    LOG_JSON - true means to output log to console in JSON format
            */
			LogLevel level = LogLevel.None;
			LoggingLevelSwitch serilogLevel = new()
			{
				MinimumLevel = LogEventLevel.Information
			};

			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LOG_MINIMUMLEVEL")))
			{
				Enum.TryParse(Environment.GetEnvironmentVariable("LOG_MINIMUMLEVEL"), out level);
				LogEventLevel eventLevel = LogEventLevel.Information;
				Enum.TryParse(Environment.GetEnvironmentVariable("LOG_MINIMUMLEVEL"), out eventLevel);
				serilogLevel.MinimumLevel = eventLevel;
			}

			bool useJson = Environment.GetEnvironmentVariable("LOG_JSON")?.ToLower() == "true";

			var config = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.ReadFrom.Configuration(Configuration);

			if (useJson)
				config.WriteTo.Console(new ElasticsearchJsonFormatter());
			else  // https://stackoverflow.com/questions/37463721 use :SSS or :fff?
				config.WriteTo.Console(outputTemplate: "[{Timestamp:MM-dd-yyyy HH:mm:ss.fff} {Level:u3}] {Message:lj} {TransactionID}{NewLine}{Exception}", theme: SystemConsoleTheme.Literate);

			if (level != LogLevel.None)
				config.MinimumLevel.ControlledBy(serilogLevel);

			Log.Logger = config.CreateLogger();

			services.AddLogging(lb =>
			{
				lb.ClearProviders();
				lb.AddSerilog();
				lb.AddDebug(); //Write to VS Output window (controlled by appsettings "Logging" section)
			});
		}
	}
}
