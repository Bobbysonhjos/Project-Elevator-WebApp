using WebApp.Models;

namespace WebApp.Services
{
    public interface IElevatorService
    {
        public Task<IEnumerable<ElevatorDto>> GetAllElevator();
        public Task<ElevatorDto> GetElevatorById(Guid id);
    }

    public class ApiElevatorService : IElevatorService
    {
        public Task<IEnumerable<ElevatorDto>> GetAllElevator()
        {
            return Task.FromResult<IEnumerable<ElevatorDto>>(new List<ElevatorDto>());
        }

        public Task<ElevatorDto> GetElevatorById(Guid id)
        {
            return Task.FromResult(new ElevatorDto());

        } 
    }
}
