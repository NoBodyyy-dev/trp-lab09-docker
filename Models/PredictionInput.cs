namespace Lab07_RazorPages.Models;

// Модель формы создания/редактирования. Поля допускают null —
// обязательность и диапазоны проверяет PredictionValidator.
public class PredictionInput
{
    public string? Sign { get; set; }
    public string? Category { get; set; }
    public string? Text { get; set; }
    public int? LuckyNumber { get; set; }

    public PredictionWriteDto ToDto() => new()
    {
        Sign = Sign,
        Category = Category,
        Text = Text,
        LuckyNumber = LuckyNumber,
    };

    public static PredictionInput From(Prediction prediction) => new()
    {
        Sign = prediction.Sign,
        Category = prediction.Category,
        Text = prediction.Text,
        LuckyNumber = prediction.LuckyNumber,
    };
}
