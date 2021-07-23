using SatTrack.Service.Models;

namespace SatTrack.Service.Services.Interfaces
{
	public interface IApiService
	{
		SatelliteLocation GetIssLocation();
		PeopleInSpaceResponse GetPeopleInSpace();
	}
}