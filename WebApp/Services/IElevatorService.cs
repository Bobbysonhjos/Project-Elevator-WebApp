using WebApp.Models;
using Moq;


namespace WebApp.Services
{
    public interface IElevatorService
    {
        public Task<IEnumerable<Elevator>> GetAllElevator();
        public Task<Elevator> GetElevatorById(Guid id);
    }

    public class MockElevatorService : IElevatorService
    {
        public Task<IEnumerable<Elevator>> GetAllElevator()
        {
            return Task.FromResult<IEnumerable<Elevator>>(new List<Elevator>());
        }

        public Task<Elevator> GetElevatorById(Guid id)
        {
            return Task.FromResult(new Elevator());

        }
        
            

       
    }


}
