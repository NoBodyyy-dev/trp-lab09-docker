using System.Globalization;

namespace Lab07_RazorPages.Models;

public record HoroscopeResult(int Day, int Month, string Sign, DateTime Date, string Forecast, int LuckyNumber);

// Логика из ЛР2 (/horoscope): знак зодиака по дате рождения и прогноз на день.
public static class Horoscope
{
    private static readonly (int Boundary, string Second, string First)[] Signs =
    {
        (20, "Водолей", "Козерог"),
        (19, "Рыбы", "Водолей"),
        (21, "Овен", "Рыбы"),
        (20, "Телец", "Овен"),
        (21, "Близнецы", "Телец"),
        (21, "Рак", "Близнецы"),
        (23, "Лев", "Рак"),
        (23, "Дева", "Лев"),
        (23, "Весы", "Дева"),
        (23, "Скорпион", "Весы"),
        (22, "Стрелец", "Скорпион"),
        (22, "Козерог", "Стрелец"),
    };

    private static readonly string[] Forecasts =
    {
        "день подходит для спокойной, методичной работы над начатым ранее",
        "звёзды советуют не распыляться и сосредоточиться на одной задаче",
        "хороший день для новых знакомств и неожиданных предложений",
        "возможны задержки в делах — закладывайте запас времени",
        "удачный день для учёбы: новая информация усваивается легко",
        "стоит навести порядок в делах и завершить давние обязательства",
        "день благоприятен для творческих задач и нестандартных решений",
    };

    public static readonly CultureInfo Russian = CultureInfo.GetCultureInfo("ru-RU");

    public static HoroscopeResult? Calculate(string? day, string? month, out string? error)
    {
        error = null;

        if (!int.TryParse(day, out int d) || !int.TryParse(month, out int m))
        {
            error = "День и месяц должны быть целыми числами.";
            return null;
        }

        if (m < 1 || m > 12)
        {
            error = "Месяц должен быть в диапазоне от 1 до 12.";
            return null;
        }

        int daysInMonth = DateTime.DaysInMonth(2024, m);
        if (d < 1 || d > daysInMonth)
        {
            error = $"Некорректный день: в месяце {m} дней не больше {daysInMonth}.";
            return null;
        }

        var (boundary, second, first) = Signs[m - 1];
        string sign = d >= boundary ? second : first;

        // Детерминированный «прогноз»: зависит от знака и дня года.
        var today = DateTime.Today;
        int seed = Catalogs.Signs.ToList().IndexOf(sign) * 37 + today.DayOfYear;

        return new HoroscopeResult(d, m, sign, today,
            Forecasts[seed % Forecasts.Length], seed % 33 + 1);
    }
}
