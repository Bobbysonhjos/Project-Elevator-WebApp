namespace WebApp.Services.Repository
{
    public interface IRepository
    {
        IElevatorRepository Elevators { get; }
        IErrandRepository Errands { get; }
        ICommentRepository Comments { get; }
    }

    public class Repository : IRepository
    {


        public Repository(IElevatorRepository elevators, IErrandRepository errands, ICommentRepository comments)
        {
            Comments = comments;
            Elevators = elevators;
            Errands = errands;
        }
        public IElevatorRepository Elevators { get; }
        public IErrandRepository Errands { get; }
        public ICommentRepository Comments { get; }
    }
}
