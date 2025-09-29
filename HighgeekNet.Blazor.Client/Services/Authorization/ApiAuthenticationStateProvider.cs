using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace HighgeekNet.Blazor.Client.Services.Authorization
{
    public class ApiAuthenticationStateProvider(HttpClient http)
    : AuthenticationStateProvider
    {
        private readonly HttpClient _http = http;



        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userInfo = await _http.GetFromJsonAsync<UserInfo>("identity/account/me");
                if (userInfo is not null)
                {
                    var claims = userInfo.Claims.Select(c => new Claim(c.Type, c.Value));
                    var identity = new ClaimsIdentity(claims, "Cookies");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch { }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void NotifyChange()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> LoginAsync(string username, string password)
        {

            var response = await _http.PostAsJsonAsync("/identity/account/login",
                new { Email = username, Password = password });

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegiserAsync(string username, string email, string password)
        {
            var response = await _http.PostAsJsonAsync("/identity/account/register",
                new { Email = email, Password = password, Username = username });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await _http.PostAsync("/identity/account/logout", null);
            return response.IsSuccessStatusCode;
        }

    }

    public record UserInfo(string? Name, IEnumerable<ClaimDto> Claims);
    public record ClaimDto(string Type, string Value);
}
