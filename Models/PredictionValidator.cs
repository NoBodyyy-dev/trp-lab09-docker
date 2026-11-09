namespace Lab07_RazorPages.Models;

public static class PredictionValidator
{
    public const int TextMinLength = 10;
    public const int TextMaxLength = 300;
    public const int LuckyNumberMin = 1;
    public const int LuckyNumberMax = 99;

    public static Dictionary<string, string[]> ValidateWrite(PredictionWriteDto? dto)
    {
        var errors = new Dictionary<string, List<string>>();

        if (dto is null)
        {
            Add(errors, "body", "Тело запроса пустое или не является JSON-объектом.");
            return Freeze(errors);
        }

        ValidateSign(errors, dto.Sign, required: true);
        ValidateCategory(errors, dto.Category, required: true);
        ValidateText(errors, dto.Text, required: true);
        ValidateLuckyNumber(errors, dto.LuckyNumber, required: true);

        return Freeze(errors);
    }

    public static Dictionary<string, string[]> ValidatePatch(PredictionPatchDto? dto)
    {
        var errors = new Dictionary<string, List<string>>();

        if (dto is null)
        {
            Add(errors, "body", "Тело запроса пустое или не является JSON-объектом.");
            return Freeze(errors);
        }

        if (dto.IsEmpty)
        {
            Add(errors, "body", "Не передано ни одного изменяемого поля.");
            return Freeze(errors);
        }

        ValidateSign(errors, dto.Sign, required: false);
        ValidateCategory(errors, dto.Category, required: false);
        ValidateText(errors, dto.Text, required: false);
        ValidateLuckyNumber(errors, dto.LuckyNumber, required: false);

        return Freeze(errors);
    }

    private static void ValidateSign(
        Dictionary<string, List<string>> errors, string? value, bool required)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            if (required)
            {
                Add(errors, "sign", "Поле обязательно для заполнения.");
            }

            return;
        }

        if (Catalogs.Normalize(Catalogs.Signs, value) is null)
        {
            Add(errors, "sign",
                $"Неизвестный знак зодиака «{value}». Допустимые значения: " +
                string.Join(", ", Catalogs.Signs) + ".");
        }
    }

    private static void ValidateCategory(
        Dictionary<string, List<string>> errors, string? value, bool required)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            if (required)
            {
                Add(errors, "category", "Поле обязательно для заполнения.");
            }

            return;
        }

        if (Catalogs.Normalize(Catalogs.Categories, value) is null)
        {
            Add(errors, "category",
                $"Неизвестная категория «{value}». Допустимые значения: " +
                string.Join(", ", Catalogs.Categories) + ".");
        }
    }

    private static void ValidateText(
        Dictionary<string, List<string>> errors, string? value, bool required)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            if (required)
            {
                Add(errors, "text", "Поле обязательно для заполнения.");
            }

            return;
        }

        int length = value.Trim().Length;
        if (length < TextMinLength || length > TextMaxLength)
        {
            Add(errors, "text",
                $"Длина текста должна быть от {TextMinLength} до {TextMaxLength} " +
                $"символов (получено {length}).");
        }
    }

    private static void ValidateLuckyNumber(
        Dictionary<string, List<string>> errors, int? value, bool required)
    {
        if (value is null)
        {
            if (required)
            {
                Add(errors, "luckyNumber", "Поле обязательно для заполнения.");
            }

            return;
        }

        if (value < LuckyNumberMin || value > LuckyNumberMax)
        {
            Add(errors, "luckyNumber",
                $"Значение должно быть в диапазоне от {LuckyNumberMin} до {LuckyNumberMax}.");
        }
    }

    private static void Add(
        Dictionary<string, List<string>> errors, string field, string message)
    {
        if (!errors.TryGetValue(field, out var list))
        {
            list = new List<string>();
            errors[field] = list;
        }

        list.Add(message);
    }

    private static Dictionary<string, string[]> Freeze(
        Dictionary<string, List<string>> errors) =>
        errors.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
}
