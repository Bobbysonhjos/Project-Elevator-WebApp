namespace WebApp.Services.Repository
{
    public interface IRepository
    {
        IElevatorRepository Elevators { get; }
        IErrandRepository Errands { get; }
    }

    public class Repository : IRepository
    {
        public Repository(IElevatorRepository elevators, IErrandRepository errands)
        {
            Elevators = elevators;
            Errands = errands;
        }

        public IElevatorRepository Elevators { get; }
        public IErrandRepository Errands { get; }
    }
}
