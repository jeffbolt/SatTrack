using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
			string urlIssCurrentLocation = Environment.GetEnvironmentVariable("ISS_CURRENT_LOCATION");
			if (!Uri.TryCreate(urlIssCurrentLocation, UriKind.Absolute, out Uri issCurrentLocation))
				throw new ArgumentException("Invalid ISS_CURRENT_LOCATION.");

			string urlPeopleInSpace = Environment.GetEnvironmentVariable("PEOPLE_IN_SPACE");
			if (!Uri.TryCreate(urlPeopleInSpace, UriKind.Absolute, out Uri peopleInSpace))
				throw new ArgumentException("Invalid PEOPLE_IN_SPACE.");

			string refresh = Environment.GetEnvironmentVariable("REFRESH_RATE");
			if (!double.TryParse(refresh, out double refreshRate))
				throw new ArgumentException("Invalid refresh rate.");

			var builder = new HostBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddScoped<IAssemblyService, AssemblyService>();
					services.AddScoped<ISatTrackConfig>(config =>
						new SatTrackConfig
						{
							IssCurrentLocation = issCurrentLocation,
							PeopleInSpace = peopleInSpace,
							RefreshRate = refreshRate
						}
					);
					services.AddScoped<ApiService>();
					services.AddHostedService<SatTrackHostedService>();

					ConfigureLogging(services);
				});

			await builder.RunConsoleAsync();
		}

		//private static void Main(string[] args)
		//{
		//	CreateHostBuilder(args).Build().Run();
		//}

		//private static IHostBuilder CreateHostBuilder(string[] args)
		//{
		//	string url = Environment.GetEnvironmentVariable("ISS_CURRENT_LOCATION");
		//	if (!Uri.TryCreate(url, UriKind.Absolute, out Uri openNotifyApiUri))
		//		throw new ArgumentException("Invalid OpenNotify API URL.");

		//	string refresh = Environment.GetEnvironmentVariable("REFRESH_RATE");
		//	if (!double.TryParse(refresh, out double refreshRate))
		//		throw new ArgumentException("Invalid refresh rate.");

		//	return Host.CreateDefaultBuilder(args)
		//		.ConfigureServices((context, services) =>
		//		{
		//			services.AddScoped<IAssemblyService, AssemblyService>();
		//			services.AddScoped<ISatTrackConfig>(config =>
		//				new SatTrackConfig
		//				{
		//					OpenNotifyApiUri = openNotifyApiUri,
		//					RefreshRate = refreshRate
		//				}
		//			);
		//			services.AddScoped<IApiService, ApiService>();
		//			services.AddHostedService<SatTrackHostedService>();
		//		});
		//}

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
