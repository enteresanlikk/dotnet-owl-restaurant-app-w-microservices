using static OwlRestaurant.WebApp.SD;

namespace OwlRestaurant.WebApp.Models;

public class APIRequest
{
    public RequestType RequestType { get; set; } = RequestType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
}
