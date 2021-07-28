using SatTrack.Service.Models;

using System.Threading.Tasks;

namespace SatTrack.Service.Services.Interfaces
{
	public interface IApiService
	{
		IssCurrentLocationResponse GetIssLocation();
		Task<IssCurrentLocationResponse> GetIssLocationAsync();
		Task<PeopleInSpaceResponse> GetPeopleInSpaceAsync();
		PeopleInSpaceResponse GetPeopleInSpace();
	}
}