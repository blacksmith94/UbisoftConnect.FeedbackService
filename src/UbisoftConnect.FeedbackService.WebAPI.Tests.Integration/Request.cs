using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UbisoftConnect.WebAPI.Tests.Integration
{
	public class Request
	{
		private readonly HttpClient client;

		public Request(HttpClient client)
		{
			this.client = client;
		}

		public async Task<T> Post<T>(string route, object request)
		{
			var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

			using (var postResponse = await client.PostAsync($"/api/{route}", requestContent))
			{
				var postResponseContent = await postResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(postResponseContent);
			}
		}

		public async Task<HttpResponseMessage> Post(string route, object request)
		{
			var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

			using (var postResponse = await client.PostAsync($"/api/{route}", content))
			{
				return postResponse;
			}
		}

		public async Task<T> Post<T>(string route, object request, IHeaderDictionary headerDictionary)
		{
			var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
			foreach (var header in headerDictionary)
			{
				requestContent.Headers.Add(header.Key, (string)header.Value);
			}
			using (var postResponse = await client.PostAsync($"/api/{route}", requestContent))
			{
				var postResponseContent = await postResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(postResponseContent);
			}
		}

		public async Task<HttpResponseMessage> Post(string route, object request, IHeaderDictionary headerDictionary)
		{
			var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
			foreach (var header in headerDictionary)
			{
				requestContent.Headers.Add(header.Key, (string)header.Value);
			}
			using (var postResponse = await client.PostAsync($"/api/{route}", requestContent))
			{
				return postResponse;
			}
		}

		public async Task<HttpResponseMessage> Get(string route)
		{
			using (var getResponse = await client.GetAsync($"/api/{route}"))
			{
				return getResponse;
			}
		}

		public async Task<T> Get<T>(string route)
		{
			using (var getResponse = await client.GetAsync($"/api/{route}"))
			{
				Assert.True(getResponse.IsSuccessStatusCode);
				var getResponseContent = await getResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(getResponseContent);
			}
		}
	}
}
