using Newtonsoft.Json;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

using SatTrack.Service.Models;
using SatTrack.Service.Services.Interfaces;

using System.Net;
using System.Threading.Tasks;

namespace SatTrack.Service.Services
{
	public class ApiService : IApiService
	{
		private readonly ISatTrackConfig _config;

		public ApiService(ISatTrackConfig config)
		{
			_config = config;
		}

		//public SatelliteLocation GetIssLocation()
		//{
		//	var client = new RestClient(_config.IssCurrentLocationUri);
		//	var response = client.Execute(new RestRequest());

		//	if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
		//	{
		//		var location = JsonConvert.DeserializeObject<IssCurrentLocationResponse>(response.Content);
		//		if (location != null && location.Message.ToLower() == "success") location.Craft = "ISS";
		//		return location;
		//	}
		//	else
		//		return null;
		//}

		//public PeopleInSpaceResponse GetPeopleInSpace()
		//{
		//	var client = new RestClient(_config.PeopleInSpaceUri);
		//	client.UseNewtonsoftJson();
		//	var response = client.Execute(new RestRequest());

		//	if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
		//		return JsonConvert.DeserializeObject<PeopleInSpaceResponse>(response.Content);
		//	else
		//		return null;
		//}

		public async Task<SatelliteLocation> GetIssLocation()
		{
			var client = new RestClient(_config.IssCurrentLocationUri);
			client.UseNewtonsoftJson();
			var response = await client.ExecuteAsync(new RestRequest());

			if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
			{
				var location = JsonConvert.DeserializeObject<IssCurrentLocationResponse>(response.Content);
				if (location != null && location.Message.ToLower() == "success") location.Craft = "ISS";
				return location;
			}
			else
				return null;
		}

		public async Task<PeopleInSpaceResponse> GetPeopleInSpace()
		{
			var client = new RestClient(_config.PeopleInSpaceUri);
			client.UseNewtonsoftJson();
			var response = await client.ExecuteAsync(new RestRequest());

			if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
				return JsonConvert.DeserializeObject<PeopleInSpaceResponse>(response.Content);
			else
				return null;
		}
	}
}
