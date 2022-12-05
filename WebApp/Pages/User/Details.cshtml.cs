using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;
using WebApp.Services.Repository;

namespace WebApp.Pages.User;

public class DetailsModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public DetailsModel(IMapper mapper, IRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
         User = new UserViewModel();
         

    }

    public UserViewModel User { get; set; }
    public bool IsSuccess { get; set; }
    

    
    public async Task<IActionResult> OnGetAsync(string Role)
    {
        return Page();
    }

    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int PhoneNumber { get; set; } 
    }
}

