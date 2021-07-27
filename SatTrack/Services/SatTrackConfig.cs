using SatTrack.Service.Services.Interfaces;

using System;

namespace SatTrack.Service.Services
{
	public class SatTrackConfig : ISatTrackConfig
	{
		public Uri IssCurrentLocationUri { get; set; }
		public double IssCurrentLocationPollRate { get; set; }
		public Uri PeopleInSpaceUri { get; set; }
		public Uri NoradStationsUri { get; set; }
	}
}
