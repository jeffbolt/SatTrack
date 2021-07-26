using FluentAssertions;

using Newtonsoft.Json;

using NSubstitute;

using RestSharp;

using SatTrack.Service.Models;
using SatTrack.Service.Services;
using SatTrack.Service.Services.Interfaces;

using System.Net;
using System.Net.Http;
using System.Text;

using Xunit;

namespace SatTrack.Service.Tests.Services
{
	public class ApiServiceTests
	{
		private readonly ISatTrackConfig _config;
		private readonly IRestClient _restClient;
		//private readonly IHttpClientFactory _httpFactory;

		public ApiServiceTests()
		{
			_config = Substitute.For<ISatTrackConfig>();
			_restClient = Substitute.For<IRestClient>();
			//_restClient = new RestClient(_config.IssCurrentLocation);
			//_httpFactory = Substitute.For<IHttpClientFactory>();
		}

		[Fact]
		public void GetIssLocation_ReturnsSuccess()
		{
			var expected = new SatelliteLocation
			{
				Craft = "ISS",
				Location = new LatLong
				{
					Latitude = 100,
					Longitude = 200
				},
				Timestamp = UnixTimeHelper.CurrentUnixTime()
			};
			//_httpFactory.CreateClient().ReturnsForAnyArgs(
			//	FakeHttpClient.GetFakeClient(HttpStatusCode.OK, expected));
			//var service = new ApiService(_httpFactory);

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
			var service = new ApiService(_config);
			
			var result = service.GetIssLocation();

			//_httpFactory.Received(1).CreateClient("CompanyAPI");
			result.Should().BeEquivalentTo(expected);
		}
	}
}
