using Newtonsoft.Json;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

using SatTrack.Service.Models;
using SatTrack.Service.Services.Interfaces;

namespace SatTrack.Service.Services
{
	public class ApiService : IApiService
	{
		private readonly ISatTrackConfig _config;

		public ApiService(ISatTrackConfig config)
		{
			_config = config;
		}

		public SatelliteLocation GetIssLocation()
		{
			var client = new RestClient(_config.IssCurrentLocation);
			client.UseNewtonsoftJson();
			var response = client.Execute(new RestRequest());

			if (response?.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
			{
				var location = JsonConvert.DeserializeObject<IssCurrentLocationResponse>(response.Content);
				return new SatelliteLocation
				{
					Craft = "ISS",
					Latitude = location.Latitude,
					Longitude = location.Longitude,
					Timestamp = location.Timestamp
				};
			}
			else
				return null;
		}

		public PeopleInSpaceResponse GetPeopleInSpace()
		{
			var client = new RestClient(_config.PeopleInSpace);
			client.UseNewtonsoftJson();
			var response = client.Execute(new RestRequest());

			if (response?.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
				return JsonConvert.DeserializeObject<PeopleInSpaceResponse>(response.Content);
			else
				return null;
		}
	}
}
