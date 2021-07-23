using SatTrack.Service.Services;

using System;

namespace SatTrack.Service.Models
{
	public class SatelliteLocation
	{
		public string Craft { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public long Timestamp { get; set; }
		public DateTime DateTime
		{
			get { return UnixTimeHelper.FromUnixTime(Timestamp); }
		}
	}
}
