namespace SatTrack.Data.Models
{
	public class TleLine2
	{
		public ushort LineNumber { get; set; }                  // Field: 1, Columns: 01–01
		public ushort SatelliteCatalogNumber { get; set; }      // Field: 2, Columns: 03–07 https://en.wikipedia.org/wiki/Satellite_Catalog_Number
		public double Inclination { get; set; }                 // Field: 3, Columns: 09–16 (degrees) https://en.wikipedia.org/wiki/Orbital_inclination
		public double RightAscensionOfAscendingNode { get; set; } // Field: 4, Columns: 18–25 (degrees) https://en.wikipedia.org/wiki/Right_ascension_of_the_ascending_node
		public double Eccentricity { get; set; }                // Field: 5, Columns: 27–33 (decimal point assumed) https://en.wikipedia.org/wiki/Orbital_eccentricity
		public double ArgumentOfPerigee { get; set; }           // Field: 6, Columns: 35–42 (degrees) https://en.wikipedia.org/wiki/Argument_of_perigee
		public double MeanAnomaly { get; set; }                 // Field: 7, Columns: 44–51 (degrees) https://en.wikipedia.org/wiki/Mean_Anomaly
		public double MeanMotion { get; set; }                  // Field: 7, Columns: 53–63 (revolutions per day) https://en.wikipedia.org/wiki/Mean_Motion
		public ulong RevolutionNumberAtEpoch { get; set; }      // Field: 8, Columns: 64–68 (revolutions) 
		public ushort Checksum { get; set; }                    // Field: 9, Columns: 69–69 (modulo 10)
	}
}
