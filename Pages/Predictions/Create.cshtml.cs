using Lab07_RazorPages.Data;
using Lab07_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab07_RazorPages.Pages.Predictions;

public class CreateModel : PageModel
{
    private readonly IPredictionRepository _repository;

    public CreateModel(IPredictionRepository repository)
    {
        _repository = repository;
    }

    // Поля формы Input.Sign, Input.Text, ... привязываются при POST
    [BindProperty]
    public PredictionInput Input { get; set; } = new();

    public void OnGet(string? sign)
    {
        Input.Sign = Catalogs.Normalize(Catalogs.Signs, sign);
    }

    public IActionResult OnPost()
    {
        ModelState.AddErrors(PredictionValidator.ValidateWrite(Input.ToDto()), nameof(Input));
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var created = _repository.Add(Input.ToDto());
        TempData["Message"] = $"Предсказание № {created.Id} добавлено.";
        return RedirectToPage("Details", new { id = created.Id });
    }
}
