namespace ApiClient;

public interface IExternalHttpService
{
    Task<T> GetAsync<T>(string url);
}
