using System.Collections.Generic;

namespace SatTrack.Service.Models
{
	public class PeopleInSpaceResponse
	{
		public string Message { get; set; }
		public long Number { get; set; }
		public List<PeopleInSpace> People { get; set; }
	}
}
