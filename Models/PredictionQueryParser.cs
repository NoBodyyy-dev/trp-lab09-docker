namespace Lab07_RazorPages.Models;

// Разбор и проверка параметров выборки списка (фильтры, сортировка, страницы).
// Общий для всех точек входа, принимающих эти параметры.
public static class PredictionQueryParser
{
    public const int DefaultPageSize = 5;
    public const int MaxPageSize = 50;

    public static PredictionQuery Parse(
        string? sign, string? category, string? search,
        string? sort, string? desc, string? page, string? pageSize,
        out Dictionary<string, string[]> errors)
    {
        errors = new Dictionary<string, string[]>();

        string? normalizedSign = null;
        if (!string.IsNullOrWhiteSpace(sign))
        {
            normalizedSign = Catalogs.Normalize(Catalogs.Signs, sign);
            if (normalizedSign is null)
            {
                errors["sign"] = new[]
                {
                    $"Неизвестный знак зодиака «{sign}». Допустимые значения: " +
                    string.Join(", ", Catalogs.Signs) + ".",
                };
            }
        }

        string? normalizedCategory = null;
        if (!string.IsNullOrWhiteSpace(category))
        {
            normalizedCategory = Catalogs.Normalize(Catalogs.Categories, category);
            if (normalizedCategory is null)
            {
                errors["category"] = new[]
                {
                    $"Неизвестная категория «{category}». Допустимые значения: " +
                    string.Join(", ", Catalogs.Categories) + ".",
                };
            }
        }

        string normalizedSort = "id";
        if (!string.IsNullOrWhiteSpace(sort))
        {
            string? found = Catalogs.Normalize(Catalogs.SortFields, sort);
            if (found is null)
            {
                errors["sort"] = new[]
                {
                    $"Недопустимое поле сортировки «{sort}». Допустимые значения: " +
                    string.Join(", ", Catalogs.SortFields) + ".",
                };
            }
            else
            {
                normalizedSort = found;
            }
        }

        bool descending = false;
        if (!string.IsNullOrWhiteSpace(desc) && !bool.TryParse(desc, out descending))
        {
            errors["desc"] = new[] { "Параметр должен иметь значение true или false." };
        }

        int pageNumber = 1;
        if (!string.IsNullOrWhiteSpace(page) &&
            (!int.TryParse(page, out pageNumber) || pageNumber < 1))
        {
            errors["page"] = new[] { "Номер страницы должен быть целым числом не меньше 1." };
            pageNumber = 1;
        }

        int size = DefaultPageSize;
        if (!string.IsNullOrWhiteSpace(pageSize) &&
            (!int.TryParse(pageSize, out size) || size < 1 || size > MaxPageSize))
        {
            errors["pageSize"] = new[]
            {
                $"Размер страницы должен быть целым числом от 1 до {MaxPageSize}.",
            };
            size = DefaultPageSize;
        }

        return new PredictionQuery(
            normalizedSign, normalizedCategory, search?.Trim(),
            normalizedSort, descending, pageNumber, size);
    }

    public static Dictionary<string, string[]> ValidateSign(string? sign, out string? normalized)
    {
        normalized = null;
        if (string.IsNullOrWhiteSpace(sign))
        {
            return new Dictionary<string, string[]>();
        }

        normalized = Catalogs.Normalize(Catalogs.Signs, sign);
        return normalized is not null
            ? new Dictionary<string, string[]>()
            : new Dictionary<string, string[]>
            {
                ["sign"] = new[]
                {
                    $"Неизвестный знак зодиака «{sign}». Допустимые значения: " +
                    string.Join(", ", Catalogs.Signs) + ".",
                },
            };
    }
}
