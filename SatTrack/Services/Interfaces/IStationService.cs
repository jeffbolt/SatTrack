using System.Threading.Tasks;

namespace SatTrack.Service.Services.Interfaces
{
	public interface IStationService
	{
		Task<bool> ReadNoradStations();
	}
}