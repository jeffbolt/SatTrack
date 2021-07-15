using SatTrack.Service.Models;

namespace SatTrack.Service.Services.Interfaces
{
	public interface IApiService
	{
		SatTrackResponse GetIssPosition();
	}
}