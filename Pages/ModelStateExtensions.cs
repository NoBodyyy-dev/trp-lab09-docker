using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lab07_RazorPages.Pages;

public static class ModelStateExtensions
{
    // Переносит ошибки PredictionValidator ("sign", "luckyNumber", "body")
    // в ModelState под именами полей формы ("Input.Sign", "Input.LuckyNumber", "").
    // Поле, для которого уже есть ошибка привязки, повторно не помечается.
    public static void AddErrors(
        this ModelStateDictionary modelState, Dictionary<string, string[]> errors, string prefix)
    {
        foreach (var (field, messages) in errors)
        {
            string key = field == "body"
                ? string.Empty
                : $"{prefix}.{char.ToUpperInvariant(field[0])}{field[1..]}";

            if (modelState.TryGetValue(key, out var entry) && entry.Errors.Count > 0)
            {
                continue;
            }

            foreach (string message in messages)
            {
                modelState.AddModelError(key, message);
            }
        }
    }
}
