using System;

using Microsoft.EntityFrameworkCore;

using SatTrack.Data.Contexts;

namespace SatTrack.Data.Tests
{
	public class DatabaseFixture : IDisposable
	{
		public SatTrackDbContext FixtureContext { get; private set; }

		public DatabaseFixture()
		{
			// Create an in-memory copy of the database context, and initialize it with some known values
			DbContextOptions<SatTrackDbContext> options;
			var builder = new DbContextOptionsBuilder<SatTrackDbContext>();
			builder.UseInMemoryDatabase("SatTrackDb");
			options = builder.Options;

			FixtureContext = new SatTrackDbContext(options);
			//FixtureContext.SeedSatellitePositions();
		}

		public void Dispose()
		{
			FixtureContext.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}