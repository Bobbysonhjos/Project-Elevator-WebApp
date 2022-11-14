using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;
using WebApp.ResourceParameters;
using WebApp.Services.Repository;

namespace WebApp.Pages.Errands;

public class DetailsModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public DetailsModel(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        Errand = new ErrandViewModel();
        PaginationMetadata = new PaginationMetadata();
        Parameters = new ErrandWithCommentsResourceParameters();
    }

    public ErrandViewModel Errand { get; set; }
    public PaginationMetadata PaginationMetadata { get; set; }
    public ErrandWithCommentsResourceParameters Parameters { get; set; }
    public bool IsSuccess { get; set; }

    [BindProperty]
    [Required]
    [StringLength(500, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} chars long")]
    public string? Comment { get; set; }


    public async Task OnGetAsync(string elevatorId, string errandId,
        [FromQuery] ErrandWithCommentsResourceParameters parameters)
    {
        await LoadData(elevatorId, errandId, parameters);
    }

    private async Task LoadData(string elevatorId, string errandId, ErrandWithCommentsResourceParameters parameters)
    {
        Parameters = parameters;
        var (errand, paginationMetadata, isSuccess) =
            await _repository.Errands.GetErrandByIdForElevatorAsync(elevatorId, errandId, parameters);

        IsSuccess = isSuccess;
        Errand = _mapper.Map<ErrandViewModel>(errand);
        PaginationMetadata = paginationMetadata ?? new PaginationMetadata();
    }

    public async Task<IActionResult> OnPostComment(string elevatorId, string errandId,
        [FromQuery] ErrandWithCommentsResourceParameters parameters)
    {
        await LoadData(elevatorId, errandId, parameters);

        if (!ModelState.IsValid)
            return Page();

        var isSuccess = await _repository.Comments.CreateCommentForErrandAsync(elevatorId, errandId, Comment!);

        if (isSuccess) return RedirectToPage();


        ModelState.AddModelError(nameof(Comment), "Could not post comment");
        return Page();
    }


    public class ErrandViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ErrandStatus { get; set; } = null!;
        public string ElevatorId { get; set; } = null!;
        public IList<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public string AssignedToId { get; set; } = null!;
        public string AssignedToName { get; set; } = null!;
        public string CreatedById { get; set; } = null!;
        public string CreatedByName { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }

    public class CommentViewModel
    {
        public string Id { get; init; } = null!;
        public string CreatedById { get; init; } = null!;
        public string CreatedByName { get; init; } = null!;
        public DateTime CreatedDateUtc { get; init; }
        public string Message { get; init; } = null!;
    }
}