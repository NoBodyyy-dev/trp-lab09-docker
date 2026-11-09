using Lab07_RazorPages.Data;
using Lab07_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab07_RazorPages.Pages.Predictions;

public class DetailsModel : PageModel
{
    private readonly IPredictionRepository _repository;

    public DetailsModel(IPredictionRepository repository)
    {
        _repository = repository;
    }

    public Prediction Prediction { get; private set; } = null!;

    public IActionResult OnGet(int id)
    {
        var prediction = _repository.GetById(id);
        if (prediction is null)
        {
            return NotFound();
        }

        Prediction = prediction;
        return Page();
    }
}
