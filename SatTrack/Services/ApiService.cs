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

		public SatTrackResponse GetIssPosition()
		{
			var client = new RestClient(_config.OpenNotifyApiUri);
			client.UseNewtonsoftJson();
			var response = client.Execute(new RestRequest());

			if (response?.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
				return JsonConvert.DeserializeObject<SatTrackResponse>(response.Content);
			else
				return null;
		}
	}
}
