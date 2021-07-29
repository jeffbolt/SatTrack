namespace SatTrack.Service.Services.Interfaces
{
	public interface ISatTrackService
	{
		void PlotSatellites(bool export = false);
		void GetPeopleInSpace();
	}
}