using SatTrack.Service.Services.Interfaces;

using System;

namespace SatTrack.Service.Services
{
	public class SatTrackConfig : ISatTrackConfig
	{
		public Uri IssCurrentLocation { get; set; }
		public Uri PeopleInSpace { get; set; }
		public double RefreshRate { get; set; }
	}
}
