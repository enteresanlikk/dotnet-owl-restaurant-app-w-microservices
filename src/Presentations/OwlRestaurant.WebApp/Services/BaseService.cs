using Newtonsoft.Json;
using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.DTOs;
using OwlRestaurant.WebApp.Models;
using System.Text;

namespace OwlRestaurant.WebApp.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T> SendAsync<T>(APIRequest apiRequest)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("OwlAPI");
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.RequestUri = new Uri(apiRequest.Url);
            requestMessage.Method = new HttpMethod(apiRequest.RequestType.ToString());
            client.DefaultRequestHeaders.Clear();
            
            if (apiRequest.Data != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<T>(content);
        }
        catch (Exception ex)
        {
            var data = new ResponseDTO() {
                Success = false,
                Message = "Error",
                ErrorMessages = new List<string>() { Convert.ToString(ex.Message) }
            };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
        }
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }
}
