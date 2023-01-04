using OwlRestaurant.WebApp.Models;

namespace OwlRestaurant.WebApp.Abstractions.Services;

public interface IBaseService : IDisposable
{
    Task<T> SendAsync<T>(APIRequest apiRequest);
}
