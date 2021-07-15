using SatTrack.Service.Services;

using System;

namespace SatTrack.Service.Models
{
	public class SatTrackResponse
	{
		public string Message { get; set; }
		public long Timestamp { get; set; }
		public DateTime DateTime
		{
			get { return UnixTimeHelper.FromUnixTime(Timestamp); }
		}
		public Position Iss_Position { get; set; }
	}
}
