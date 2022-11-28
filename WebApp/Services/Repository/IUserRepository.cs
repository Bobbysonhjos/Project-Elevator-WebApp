using Newtonsoft.Json;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ResourceParameters;

namespace WebApp.Services.Repository;

public interface IUserRepository
{
    public Task<(IEnumerable<UserDto> Users, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAllPaginatedAsync(UserResourceParameters parameters); 
    public Task<UserDto?> GetByIdAsync(string id);
    public Task<IEnumerable<UserIdDto>> GetAllUsersId(string role);


}

public class ApiUserRepository : IUserRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiUserRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<(IEnumerable<UserDto> Users, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAllPaginatedAsync(UserResourceParameters parameters)
    {
        try
        {
            using var client = _httpClientFactory.CreateClient("APIClient");

            var httpRequestUri = $"users?currentPage={parameters.CurrentPage}&pageSize={parameters.PageSize}";
            if (!string.IsNullOrEmpty(parameters.Filter))
                httpRequestUri += $"&filter={parameters.Filter}";
            if (!string.IsNullOrEmpty(parameters.SearchQuery))
                httpRequestUri += $"&searchQuery={parameters.SearchQuery}";
            if (!string.IsNullOrEmpty(parameters.OrderBy))
                httpRequestUri += $"&orderBy={parameters.OrderBy},{parameters.OrderDirection}";



            var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

            var response = await client.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var data = JsonConvert.DeserializeObject<PaginatedHttpResponse<IEnumerable<UserDto>>>(await response.Content.ReadAsStringAsync());

            return (data.Data, data.PaginationMetadata, true);


        }
        catch
        {
            // Ignored
        }

        return (Enumerable.Empty<UserDto>(), null, false)!;

    }
    public async Task<UserDto?> GetByIdAsync(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception();

            using var client = _httpClientFactory.CreateClient("APIClient");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"users/{id}");
            var response = await client.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var user = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());

            return user;
        }
        catch
        {
            // Ignored
        }
        return null!;
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
