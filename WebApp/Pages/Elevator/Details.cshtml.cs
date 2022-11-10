using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Services.Repository;

namespace WebApp.Pages.Elevator
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DetailsModel(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ElevatorViewModel Elevator { get; private set; }
        public List<ErrandViewModel> Errands { get; private set; }

        public class ElevatorViewModel
        {
            public string Id { get; set; } = null!;
            public string ElevatorStatus { get; set; } = null!;
            public string Location { get; set; } = null!;
        } 

        public class ErrandViewModel
        {
            public string Id { get; set; } = null!;
            public string Title { get; set; } = null!;
            public string Description { get; set; } = null!;
            public string ErrandStatus { get; set; } = null!;
            public string AssignedToId { get; set; } = null!;
            public string AssignedToName { get; set; } = null!;
            public string CreatedById { get; set; } = null!;
            public string CreatedByName { get; set; } = null!;
            public DateTime CreatedDateUtc { get; set; }
        }

        public async Task OnGetAsync(string elevatorId)
        {
            Elevator = _mapper.Map<ElevatorViewModel>(await _repository.Elevators.GetByIdAsync(elevatorId));

            Errands = _mapper.Map<List<ErrandViewModel>>(await _repository.Errands.GetAllErrandsForElevatorAsync(elevatorId));
        }
    }
}
