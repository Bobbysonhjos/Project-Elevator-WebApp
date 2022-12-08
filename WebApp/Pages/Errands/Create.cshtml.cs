using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using WebApp.Models;
using WebApp.Pages.Account;
using WebApp.Services.Repository;
using static WebApp.Pages.Errands.CreateModel;

namespace WebApp.Pages.Errands
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repository;

        public CreateModel(IRepository repository)
        {
            _repository = repository;
            Technicians = new SelectList(new List<Technician>(), "Id", "Name");
            Elevators = new SelectList(new List<Elevator>(), "Id", "Location");
        }

        [BindProperty]
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} letters long")]
        public string Title { get; set; } = null!;

        [BindProperty]
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} letters long")]
        public string Description { get; set; } = null!;

        [BindProperty]
        [Required(ErrorMessage = "You need to select a technician")]
        public string AssignedToId { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public string? ElevatorId { get; set; }

        public class Technician
        {
            public string Id { get; set; } = null!;
            public string Name { get; set; } = null!;
        }
        public class Elevator
        {
            public string Id { get; set; } = null!;
            public string Location { get; set; } = null!;
        }


        public SelectList Elevators { get; set; }

        public SelectList Technicians { get; set; }

        public async Task OnGetAsync(string? elevatorId = null)
        {
            await Initialize();
        }

        public bool RoutedElevatorId { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(ElevatorId))
                ModelState.AddModelError(nameof(ElevatorId), "You need to select an elevator");



            if (!ModelState.IsValid)
            {
                await Initialize();
                return Page();
            }

            var (errand, isSuccess) = await _repository.Errands.CreateErrandForElevatorAsync(ElevatorId!, Title, Description, AssignedToId);
            if (!isSuccess)
            {
                ModelState.AddModelError("Errand", "Could not create errand");
                return Page();
            }

            return RedirectToPage("./Details", new { ElevatorId, ErrandId = errand.Id });
        }


        public async Task Initialize()
        {
            var techs = (await _repository.Users.GetAllUsersId("technician")).Select(x => new Technician()
            {
                Id = x.Id,
                Name = x.FirstName + " " + x.LastName,
            });
            Technicians = new SelectList(techs, "Id", "Name");
            var elevators = ((await _repository.Elevators.GetAllElevatorIds() ?? Array.Empty<ElevatorIdDto>())).Select(x => new Elevator()
            {
                Id = x.Id,
                Location = x.Location,
            });
            Elevators = new SelectList(elevators, "Id", "Location");

            if (!string.IsNullOrEmpty(ElevatorId) && Elevators.FirstOrDefault(x => x.Value == ElevatorId) is not null)
                RoutedElevatorId = true;
        }
    }
}
