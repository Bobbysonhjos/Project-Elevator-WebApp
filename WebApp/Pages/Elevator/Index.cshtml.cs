using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helpers;
using WebApp.Services.Repository;

namespace WebApp.Pages.Elevator;

[BindProperties]
public class IndexModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public IndexModel(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        Elevators = new List<ElevatorViewModel>();
        Filters = new SelectListItem[]
        {
            new() { Text = "None", Value = null }, new() { Text = "Enabled", Value = "enabled" },
            new() { Text = "Disabled", Value = "disabled" }, new() { Text = "Error", Value = "error" }
        };
        PaginationMetadata = new PaginationMetadata();
    }

    public class ElevatorViewModel
    {
        public string Id { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }

    public List<ElevatorViewModel> Elevators { get; private set; }

    public PaginationMetadata PaginationMetadata { get; private set; }

    public SelectListItem[] Filters { get; private set; }


    public string? Order { get; private set; }
    public string? Column { get; private set; }
    public string? SearchQuery { get; private set; }
    public string? Filter { get; private set; }
    public string? OrderBy { get; private set; }
    public int PageSize { get; private set; }

    public async Task OnGetAsync(string? filter, string? column, string? order, string? searchQuery,
        int currentPage = 1, int pageSize = 10)
    {

        if (SearchQuery != searchQuery)
            currentPage = 1;

        Filter = filter?.Trim().ToLower() == "none" ? null : filter;
        SearchQuery = !string.IsNullOrEmpty(searchQuery) ? searchQuery : null;
        PageSize = pageSize < 1 ? 10 : pageSize > 20 ? 20 : pageSize;
        Order = order ?? "asc";
        Column = column ?? "id";
        OrderBy = column is not null ? $"{column},{order}" : null;

        var (elevators, paginationMetadata, isSuccess) =
            await _repository.Elevators.GetAllAsync(searchQuery: SearchQuery, filter: Filter, currentPage: currentPage,
                pageSize: PageSize, orderBy: OrderBy);


        if (!isSuccess)
            return;
        // TODO add error message

        Elevators = _mapper.Map<List<ElevatorViewModel>>(elevators);
        PaginationMetadata = paginationMetadata;
    }

    public string SetSortIcon(string col)
    {
        return !string.Equals(Column?.Trim(), col.Trim(), StringComparison.CurrentCultureIgnoreCase) ? "" : Order?.Trim().ToLower() == "desc" ? "fa-sort-up" : "fa-sort-down";
    }
    public string? SetOrder(string? col)
    {
        if (col is null)
            return null;


        if (string.Equals(Column?.Trim(), col.Trim(), StringComparison.CurrentCultureIgnoreCase))
            return Order == "asc" ? "desc" : "asc";

        return "asc";
    }
}