//-----------------------------------------------------------------------
// <copiright file="AuthorizationMessageHandler.cs">
//      <author> Kamil.D Racim.Z Denis.C </author>
// </copiright>
//-----------------------------------------------------------------------

using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace AppLidra.Client.Services;
public class AuthorizationMessageHandler(ILocalStorageService localStorage) : DelegatingHandler()
{
    private readonly ILocalStorageService _localStorage = localStorage;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = await _localStorage.GetItemAsync<string>("authToken", cancellationToken);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Dans AuthorizationMessageHandler
            Console.WriteLine($"Token being used: {token[..20]}...");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}