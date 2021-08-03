using FluentAssertions;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using NSubstitute;

using RestSharp;

using SatTrack.Service.Models;
using SatTrack.Service.Services;
using SatTrack.Service.Services.Interfaces;

using System.Net;
using System.Text;
using System.Threading;

using Xunit;

namespace SatTrack.Service.Tests.Services
{
	public class ApiServiceTests
	{
		private readonly ISatTrackConfig _config;
		private readonly IRestClient _restClient;
		private readonly ILogger<ApiService> _logger;

		public ApiServiceTests()
		{
			_config = Substitute.For<ISatTrackConfig>();
			_restClient = Substitute.For<IRestClient>();
			_logger = Substitute.For<ILogger<ApiService>>();
		}

		[Fact]
		public async void GetIssLocation_ReturnsSuccess()
		{
			var expected = new IssCurrentLocationResponse
			{
				Message = "success",
				Craft = "ISS",
				Location = new LatLong
				{
					Latitude = 100,
					Longitude = 200
				},
				Timestamp = UnixTimeHelper.CurrentUnixTime()
			};

			string message = JsonConvert.SerializeObject(expected);
			var response = new RestResponse
			{
				StatusCode = HttpStatusCode.OK,
				ContentEncoding = Encoding.UTF8.ToString(),
				Content = message,
				ContentLength = message.Length
			};

			_config.IssCurrentLocationUri.Returns(new System.Uri("http://test.api"));
			_restClient.ExecuteAsync(Arg.Any<RestRequest>()).Returns(response);
			var service = new ApiService(_restClient, _config, _logger);
			
			var actual = await service.GetIssLocation();

			await _restClient.Received(1).ExecuteAsync(Arg.Any<IRestRequest>(), Arg.Any<CancellationToken>());

			Assert.NotNull(actual);
			Assert.IsType<IssCurrentLocationResponse>(actual);

			actual.Should().BeEquivalentTo(expected);
		}
	}
}
