using System.Threading.Tasks;

namespace SatTrack.Service.Services.Interfaces
{
	public interface ISatTrackService
	{
		Task<bool> PlotSatellites(bool export = false);
		Task<bool> GetPeopleInSpace();
	}
}