using System;
using System.Globalization;
using System.Threading;

namespace SatTrack.Service.Services
{
	public static class DateTimeFormatter
	{
		public static string Format(DateTime dateTime, string format = "G", string cultureName = "")
		{
			#region Table of format specifiers
			/* See https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
			 
				Format specifier			Description									Examples
				===========================================================================================================================================================
				d							Short date pattern.							2009-06-15T13:45:30 -> 6/15/2009
				D							Long date pattern.							2009-06-15T13:45:30 -> Monday, June 15, 2009
				f							Full date/time pattern (short time).		2009-06-15T13:45:30 -> Monday, June 15, 2009 1:45 PM
				F							Full date/time pattern (long time).			2009-06-15T13:45:30 -> Monday, June 15, 2009 1:45:30 PM
				g							General date/time pattern (short time).		2009-06-15T13:45:30 -> 6/15/2009 1:45 PM
				G							General date/time pattern (long time).		2009-06-15T13:45:30 -> 6/15/2009 1:45:30 PM
				M, "m"						Month/day pattern.							2009-06-15T13:45:30 -> June 15
				O, "o"						round-trip date/time pattern.				DateTime values:
																						2009-06-15T13:45:30 (DateTimeKind.Local) --> 2009-06-15T13:45:30.0000000-07:00
																						2009-06-15T13:45:30 (DateTimeKind.Utc) --> 2009-06-15T13:45:30.0000000Z
																						2009-06-15T13:45:30 (DateTimeKind.Unspecified) --> 2009-06-15T13:45:30.0000000
																						DateTimeOffset values:
																						2009-06-15T13:45:30-07:00 --> 2009-06-15T13:45:30.0000000-07:00
				R, "r"						RFC1123 pattern.							2009-06-15T13:45:30 -> Mon, 15 Jun 2009 20:45:30 GMT
				s							Sortable date/time pattern.					2009-06-15T13:45:30 (DateTimeKind.Local) -> 2009-06-15T13:45:30
				t							Short time pattern.							2009-06-15T13:45:30 -> 1:45 PM
				T							Long time pattern.							2009-06-15T13:45:30 -> 1:45:30 PM
				u							Universal sortable date/time pattern.		With a DateTime value:
																						2009-06-15T13:45:30 -> 2009-06-15 13:45:30Z
																						With a DateTimeOffset value:
																						2009-06-15T13:45:30 -> 2009-06-15 20:45:30Z
				U							Universal full date/time pattern.			2009-06-15T13:45:30 -> Monday, June 15, 2009 8:45:30 PM
				Y, "y"						Year month pattern.							2009-06-15T13:45:30 -> June 2009
				Any other single character	Unknown specifier.							Throws a run-time FormatException.
			*/
			#endregion

			//var cultureInfo = string.IsNullOrEmpty(cultureName) ? Thread.CurrentThread.CurrentCulture : CultureInfo.CreateSpecificCulture(cultureName);
			//return dateTime.ToString(format, cultureInfo);

			//if (string.IsNullOrEmpty(cultureName)) cultureName = CultureInfo.CurrentCulture.Name;  //Thread.CurrentThread.CurrentCulture.Name
			//return dateTime.ToString(format, CultureInfo.CreateSpecificCulture(cultureName));

			return dateTime.ToString(format.Trim(), CultureInfo.CreateSpecificCulture(cultureName.Trim()));
		}
	}
}
