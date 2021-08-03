using SatTrack.Service.Models;

using System.Threading.Tasks;

namespace SatTrack.Service.Services.Interfaces
{
	public interface IExportService
	{
		Task<bool> ExportIssLocationToCsv(SatelliteLocation location);
	}
}