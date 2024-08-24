namespace Api.Config;

public class WebApiOptions
{
    public const string WebApi = "WebAPI";

    public string[] AllowedOrigins { get; set; } = [];
}
