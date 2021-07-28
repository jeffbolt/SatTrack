using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace SatTrack.Service.Tests.Services
{
    public static class FakeHttpClient
    {
        public static HttpClient GetFakeClient(HttpStatusCode statusCode, object expected)
        {
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json")
            });
            var fakeClient = new HttpClient(fakeHttpMessageHandler);
            fakeClient.BaseAddress = new Uri("http://www.wiley-epic.com");

            return fakeClient;
        }
    }
}