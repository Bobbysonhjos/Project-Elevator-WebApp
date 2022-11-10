using Newtonsoft.Json;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Services.Repository
{
    public interface IElevatorRepository
    {
        public Task<(IEnumerable<ElevatorDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAllAsync(string? filterOnStatus = "", string? search = "", int pageNumber = 1, int pageSize = 10);
        public Task<ElevatorDto?> GetByIdAsync(string id);
        public Task<ElevatorDto> UpdateAsync();
    }


    public class ApiElevatorRepository : IElevatorRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiElevatorRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(IEnumerable<ElevatorDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)>
            GetAllAsync(string? filterOnStatus = "", string? search = "", int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");

                var httpRequestUri = $"elevators?pageNumber={pageNumber}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(filterOnStatus))
                    httpRequestUri += $"&filterOnStatus={filterOnStatus}";
                if (!string.IsNullOrEmpty(search))
                    httpRequestUri += $"&search={search}";



                var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<HttpResponse<ElevatorDto>>(await response.Content.ReadAsStringAsync());

                return (data.Data, data.PaginationMetadata, true);
            }
            catch
            {
                // Ignored
            }
            return (Enumerable.Empty<ElevatorDto>(), null, false)!;
        }

        public async Task<ElevatorDto?> GetByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception();                    
               
                using var client = _httpClientFactory.CreateClient("APIClient");
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"elevators/{id}");
                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var elevator = JsonConvert.DeserializeObject<ElevatorDto>(await response.Content.ReadAsStringAsync());

                return elevator;
            }
            catch
            {
                // Ignored
            }
            return null!;
        }

        public Task<ElevatorDto> UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
