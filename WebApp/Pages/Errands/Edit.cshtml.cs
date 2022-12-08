using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static WebApp.Pages.Errands.CreateModel;
using WebApp.Models;
using WebApp.Services.Repository;
using System.Collections.Generic;

namespace WebApp.Pages.Errands
{
    public class EditModel : PageModel
    {
        private readonly IRepository _repository;


        public EditModel(IRepository repository)
        {
            _repository = repository;
            Technicians = new SelectList(new List<Technician>(), "Id", "Name");
            Statuses = new SelectList(new Dictionary<string, string>(), "Key", "Value");
        }

        public SelectList Statuses { get; set; } = null!;

        [BindProperty]
        [Required]
        public UpdateErrandModel UpdateErrand { get; set; } = null!;



        [BindProperty(SupportsGet = true)]
        public string ElevatorId { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public string ErrandId { get; set; } = null!;


        public SelectList Technicians { get; set; }

        public class Technician
        {
            public string Id { get; set; } = null!;
            public string Name { get; set; } = null!;
        }

        public async Task<IActionResult> OnGetAsync(string elevatorId, string errandId)
        {
            var isSuccess = await Initialize();

            if (!isSuccess)
                return RedirectToPage("/Errands/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string elevatorId, string errandId)
        {
            var isSuccess = false;
            if (!ModelState.IsValid)
            {
                isSuccess = await Initialize();
                if (!isSuccess)
                    return RedirectToPage("/Errands/Index");

                return Page();
            }

            isSuccess = await _repository.Errands.UpdateErrandForElevatorAsync(ElevatorId, ErrandId, UpdateErrand.Title,
                UpdateErrand.Description, UpdateErrand.ErrandStatus, UpdateErrand.AssignedToId);

            if (!isSuccess)
                ModelState.AddModelError("Errand", "Could not update errand");


            if (!ModelState.IsValid)
            {
                isSuccess = await Initialize();
                if (!isSuccess)
                    return RedirectToPage("/Errands/Index");

                return Page();
            }
            return RedirectToPage("./Details", new { ElevatorId, ErrandId });
        }


        public async Task<bool> Initialize()
        {

            var dictionary = new Dictionary<string, string>
            {
                {"new", "New"},
                {"inprogress", "In Progress"},
                {"completed", "Completed"}
            };

            Statuses = new SelectList(dictionary, "Key", "Value");

            var techs = (await _repository.Users.GetAllUsersId("technician")).Select(x => new Technician()
            {
                Id = x.Id,
                Name = x.FirstName + " " + x.LastName,
            });
            Technicians = new SelectList(techs, "Id", "Name");

            var (errand, isSuccess) = await _repository.Errands.GetErrandById(ElevatorId, ErrandId);

            if (!isSuccess)
                return false;


            UpdateErrand = new UpdateErrandModel()
            {
                Description = errand.Description,
                AssignedToId = errand.AssignedToId,
                ErrandStatus = errand.ErrandStatus,
                Title = errand.Title
            };

            return true;
        }


        public class UpdateErrandModel
        {
            [Required]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} letters long")]
            public string Title { get; set; } = null!;

            [Required]
            [StringLength(500, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} letters long")]
            public string Description { get; set; } = null!;

            [Required(ErrorMessage = "You need to select a technician")]
            public string AssignedToId { get; set; } = null!;

            [RegularExpression("new|inprogress|completed", ErrorMessage = "Status must be new, inprogress, completed.")]
            [Required(ErrorMessage = "You need to select a status")]
            public string ErrandStatus { get; set; } = null!;
        }
    }
}
