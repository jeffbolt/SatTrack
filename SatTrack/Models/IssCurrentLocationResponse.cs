using SatTrack.Service.Services;

using System;

namespace SatTrack.Service.Models
{
	public class IssCurrentLocationResponse
	{
		public string Message { get; set; }
		public long Timestamp { get; set; }
		public DateTime DateTime
		{
			get { return UnixTimeHelper.FromUnixTime(Timestamp); }
		}
		public IssCurrentLocation Iss_Position { get; set; }
	}
}
