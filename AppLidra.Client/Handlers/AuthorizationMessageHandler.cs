﻿using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace AppLidra.Client.Services;
public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthorizationMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Dans AuthorizationMessageHandler
            Console.WriteLine($"Token being used: {token?.Substring(0, 20)}...");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}