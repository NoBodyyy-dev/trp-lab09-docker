namespace Lab07_RazorPages.Models;

public static class Catalogs
{

    public static readonly string[] Signs =
    {
        "Овен", "Телец", "Близнецы", "Рак", "Лев", "Дева",
        "Весы", "Скорпион", "Стрелец", "Козерог", "Водолей", "Рыбы",
    };

    public static readonly string[] Categories =
    {
        "общий", "любовь", "работа", "здоровье",
    };

    public static readonly string[] SortFields =
    {
        "id", "sign", "category", "number",
    };

    public static string? Normalize(string[] catalog, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string trimmed = value.Trim();
        foreach (string item in catalog)
        {
            if (string.Equals(item, trimmed, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }
        }

        return null;
    }
}
