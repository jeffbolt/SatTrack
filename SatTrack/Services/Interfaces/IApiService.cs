using SatTrack.Service.Models;

using System.Threading.Tasks;

namespace SatTrack.Service.Services.Interfaces
{
	public interface IApiService
	{
		Task<SatelliteLocation> GetIssLocation();
		Task<PeopleInSpaceResponse> GetPeopleInSpace();
	}
}