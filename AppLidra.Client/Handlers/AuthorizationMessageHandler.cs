//-----------------------------------------------------------------------
// <copiright file="AuthorizationMessageHandler.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

namespace AppLidra.Client.Handlers
{
    using System.Net;
    using System.Net.Http.Headers;
    using Blazored.LocalStorage;

    /// <summary>
    /// A message handler that adds an authorization header to HTTP requests if a token is available.
    /// </summary>
    /// <param name="localStorage">The local storage service to retrieve the token from.</param>
    public class AuthorizationMessageHandler(ILocalStorageService localStorage) : DelegatingHandler()
    {
        private readonly ILocalStorageService _localStorage = localStorage;

        /// <summary>
        /// Sends an HTTP request with an authorization header if a token is available.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The HTTP response message.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = await this._localStorage.GetItemAsync<string>("authToken", cancellationToken).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(token))
            {
                if (request is null || request.Headers is null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // In AuthorizationMessageHandler
                Console.WriteLine($"Token being used: {token[..20]}...");
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}