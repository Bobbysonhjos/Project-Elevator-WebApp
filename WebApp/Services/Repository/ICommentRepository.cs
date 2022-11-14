using AutoMapper;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace WebApp.Services.Repository;

public interface ICommentRepository
{
    public Task<bool> CreateCommentForErrandAsync(string elevatorId, string errandId, string message);
}

public class ApiCommentRepository : ICommentRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiCommentRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<bool> CreateCommentForErrandAsync(string elevatorId, string errandId, string message)
    {
        if (string.IsNullOrEmpty(elevatorId) || string.IsNullOrEmpty(errandId))
            return false;
        
        try
        {
            using var client = _httpClientFactory.CreateClient("APIClient");
            var httpRequestUri = $"elevators/{elevatorId}/errands/{errandId}/comments";
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, httpRequestUri);

            httpRequest.Content = new StringContent(JsonConvert.SerializeObject(new { Message = message}), System.Text.Encoding.UTF8, "application/json");

            var response = await client.SendAsync(httpRequest);
            return response.IsSuccessStatusCode;

        }
        catch
        {
            // ignored
        }
        return false;
    }
}