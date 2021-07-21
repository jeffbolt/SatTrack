namespace SatTrack.Data.Models
{
	public class TleLine1
	{
		public ushort LineNumber { get; set; }                  // Field: 1, Columns: 01–01
		public ushort SatelliteCatalogNumber { get; set; }      // Field: 2, Columns: 03–07 https://en.wikipedia.org/wiki/Satellite_Catalog_Number
		public char Classification { get; set; }                // Field: 3, Columns: 08–08 (U=Unclassified, C=Classified, S=Secret)
		public InternationalDesignator CosparID { get; set; }
		public ushort EpochYear { get; set; }                   // Field: 7, Columns: 19–20 (last two digits of year)
		public double Epoch { get; set; }                       // Field: 8, Columns: 21–32 (day of the year and fractional portion of the day)
		public double MeanMotionD1 { get; set; }                // Field: 9, Columns: 34–43 (First Derivative of Mean Motion aka the Ballistic Coefficient)
		public double MeanMotionD2 { get; set; }                // Field: 10, Columns: 45–52 (Second Derivative of Mean Motion (decimal point assumed))
		public double Bstar { get; set; }                       // Field: 11, Columns: 54–61 (Drag Term aka Radiation Pressure Coefficient or BSTAR (decimal point assumed))
		public ushort EphemerisType { get; set; }               // Field: 12, Columns: 63–63 (internal use only - always zero in distributed TLE data)
		public ushort ElementSetNumber { get; set; }            // Field: 13, Columns: 65–68 (Incremented when a new TLE is generated for this object)
		public ushort Checksum { get; set; }                    // Field: 14, Columns: 69–69 (modulo 10)
	}
}
