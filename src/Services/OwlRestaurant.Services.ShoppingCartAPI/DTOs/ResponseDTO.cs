namespace OwlRestaurant.Services.ShoppingCartAPI.DTOs;

public class ResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public List<string> ErrorMessages { get; set; }
}
