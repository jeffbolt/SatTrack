using SatTrack.Service.Services.Interfaces;

using System;

namespace SatTrack.Service.Services
{
	public class SatTrackConfig : ISatTrackConfig
	{
		public Uri OpenNotifyApiUri { get; set; }
		public double RefreshRate { get; set; }
	}
}
