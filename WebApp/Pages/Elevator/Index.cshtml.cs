using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Elevator
{
    public class IndexModel : PageModel
    {
        [BindProperty] public Models.Elevator Elevator { get; set; } = null!;
        public void OnGet()
        {
            
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
