using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.Extensions.Logging;

using SatTrack.Service.Models;
using SatTrack.Service.Services.Interfaces;

using System;
using System.Globalization;
using System.IO;

namespace SatTrack.Service.Services
{
	public class ExportService : IExportService
	{
		private readonly ILogger<ExportService> _logger;

		public ExportService(ILogger<ExportService> logger)
		{
			_logger = logger;
		}

		public bool ExportIssLocationToCsv(SatelliteLocation location)
		{
			try
			{
				string curDir = Directory.GetCurrentDirectory();
				var directoryInfo = new DirectoryInfo(curDir);
				string exportFolder = Path.Combine(directoryInfo.Parent.Parent.Parent.FullName, "Export");
				if (!Directory.Exists(exportFolder)) Directory.CreateDirectory(exportFolder);

				//string fileName = string.Concat(UnixTimeHelper.CurrentUnixTime().ToString(), ".csv");
				string fileName = "ISS-Locations.csv";
				string filePath = Path.Combine(exportFolder, fileName);
				bool fileExists = File.Exists(filePath);

				using var streamWriter = File.AppendText(filePath);
				var cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);  // en-US = 1033
				var config = new CsvConfiguration(cultureInfo);  // CultureInfo.InvariantCulture = 127

				using var csvWriter = new CsvWriter(streamWriter, config);
				if (!fileExists)
				{
					// Only write headers if file is newly created
					csvWriter.WriteHeader<SatelliteLocation>();
					csvWriter.NextRecord();
				}
				
				csvWriter.WriteRecord(location);
				csvWriter.NextRecord();

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError($"ExportService.ExportIssLocationToCsv threw an exception.\r\n{ex}");
			}
			return false;
		}
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
