namespace ApiClient.Services;

public interface IHttpClientWrapper
{
    public Task<HttpResponseMessage> GetAsync(string url);
}
