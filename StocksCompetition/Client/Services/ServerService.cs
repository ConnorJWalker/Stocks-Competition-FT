using System.Net.Http.Json;
using StocksCompetition.Shared;

namespace StocksCompetition.Client.Services;

public class ServerService
{
    private readonly HttpClient _httpClient;

    public ServerService(IConfiguration configuration)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration["ServerBaseAddress"] 
                ?? throw  new MissingFieldException("No server base address provided"))
        };
    }

    public async Task<JwtResponse> SignUp(SignUpForm signUpForm)
    {
        return await SendPost<JwtResponse>("authentication/signup", signUpForm);
    }

    public async Task<JwtResponse> LogIn(LogInForm logInFrom)
    {
        return await SendPost<JwtResponse>("authentication/login", logInFrom);
    }

    private async Task<T> SendPost<T>(string path, object content)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(path, content);
        if (response is null || !response.IsSuccessStatusCode)
        {
            // todo: not this
            throw new Exception("Response was not success code");
        }

        return (await response.Content.ReadFromJsonAsync<T>())!;
    }
}