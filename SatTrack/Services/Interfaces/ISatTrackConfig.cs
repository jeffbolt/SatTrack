using System;

namespace SatTrack.Service.Services.Interfaces
{
	public interface ISatTrackConfig
	{
		Uri OpenNotifyApiUri { get; set; }
		double RefreshRate { get; set; }
	}
}