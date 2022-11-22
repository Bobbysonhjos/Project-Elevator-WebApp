namespace WebApp.Services.Repository
{
    public interface IRepository
    {
        IElevatorRepository Elevators { get; }
        IErrandRepository Errands { get; }
        ICommentRepository Comments { get; }
        IUserRepository Users { get; }
    }

    public class Repository : IRepository
    {


        public Repository(IElevatorRepository elevators, IErrandRepository errands, ICommentRepository comments, IUserRepository users)
        {
            Comments = comments;
            Users = users;
            Elevators = elevators;
            Errands = errands;
        }
        public IElevatorRepository Elevators { get; }
        public IErrandRepository Errands { get; }
        public ICommentRepository Comments { get; }
        public IUserRepository Users { get; }
    }
}
