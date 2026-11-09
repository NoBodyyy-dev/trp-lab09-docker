namespace Lab07_RazorPages.Models;

public class PredictionWriteDto
{
    public string? Sign { get; set; }
    public string? Category { get; set; }
    public string? Text { get; set; }
    public int? LuckyNumber { get; set; }
}

public class PredictionPatchDto
{
    public string? Sign { get; set; }
    public string? Category { get; set; }
    public string? Text { get; set; }
    public int? LuckyNumber { get; set; }

    public bool IsEmpty =>
        Sign is null && Category is null && Text is null && LuckyNumber is null;
}

public record PredictionQuery(
    string? Sign,
    string? Category,
    string? Search,
    string Sort,
    bool Desc,
    int Page,
    int PageSize);

public record PagedResult<T>(
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    IReadOnlyList<T> Items);
