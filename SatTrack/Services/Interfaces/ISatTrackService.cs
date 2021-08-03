using System.Threading.Tasks;

namespace SatTrack.Service.Services.Interfaces
{
	public interface ISatTrackService
	{
		Task PlotSatellites(bool export = false);
		Task GetPeopleInSpace();
	}
}