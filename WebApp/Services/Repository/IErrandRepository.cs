using IdentityModel.Client;
using Newtonsoft.Json;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ResourceParameters;

namespace WebApp.Services.Repository
{
    public interface IErrandRepository
    {
        public Task<IEnumerable<ErrandDto>> GetAllErrandsForElevatorAsync(string elevatorId);
        public Task<(IEnumerable<ErrandDto> Errands, PaginationMetadata? PaginationMetadata, bool IsSuccess)> GetAllErrandsPaginatedAsync(ErrandsResourceParameters parameters);
        public Task<ErrandDto> GetErrandByIdForElevatorAsync(string elevatorId, string errandId);
        public Task<(ErrandWithCommentsDto? Errand, PaginationMetadata? paginationMetadata, bool IsSuccess)> GetErrandByIdForElevatorAsync(string elevatorId, string errandId, ErrandWithCommentsResourceParameters parameters);
        public Task<(ErrandDto? Errand, bool IsSuccess)> CreateErrandForElevatorAsync(string elevatorId, string title, string description, string assignedToId);
        public Task<(ErrandDto? Errand, bool IsSuccess)> GetErrandById(string elevatorId, string errandId);
        public Task<bool> UpdateErrandForElevatorAsync(string elevatorId, string errandId, string title, string description, string errandStatus, string assignedToId);
    }


    public class ApiErrandRepository : IErrandRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiErrandRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IEnumerable<ErrandDto>> GetAllErrandsForElevatorAsync(string elevatorId)
        {
            try
            {
                if (string.IsNullOrEmpty(elevatorId))
                    throw new Exception();

                using var client = _httpClientFactory.CreateClient("APIClient");
                var request = new HttpRequestMessage(HttpMethod.Get, $"elevators/{elevatorId}/errands");
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var errands = JsonConvert.DeserializeObject<IEnumerable<ErrandDto>>(await response.Content.ReadAsStringAsync());

                return errands;
            }
            catch
            {
                // Ignored
            }

            return Enumerable.Empty<ErrandDto>();
        }

        public async Task<(IEnumerable<ErrandDto> Errands, PaginationMetadata? PaginationMetadata, bool IsSuccess)> GetAllErrandsPaginatedAsync(ErrandsResourceParameters parameters)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");

                var httpRequestUri = $"errands?currentPage={parameters.CurrentPage}&pageSize={parameters.PageSize}";
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

                var data = JsonConvert.DeserializeObject<PaginatedHttpResponse<IEnumerable<ErrandDto>>>(await response.Content.ReadAsStringAsync());

                return (data.Data, data.PaginationMetadata, true);
            }
            catch
            {
                // Ignored
            }
            return (Enumerable.Empty<ErrandDto>(), null, false);
        }

        public async Task<ErrandDto> GetErrandByIdForElevatorAsync(string elevatorId, string errandId)
        {
            if (string.IsNullOrEmpty(elevatorId) || string.IsNullOrEmpty(errandId))
                return null!;
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");

                var httpRequestUri = $"elevator/{elevatorId}/errands/{errandId}";


                var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<ErrandDto>(await response.Content.ReadAsStringAsync());

                return (data);
            }
            catch
            {
                // Ignored
            }
            return null!;
        }

        public async Task<(ErrandWithCommentsDto? Errand, PaginationMetadata? paginationMetadata, bool IsSuccess)> GetErrandByIdForElevatorAsync(string elevatorId, string errandId, ErrandWithCommentsResourceParameters parameters)
        {
            if (string.IsNullOrEmpty(elevatorId) || string.IsNullOrEmpty(errandId))
                return (null, null, false);
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");

                var httpRequestUri = $"elevators/{elevatorId}/errands/{errandId}?includeComments=true";
                if (parameters.PageSize != 10)
                    httpRequestUri += $"&pageSize={parameters.PageSize}";
                if (parameters.CurrentPage > 1)
                    httpRequestUri += $"&currentPage={parameters.CurrentPage}";

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<PaginatedHttpResponse<ErrandWithCommentsDto>>(await response.Content.ReadAsStringAsync());

                return (data.Data, data.PaginationMetadata, true);
            }
            catch
            {
                // Ignored
            }
            return (null, null, false);
        }

        public async Task<(ErrandDto? Errand, bool IsSuccess)> CreateErrandForElevatorAsync(string elevatorId, string title, string description, string assignedToId)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");
                var httpRequestUri = $"elevators/{elevatorId}/errands";
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, httpRequestUri)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { title, description, assignedToId }), System.Text.Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<HttpResponse<ErrandDto>>(await response.Content.ReadAsStringAsync());

                if (data.Data is null)
                    throw new Exception();


                return (data.Data, true);
            }
            catch
            {
                // Ignored
            }
            return (null, false);
        }

        public async Task<(ErrandDto? Errand, bool IsSuccess)> GetErrandById(string elevatorId, string errandId)
        {
            if (string.IsNullOrEmpty(elevatorId) || string.IsNullOrEmpty(errandId))
                return (null, false);
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");

                var httpRequestUri = $"elevators/{elevatorId}/errands/{errandId}?includeComments=false";

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, httpRequestUri);

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                var data = JsonConvert.DeserializeObject<HttpResponse<ErrandDto>>(await response.Content.ReadAsStringAsync());

                return (data.Data, true);
            }
            catch
            {
                // Ignored
            }
            return (null, false);
        }

        public async Task<bool> UpdateErrandForElevatorAsync(string elevatorId, string errandId, string title, string description,
            string errandStatus, string assignedToId)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("APIClient");
                var httpRequestUri = $"elevators/{elevatorId}/errands/{errandId}";
                var httpRequest = new HttpRequestMessage(HttpMethod.Put, httpRequestUri)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { title, description, assignedToId, errandStatus }), System.Text.Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();


                return true;
            }
            catch
            {
                // Ignored
            }
            return false;
        }

    }
}
