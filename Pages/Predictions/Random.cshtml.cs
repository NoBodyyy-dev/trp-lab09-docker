using Lab07_RazorPages.Data;
using Lab07_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab07_RazorPages.Pages.Predictions;

public class RandomModel : PageModel
{
    private readonly IPredictionRepository _repository;

    public RandomModel(IPredictionRepository repository)
    {
        _repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public string? Sign { get; set; }

    public Prediction? Prediction { get; private set; }

    public void OnGet()
    {
        Sign = Catalogs.Normalize(Catalogs.Signs, Sign);
        Prediction = _repository.GetRandom(Sign);
    }
}
