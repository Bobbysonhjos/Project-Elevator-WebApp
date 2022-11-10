using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;
using WebApp.Services.Repository;

namespace WebApp.Pages.Elevator
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public List<ElevatorViewModel> Elevators { get; private set; }
        public PaginationMetadata? PaginationMetadata { get; private set; }

        public string SortOrder { get; set; } = null!;
        public string SortCol { get; set; } = null!;

        public string Search { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int PageSize { get; set; }



        public class ElevatorViewModel
        {
            public string Id { get; set; } = null!;
            public string Location { get; set; } = null!;
            public string ElevatorStatus { get; set; } = null!;
        }

        public IndexModel(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            Elevators = new List<ElevatorViewModel>();
        }

        public async Task OnGetAsync(string search, string order = "asc", int currentPage = 1, int pageSize = 10, string status = "")
        {
            Status = status;
            Search = search;
            PageSize = pageSize < 1 ? 10 : pageSize > 20 ? 20 : pageSize;

            var (elevators, paginationMetadata, isSuccess) = 
                await _repository.Elevators.GetAllAsync( search: Search, filterOnStatus: Status, pageNumber: currentPage, pageSize: PageSize);

            if (!isSuccess)
                return;
            // TODO add error message

            Elevators = _mapper.Map<List<ElevatorViewModel>>(elevators);
            PaginationMetadata = paginationMetadata;
        }


        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
