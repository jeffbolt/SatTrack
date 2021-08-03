using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.Extensions.Logging;

using SatTrack.Service.Models;
using SatTrack.Service.Services.Interfaces;

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace SatTrack.Service.Services
{
	public class ExportService : IExportService
	{
		private readonly ISatTrackConfig _config;
		private readonly ILogger<ExportService> _logger;

		public ExportService(ISatTrackConfig config, ILogger<ExportService> logger)
		{
			_config = config;
			_logger = logger;
		}

		public async Task<bool> ExportIssLocationToCsv(SatelliteLocation location)
		{
			// See https://joshclose.github.io/CsvHelper/getting-started/#writing-a-csv-file
			
			try
			{
				string curDir = Directory.GetCurrentDirectory();
				var directoryInfo = new DirectoryInfo(curDir);
				string exportFolder = Path.Combine(directoryInfo.Parent.Parent.Parent.FullName, "Export");
				if (!Directory.Exists(exportFolder)) Directory.CreateDirectory(exportFolder);

				//string fileName = string.Concat(UnixTimeHelper.CurrentUnixTime().ToString(), ".csv");
				string fileName = _config.IssCurrentLocationExportFileName;
				string filePath = Path.Combine(exportFolder, fileName);
				bool fileExists = File.Exists(filePath);

				using var streamWriter = File.AppendText(filePath);
				var cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);  // en-US = 1033
				var config = new CsvConfiguration(cultureInfo);  // CultureInfo.InvariantCulture = 127

				using var csvWriter = new CsvWriter(streamWriter, config);
				if (!fileExists) csvWriter.WriteHeader<SatelliteLocation>();
				//csvWriter.NextRecord();
				await csvWriter.NextRecordAsync();
				csvWriter.WriteRecord(location);

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError($"ExportService.ExportIssLocationToCsv threw an exception.{Environment.NewLine}{ex}");
			}
			return false;
		}

		//public async Task<bool> ExportIssLocationsToCsv(List<SatelliteLocation> locations)
		//{
		//	try
		//	{
		//		string curDir = Directory.GetCurrentDirectory();
		//		var directoryInfo = new DirectoryInfo(curDir);
		//		string exportFolder = Path.Combine(directoryInfo.Parent.Parent.Parent.FullName, "Export");
		//		if (!Directory.Exists(exportFolder)) Directory.CreateDirectory(exportFolder);

		//		//string fileName = string.Concat(UnixTimeHelper.CurrentUnixTime().ToString(), ".csv");
		//		string fileName = "ISS-Locations.csv";
		//		string filePath = Path.Combine(exportFolder, fileName);
		//		bool fileExists = File.Exists(filePath);

		//		using var streamWriter = File.AppendText(filePath);
		//		var cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);  // en-US = 1033
		//		var config = new CsvConfiguration(cultureInfo);  // CultureInfo.InvariantCulture = 127

		//		using var csvWriter = new CsvWriter(streamWriter, config);
		//		if (!fileExists)
		//		{
		//			// Only write headers if file is newly created
		//			csvWriter.WriteHeader<SatelliteLocation>();
		//		}
		//		await csvWriter.NextRecordAsync();
		//		await csvWriter.WriteRecordsAsync(locations);

		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError($"ExportService.ExportIssLocationsToCsv threw an exception.{Environment.NewLine}{ex}");
		//	}
		//	return false;
		//}
	}

	//public class SatelliteLocationMap : ClassMap<SatelliteLocation>
	//{
	//	public SatelliteLocationMap()
	//	{
	//		Map(m => m.Timestamp).Index(0).Name("Timestamp");
	//		Map(m => m.DateTime).Index(1).Name("DateTime");
	//		Map(m => m.Craft).Index(2).Name("Craft");
	//		Map(m => m.Location.Latitude).Index(3).Name("Latitude");
	//		Map(m => m.Location.Longitude).Index(4).Name("Longitude");
	//	}
	//}
}
