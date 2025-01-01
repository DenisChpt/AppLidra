//-----------------------------------------------------------------------
// <copiright file="HttpClientAuthorizationExtensions.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Client.Handlers
{
    /// <summary>
    /// Provides extension methods for adding authorization handlers to the service collection.
    /// </summary>
    public static class HttpClientAuthorizationExtensions
    {
        /// <summary>
        /// Adds the authorization handler to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the handler to.</param>
        public static void AddAuthorizationHandler(this IServiceCollection services)
        {
            services.AddScoped<AuthorizationMessageHandler>();
            services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<AuthorizationMessageHandler>();
                handler.InnerHandler = new HttpClientHandler();

                var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:44354/"),
                };

                return client;
            });
        }
    }
}