using Newtonsoft.Json;
using System.Net.Http;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Services.Repository;

public interface IUserRepository
{
    public Task<IEnumerable<UserIdDto>> GetAllUsersId(string role);
}

public class ApiUserRepository : IUserRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiUserRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IEnumerable<UserIdDto>> GetAllUsersId(string role)
    {
        try
        {
            var httpRequestUri = $"users/ids?role={role}";

            using var client = _httpClientFactory.CreateClient("APIClient");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

            var response = await client.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var data = JsonConvert.DeserializeObject<HttpResponse<IEnumerable<UserIdDto>>>(await response.Content.ReadAsStringAsync());


            return data.Data;
        }
        catch
        {
            // Ignored
        }
        return Enumerable.Empty<UserIdDto>();
    }
}