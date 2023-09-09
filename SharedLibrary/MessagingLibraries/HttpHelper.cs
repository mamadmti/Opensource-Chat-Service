using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SharedLibrary.MessagingLibraries
{
	public class HttpHelper
	{

		public string apiBasicUri { get; set; }
		public async Task<T> Post<T>(string url, object contentValue,string? apikey, string? token = "")
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(apiBasicUri);
				client.DefaultRequestHeaders.Add("api-key", apikey);
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8,
					"application/json");


                HttpResponseMessage result = await client.PostAsync(url, content);
				//result.EnsureSuccessStatusCode();
				//responseHandling responseHandling = new responseHandling();
				string resultContentString = await result.Content.ReadAsStringAsync();


                //responseHandling.StatusCode = result.StatusCode;
                //result.

                try
				{
					T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
					
					//T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
					return resultContent;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
				//T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
				////T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
				//return resultContent;

				//try
				//{

				//}
				//catch (Exception e)
				//{

				//}
				//object resultContent = JsonConvert.DeserializeObject<object>(resultContentString);
				//return resultContent;
			}
		}




		public async Task<T> Get<T>(string url, string? apikey="",string? token = "")
		{
			using (var client = new HttpClient())
			{
                client.BaseAddress = new Uri(apiBasicUri);
                //client.DefaultRequestHeaders.Add("api-key", apikey);

                client.DefaultRequestHeaders.Add("accept", "text/plain");


                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = await client.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);

                return resultContent;
            }
		}


		public async Task<T> Put<T>(string url, object contentValue, string? apikey)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(apiBasicUri);
				client.DefaultRequestHeaders.Add("api-key", apikey);

				var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8,
					"application/json");
				//client.DefaultRequestHeaders.Add("api-key", apiKey);

				var result = await client.PutAsync(url, content);
				result.EnsureSuccessStatusCode();

				string resultContentString = await result.Content.ReadAsStringAsync();
				T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
				return resultContent;
			}
		}


		public async Task Delete(string url, string? apikey)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(apiBasicUri);
				client.DefaultRequestHeaders.Add("api-key", apikey);

				var result = await client.DeleteAsync(url);
				result.EnsureSuccessStatusCode();
			}
		}
	}

}
