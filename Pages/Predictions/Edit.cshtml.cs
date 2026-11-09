using Lab07_RazorPages.Data;
using Lab07_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab07_RazorPages.Pages.Predictions;

public class EditModel : PageModel
{
    private readonly IPredictionRepository _repository;

    public EditModel(IPredictionRepository repository)
    {
        _repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public PredictionInput Input { get; set; } = new();

    public IActionResult OnGet()
    {
        var prediction = _repository.GetById(Id);
        if (prediction is null)
        {
            return NotFound();
        }

        Input = PredictionInput.From(prediction);
        return Page();
    }

    public IActionResult OnPost()
    {
        ModelState.AddErrors(PredictionValidator.ValidateWrite(Input.ToDto()), nameof(Input));
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (_repository.Replace(Id, Input.ToDto()) is null)
        {
            return NotFound();
        }

        TempData["Message"] = $"Предсказание № {Id} сохранено.";
        return RedirectToPage("Details", new { id = Id });
    }
}
