using SatTrack.Service.Models;

namespace SatTrack.Service.Services.Interfaces
{
	public interface IExportService
	{
		bool ExportIssLocationToCsv(SatelliteLocation location);
	}
}