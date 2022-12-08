using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helpers;
using WebApp.ResourceParameters;
using WebApp.Services.Repository;

namespace WebApp.Pages.User;

[BindProperties]
public class IndexModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public IndexModel(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        Users = new List<UserViewModel>();
        Filters = new SelectListItem[]
        {
                new() { Text = "Roles", Value = null }, new() { Text = "Admin", Value = "admin" },
                new() { Text = "SecondLineTech", Value = "secondlinetech" },
                new() { Text = "ServiceTech", Value = "servicetech" }
        };
        PaginationMetadata = new PaginationMetadata();
        Parameters = new UserResourceParameters();

    }
    public List<UserViewModel> Users { get; private set; }
    public SelectListItem[] Filters { get; private set; }
    public string? Filter { get; private set; }
    public PaginationMetadata PaginationMetadata { get; set; }
    public UserResourceParameters Parameters { get; set; }

    public async Task OnGetAsync([FromQuery] UserResourceParameters 
        parameters)
    {
        Parameters = parameters;


        var (users, paginationMetadata, isSuccess) =
            await _repository.Users.GetAllPaginatedAsync(parameters);

        if (!isSuccess)
            return;

        Users = _mapper.Map<List<UserViewModel>>(users);
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
    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}




