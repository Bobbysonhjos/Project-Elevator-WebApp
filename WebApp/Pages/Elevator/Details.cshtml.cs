using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;
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

        public PaginationMetadata PaginationMetadata { get; private set; }
        public ElevatorViewModel Elevator { get; private set; }
   

        public string? Filter { get; private set; }
        public string? SearchQuery { get; private set; }
        public string? Order { get; private set; }
        public string? Column { get; private set; }
        public string? OrderBy { get; private set; }
        public int PageSize { get; private set; }

        public class ElevatorViewModel
        {
            public string Id { get; set; } = null!;
            public string ElevatorStatus { get; set; } = null!;
            public string Location { get; set; } = null!;
            public DateTime CreatedDateUtc { get; set; }
            public IList<ErrandViewModel> Errands { get; set; } = new List<ErrandViewModel>();
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

        public async Task OnGetAsync(string elevatorId, string? filter, string? column, string? order, string? searchQuery, int currentPage = 1, int pageSize = 10)
        {
            Filter = filter;
            SearchQuery = searchQuery;
            PageSize = pageSize < 1 ? 10 : pageSize > 20 ? 20 : pageSize;
            Order = order;
            Column = column;
            OrderBy = column is null ? null : $"{column},{order}";

            if (string.IsNullOrEmpty(elevatorId))
            {
                //redirect error or something
                return;
            }

            var (elevator, paginationMetadata, isSuccess) = await _repository.Elevators.GetByIdWithErrandsAsync(elevatorId, filter: Filter, searchQuery: SearchQuery, orderBy: OrderBy, currentPage: currentPage, pageSize: pageSize);
            if (!isSuccess)
            {
                // redirect or error or something
                return;
            }
            PaginationMetadata = paginationMetadata;
            Elevator = _mapper.Map<ElevatorViewModel>(elevator);
        }
    }
}
