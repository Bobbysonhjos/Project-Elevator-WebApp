using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helpers;
using WebApp.ResourceParameters;
using WebApp.Services.Repository;

namespace WebApp.Pages.Errands
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public IndexModel(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            Errands = new List<ErrandViewModel>();
            PaginationMetadata = new PaginationMetadata();
            Parameters = new ErrandsResourceParameters();
            Filters = new SelectListItem[]
            {
                new() { Text = "None", Value = null }, new() { Text = "New", Value = "new" },
                new() { Text = "In Progress", Value = "inprogress" }, new() { Text = "Completed", Value = "completed" }
            };
        }

        public SelectListItem[] Filters { get; set; }
        public IReadOnlyList<ErrandViewModel> Errands { get; private set; }
        public PaginationMetadata PaginationMetadata { get; set; }
        public ErrandsResourceParameters Parameters { get; private set; }
        public string? Filter { get; set; }
        public bool IsSuccess { get; set; }


        public async Task OnGetAsync([FromQuery] ErrandsResourceParameters parameters)
        {
            Parameters = parameters;
            var (errands, paginationMetadata, isSuccess) = await _repository.Errands.GetAllErrandsPaginatedAsync(parameters);
            IsSuccess = isSuccess;
            Errands = _mapper.Map<IReadOnlyList<ErrandViewModel>>(errands);
            PaginationMetadata = paginationMetadata ?? new PaginationMetadata();
        }


        public string SetSortIcon(string col)
        {
            return !string.Equals(Parameters.OrderBy?.Trim(), col.Trim(), StringComparison.CurrentCultureIgnoreCase) ? "" : Parameters.OrderDirection?.Trim().ToLower() == "desc" ? "fa-sort-up" : "fa-sort-down";
        }
        public string? SetOrder(string? col)
        {
            if (col is null)
                return null;


            if (string.Equals(Parameters.OrderBy?.Trim(), col.Trim(), StringComparison.CurrentCultureIgnoreCase))
                return Parameters.OrderDirection == "asc" ? "desc" : "asc";

            return "asc";
        }

        public class ErrandViewModel
        {
            public string Id { get; set; } = null!;
            public string Title { get; set; } = null!;
            public string ErrandStatus { get; set; } = null!;
            public string ElevatorId { get; set; } = null!;
            public string AssignedToId { get; set; } = null!;
            public string AssignedToName { get; set; } = null!;
            public string CreatedById { get; set; } = null!;
            public string CreatedByName { get; set; } = null!;
            public DateTime CreatedDateUtc { get; set; }
        }
    }
}
