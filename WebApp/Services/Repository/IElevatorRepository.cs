using Newtonsoft.Json;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Services.Repository
{
    public interface IElevatorRepository
    {
        public Task<(IEnumerable<ElevatorDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAllAsync(string? filter, string? searchQuery, string? orderBy, int currentPage = 1, int pageSize = 10);
        public Task<ElevatorDto?> GetByIdAsync(string id);
        public Task<(ElevatorWithErrandsDto Elevator, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetByIdWithErrandsAsync(string id, string? filter, string? searchQuery, string? orderBy, int currentPage = 1, int pageSize = 10);
        public Task<IEnumerable<ElevatorIdDto>?> GetAllElevatorIds();
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
            GetAllAsync(string? filter, string? searchQuery, string? orderBy, int currentPage = 1, int pageSize = 10)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");

                var httpRequestUri = $"elevators?currentPage={currentPage}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(filter))
                    httpRequestUri += $"&filter={filter}";
                if (!string.IsNullOrEmpty(searchQuery))
                    httpRequestUri += $"&searchQuery={searchQuery}";
                if (!string.IsNullOrEmpty(orderBy))
                    httpRequestUri += $"&orderBy={orderBy}";



                var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<PaginatedHttpResponse<IEnumerable<ElevatorDto>>>(await response.Content.ReadAsStringAsync());

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

        public async Task<(ElevatorWithErrandsDto Elevator, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetByIdWithErrandsAsync(string id,string? filter, string? searchQuery, string? orderBy, int currentPage = 1, int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception();

                var httpRequestUri = $"elevators/{id}?includeErrands=true&currentPage={currentPage}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(filter))
                    httpRequestUri += $"&filter={filter}";
                if (!string.IsNullOrEmpty(searchQuery))
                    httpRequestUri += $"&searchQuery={searchQuery}";
                if (!string.IsNullOrEmpty(orderBy))
                    httpRequestUri += $"&orderBy={orderBy}";

                using var client = _httpClientFactory.CreateClient("APIClient");
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<PaginatedHttpResponse<ElevatorWithErrandsDto>>(await response.Content.ReadAsStringAsync());
                if (data.Data is null || data.PaginationMetadata is null)
                    throw new Exception();
                   
               
                return (data.Data, data.PaginationMetadata, true);
            }
            catch 
            {
                // Ignored
            }
            return (null,null,false)!;
        }

        public async Task<IEnumerable<ElevatorIdDto>?> GetAllElevatorIds()
        {
            try
            {

                var httpRequestUri = $"elevators/ids";

                using var client = _httpClientFactory.CreateClient("APIClient");
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<HttpResponse<IEnumerable<ElevatorIdDto>>>(await response.Content.ReadAsStringAsync());
                

                return data.Data;
            }
            catch
            {
                // Ignored
            }
            return null;
        }

        public Task<ElevatorDto> UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
