using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Elevator
{
    public class IndexModel : PageModel
    {
        public List<ElevatorViewModel> Elevators { get; private set; }


        public string SortOrder { get; set; } = null!;
        public string SortCol { get; set; } = null!;
        public int PageNo { get; set; } 

        public int TotalPageCount { get; set; }

        public string SearchWord { get; set; } = null!;


        public class ElevatorViewModel
        {
            public Guid Id { get; set; }
            public string Location { get; set; } = null!;
            public string Status { get; set; } = null!;
        }



        public void OnGet(string searchWord, string col = "Location", string order = "asc", int pageno = 1)
        {
            PageNo = pageno;
            SearchWord = searchWord;
            SortCol = col;
            SortOrder = order;
            

            Elevators = new List<ElevatorViewModel>();
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
