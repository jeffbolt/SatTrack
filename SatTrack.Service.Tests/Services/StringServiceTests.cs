using SatTrack.Service.Services;
using SatTrack.Service.Services.Interfaces;

using Xunit;

namespace SatTrack.Service.Tests.Services
{
	public class StringServiceTests
	{
		private IStringService _service;

		public StringServiceTests()
		{
			_service = new StringService();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(8)]
		[InlineData(16)]
		[InlineData(64)]
		[InlineData(128)]
		[InlineData(256)]
		[InlineData(1024)]
		[InlineData(2056)]
		public void RandomStringTests(int length)
		{
			string rs = _service.RandomString(length);

			Assert.Equal(length, rs.Trim().Length);
			Assert.Contains(rs, c => c >= 'a' && c <= 'z');
			Assert.DoesNotContain(rs, c => c < 'a' && c > 'z');
		}
	}
}
