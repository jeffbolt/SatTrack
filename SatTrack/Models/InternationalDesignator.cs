using System.ComponentModel.DataAnnotations;

namespace SatTrack.Service.Models
{
	public class InternationalDesignator
	{
		// aka COSPAR ID https://en.wikipedia.org/wiki/International_Designator
		public ushort LaunchYear { get; set; }                  // Field: 4, Columns: 10–11	(last two digits of launch year)
		public ushort LaunchNumber { get; set; }                // Field: 5, Columns: 12–14 (launch number of the year)
		[MaxLength(3)]
		public string LaunchPiece { get; set; }                 // Field: 6, Columns: 15–17 (piece of the launch)
	}
}
