using Lab07_RazorPages.Data;
using Lab07_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab07_RazorPages.Pages.Predictions;

public class IndexModel : PageModel
{
    private readonly IPredictionRepository _repository;

    public IndexModel(IPredictionRepository repository)
    {
        _repository = repository;
    }

    [BindProperty(SupportsGet = true)] public string? Sign { get; set; }
    [BindProperty(SupportsGet = true)] public string? Category { get; set; }
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public string? Sort { get; set; }
    [BindProperty(SupportsGet = true)] public string? Desc { get; set; }
    // "page" в Razor Pages зарезервировано под путь страницы, поэтому номер — pageNumber
    [BindProperty(SupportsGet = true)] public string? PageNumber { get; set; }
    [BindProperty(SupportsGet = true)] public string? PageSize { get; set; }

    public PredictionQuery Query { get; private set; } = null!;
    public PagedResult<Prediction> Result { get; private set; } = null!;
    public Dictionary<string, string[]> Errors { get; private set; } = new();

    public void OnGet()
    {
        Query = PredictionQueryParser.Parse(
            Sign, Category, Search, Sort, Desc, PageNumber, PageSize, out var errors);
        Errors = errors;
        Result = _repository.GetPage(Query);
    }

    // Параметры ссылок пагинации с сохранением текущих фильтров
    public Dictionary<string, string?> RouteFor(int page) => new()
    {
        ["sign"] = Query.Sign,
        ["category"] = Query.Category,
        ["search"] = Query.Search,
        ["sort"] = Query.Sort,
        ["desc"] = Query.Desc ? "true" : null,
        ["pageSize"] = Query.PageSize.ToString(),
        ["pageNumber"] = page.ToString(),
    };
}
