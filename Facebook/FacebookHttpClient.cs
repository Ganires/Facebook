using FacebookService.ApiConfig;
using FacebookService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FacebookService.Facebook
{
    public interface IFacebookHttpClient
    {
        Task<T> GetAsync<T>( string endpoint, string args = null);
        Task<T> SendToOpenCity<T>(AppealToSend appealToSend);
        Task PostAsync( string endpoint, object data, string args = null);
    }
    public class FacebookHttpClient : IFacebookHttpClient
    {
        private readonly IConfiguration _configuration;
        private string accessToken ;
        private HttpClient _httpClient;

        public FacebookHttpClient(FacebookApi facebookApi, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetValue<string>("BaseUrlForConnectionFacebook"))
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
       
        public async Task<T> GetAsync<T>(string endpoint, string args = null)
        {
            accessToken = _configuration.GetValue<string>("AccessToken");
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
            return default(T);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task PostAsync(string endpoint, object data, string args = null)
        {
            accessToken = _configuration.GetValue<string>("AccessToken");
            
            var payload = GetPayload(data);

            await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
        }

        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<T> SendToOpenCity<T>(AppealToSend appealToSend)
        {
            string jsonString = JsonConvert.SerializeObject(appealToSend).ToString();
            var pageRequestJson = new StringContent(jsonString, Encoding.UTF8, "application/json");
           
            var urlMainProject = _configuration.GetValue<string>("UrlApiForOpenCityApp");


            var response = _httpClient.PostAsync(urlMainProject, pageRequestJson).Result;
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);

        }
    }
}
