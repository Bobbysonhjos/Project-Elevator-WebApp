using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Services.Repository
{
    public interface IErrandRepository
    {
        public Task<IEnumerable<ErrandDto>> GetAllErrandsForElevatorAsync(string elevatorId);
        public Task<ErrandDto> GetErrandByIdForElevatorAsync();
        public Task<ErrandDto> CreateErrandForElevatorAsync();
        public Task<ErrandDto> UpdateErrandForElevatorAsync();
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

        public Task<ErrandDto> GetErrandByIdForElevatorAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ErrandDto> CreateErrandForElevatorAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ErrandDto> UpdateErrandForElevatorAsync()
        {
            throw new NotImplementedException();
        }
    }

}
