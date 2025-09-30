using Microsoft.JSInterop;

namespace HighgeekNet.Blazor.Client.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task AddXsrfHeaderAsync(this HttpClient client, IJSRuntime jsRuntime)
        {
            var token = await jsRuntime.InvokeAsync<string>("getCookie", "XSRF-TOKEN");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Remove("X-CSRF-TOKEN");
                client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", token);
            }
        }
    }
}
