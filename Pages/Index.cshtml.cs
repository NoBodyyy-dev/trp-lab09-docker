using Lab07_RazorPages.Data;
using Lab07_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab07_RazorPages.Pages;

public class IndexModel : PageModel
{
    private readonly IPredictionRepository _repository;

    public IndexModel(IPredictionRepository repository)
    {
        _repository = repository;
    }

    // Параметры GET-запроса (?day=15&month=3) привязываются к свойствам
    [BindProperty(SupportsGet = true)]
    public string? Day { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Month { get; set; }

    public HoroscopeResult? Result { get; private set; }
    public Prediction? Prediction { get; private set; }
    public string? Error { get; private set; }

    public void OnGet()
    {
        if (Day is null && Month is null)
        {
            return;
        }

        Result = Horoscope.Calculate(Day, Month, out string? error);
        Error = error;

        if (Result is not null)
        {
            Prediction = _repository.GetRandom(Result.Sign);
        }
    }
}
