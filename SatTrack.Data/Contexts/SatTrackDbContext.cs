using Microsoft.EntityFrameworkCore;

using SatTrack.Data.Contexts.Interfaces;
using SatTrack.Service.Models;

namespace SatTrack.Data.Contexts
{
	public class SatTrackDbContext : DbContext, ISatTrackDbContext
	{
		public SatTrackDbContext() { }

		public SatTrackDbContext(DbContextOptions<SatTrackDbContext> options) : base(options) { }

		public DbSet<SatelliteLocation> SatelliteLocations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<SatelliteLocation>().Property(l => l.Craft);
			modelBuilder.Entity<SatelliteLocation>().Property(l => l.Latitude);
			modelBuilder.Entity<SatelliteLocation>().Property(l => l.Longitude);
			modelBuilder.Entity<SatelliteLocation>().Property(l => l.Timestamp);
			modelBuilder.Entity<SatelliteLocation>().Property(l => l.DateTime);
		}
	}
}
