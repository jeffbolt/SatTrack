using SatTrack.Service.Services;

using System;

namespace SatTrack.Service.Models
{
	public class IssCurrentLocationResponse : SatelliteLocation
	{
		public string Message { get; set; }
		public SatelliteLocation Iss_Position { get; set; }
	}
}
