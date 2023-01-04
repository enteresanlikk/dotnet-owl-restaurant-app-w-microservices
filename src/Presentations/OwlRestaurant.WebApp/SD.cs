namespace OwlRestaurant.WebApp;

public static class SD
{
    public static string ProductAPIBase { get; set; }
    public enum RequestType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
