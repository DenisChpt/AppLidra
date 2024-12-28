using AppLidra.Client.Services;
namespace AppLidra.Client.Handlers;
public static class HttpClientAuthorizationExtensions
{
    public static void AddAuthorizationHandler(this IServiceCollection services)
    {
        services.AddScoped<AuthorizationMessageHandler>();
        services.AddScoped(sp =>
        {
            var handler = sp.GetRequiredService<AuthorizationMessageHandler>();
            handler.InnerHandler = new HttpClientHandler();

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:44354/")
            };

            return client;
        });
    }
}