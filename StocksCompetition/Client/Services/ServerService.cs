using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using StocksCompetition.Shared;
using StocksCompetition.Shared.Freetrade;

namespace StocksCompetition.Client.Services;

public class ServerService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    
    public ServerService(IConfiguration configuration, ILocalStorageService localStorage)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration["ServerBaseAddress"] 
                ?? throw  new MissingFieldException("No server base address provided"))
        };

        _localStorage = localStorage;
    }

    public async Task<JwtResponse> SignUp(SignUpForm signUpForm)
    {
        return await SendPost<JwtResponse>("authentication/signup", signUpForm, false);
    }

    public async Task<JwtResponse> LogIn(LogInForm logInFrom)
    {
        return await SendPost<JwtResponse>("authentication/login", logInFrom, false);
    }

    public async Task<AccountDetails> GetAccountChart()
    {
        return await SendGet<AccountDetails>("freetrade/userchart");
    }

    private async Task<T> SendGet<T>(string path, bool withToken = true)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);

        HttpResponseMessage response = await SendRequest(request, withToken);
        return (await response.Content.ReadFromJsonAsync<T>())!;
    }
    
    private async Task<T> SendPost<T>(string path, object content, bool withToken = true)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path);
        request.Content = JsonContent.Create(content);

        HttpResponseMessage response = await SendRequest(request, withToken);
        return (await response.Content.ReadFromJsonAsync<T>())!;
    }

    private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request, bool withToken = true)
    {
        if (withToken)
        {
            // Todo: handle refresh tokens
            var token = await _localStorage.GetItemAsync<JwtResponse>("token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        }

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        if (response is null || !response.IsSuccessStatusCode)
        {
            // Todo: handle failed responses better
            throw new Exception("Response was not success code");
        }

        return response;
    }
}