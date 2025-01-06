using System.Text.Json;

namespace ApiClient.Services;

public class ExternalHttpService : IExternalHttpService
{
    private readonly IHttpClientWrapper _httpClient;

    public ExternalHttpService(IHttpClientWrapper httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<T> GetAsync<T>(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Supplied URL is empty/null"); 
        }
       
        // await the response.
        var response = await _httpClient.GetAsync(url);
        
        // Throws an HttpResponseException if the request was unsuccessful.
        response.EnsureSuccessStatusCode();
        
        // Get the content of the response.
        var content = await response.Content.ReadAsStringAsync();
        
        // Serialize from JSON to type T.
        return JsonSerializer.Deserialize<T>(content);
    }
}
