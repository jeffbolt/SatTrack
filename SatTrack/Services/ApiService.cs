using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RestSharp;

using SatTrack.Service.Models;
using SatTrack.Service.Services.Interfaces;

using System;
using System.Net;
using System.Threading.Tasks;

namespace SatTrack.Service.Services
{
	public class ApiService : IApiService
	{
		private readonly IRestClient _restClient;
		private readonly ISatTrackConfig _config;
		private readonly ILogger<ApiService> _logger;

		public ApiService(IRestClient restClient, ISatTrackConfig config, ILogger<ApiService> logger)
		{
			_restClient = restClient;
			_config = config;
			_logger = logger;
		}

		public IssCurrentLocationResponse GetIssLocation()
		{
			try
			{
				var response = _restClient.Execute(new RestRequest(_config.IssCurrentLocationUri, Method.GET));
				if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
				{
					var location = JsonConvert.DeserializeObject<IssCurrentLocationResponse>(response.Content);
					if (location != null && location.Message.ToLower() == "success")
					{
						location.Craft = "ISS";
						return location;
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"ApiService.GetIssLocation threw an exception.\r\n{ex}");
			}

			return null;
		}

		public async Task<IssCurrentLocationResponse> GetIssLocationAsync()
		{
			try
			{
				var response = await _restClient.ExecuteAsync(new RestRequest(_config.IssCurrentLocationUri, Method.GET));
				if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
				{
					var location = JsonConvert.DeserializeObject<IssCurrentLocationResponse>(response.Content);
					if (location != null && location.Message.ToLower() == "success")
					{
						location.Craft = "ISS";
						return location;
					}
				}
			}
			catch(Exception ex)
			{
				_logger.LogError($"ApiService.GetIssLocation threw an exception.\r\n{ex}");
			}
			
			return null;
		}

		public PeopleInSpaceResponse GetPeopleInSpace()
		{
			var response = _restClient.Execute(new RestRequest(_config.PeopleInSpaceUri, Method.GET));

			if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
				return JsonConvert.DeserializeObject<PeopleInSpaceResponse>(response.Content);
			else
				return null;
		}

		public async Task<PeopleInSpaceResponse> GetPeopleInSpaceAsync()
		{
			var response = await _restClient.ExecuteAsync(new RestRequest(_config.PeopleInSpaceUri, Method.GET));

			if (response?.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response?.Content))
				return JsonConvert.DeserializeObject<PeopleInSpaceResponse>(response.Content);
			else
				return null;
		}
	}
}
