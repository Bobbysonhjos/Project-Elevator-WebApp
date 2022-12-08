using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helpers;
using WebApp.ResourceParameters;
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
        Parameters = new ElevatorsResourceParameters();
    }

    public List<ElevatorViewModel> Elevators { get; private set; }

    public PaginationMetadata PaginationMetadata { get; set; }
    public ElevatorsResourceParameters Parameters { get; set; }
    public SelectListItem[] Filters { get; private set; }
    public string? Filter { get; private set; }

    public async Task OnGetAsync([FromQuery] ElevatorsResourceParameters parameters)
    {
        Parameters = parameters;

        var (elevators, paginationMetadata, isSuccess) =
            await _repository.Elevators.GetAllPaginatedAsync(parameters);


        if (!isSuccess)
            return;
        // TODO add error message

        Elevators = _mapper.Map<List<ElevatorViewModel>>(elevators);
        PaginationMetadata = paginationMetadata;
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
    public class ElevatorViewModel
    {
        public string Id { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }

}