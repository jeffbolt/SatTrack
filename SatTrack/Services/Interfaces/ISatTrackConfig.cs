using System;

namespace SatTrack.Service.Services.Interfaces
{
	public interface ISatTrackConfig
	{
		Uri IssCurrentLocationUri { get; set; }
		double IssCurrentLocationPollRate { get; set; }
		public bool IssCurrentLocationExportToFile { get; set; }
		Uri PeopleInSpaceUri { get; set; }
		Uri NoradStationsUri { get; set; }
	}
}