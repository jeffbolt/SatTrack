using Microsoft.EntityFrameworkCore;

using SatTrack.Service.Models;

namespace SatTrack.Data.Contexts.Interfaces
{
	public interface ISatTrackDbContext
	{
		DbSet<SatelliteLocation> SatelliteLocations { get; set; }
	}
}