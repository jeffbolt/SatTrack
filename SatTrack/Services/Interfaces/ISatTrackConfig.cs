using System;

namespace SatTrack.Service.Services.Interfaces
{
	public interface ISatTrackConfig
	{
		Uri IssCurrentLocationUri { get; set; }
		Uri PeopleInSpaceUri { get; set; }
		double IssCurrentLocationPollRate { get; set; }
	}
}