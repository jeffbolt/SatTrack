using System;

namespace SatTrack.Service.Services.Interfaces
{
	public interface ISatTrackConfig
	{
		Uri IssCurrentLocation { get; set; }
		Uri PeopleInSpace { get; set; }
		double RefreshRate { get; set; }
	}
}