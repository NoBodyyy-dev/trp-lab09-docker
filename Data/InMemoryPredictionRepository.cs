using System.Collections.Concurrent;
using Lab07_RazorPages.Models;

namespace Lab07_RazorPages.Data;

public class InMemoryPredictionRepository : IPredictionRepository
{
    private readonly ConcurrentDictionary<int, Prediction> _items = new();
    private int _lastId;

    public InMemoryPredictionRepository()
    {
        Seed();
    }

    public int Count => _items.Count;

    public PagedResult<Prediction> GetPage(PredictionQuery query)
    {
        IEnumerable<Prediction> items = _items.Values;

        if (!string.IsNullOrWhiteSpace(query.Sign))
        {
            items = items.Where(p => string.Equals(
                p.Sign, query.Sign, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            items = items.Where(p => string.Equals(
                p.Category, query.Category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            items = items.Where(p => p.Text.Contains(
                query.Search, StringComparison.OrdinalIgnoreCase));
        }

        items = query.Sort switch
        {
            "sign" => Order(items, p => p.Sign, query.Desc),
            "category" => Order(items, p => p.Category, query.Desc),
            "number" => Order(items, p => p.LuckyNumber, query.Desc),
            _ => Order(items, p => p.Id, query.Desc),
        };

        var filtered = items.ToList();
        int totalItems = filtered.Count;
        int totalPages = totalItems == 0
            ? 0
            : (int)Math.Ceiling(totalItems / (double)query.PageSize);

        var page = filtered
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return new PagedResult<Prediction>(
            query.Page, query.PageSize, totalItems, totalPages, page);
    }

    public Prediction? GetById(int id) =>
        _items.TryGetValue(id, out var prediction) ? prediction : null;

    public Prediction Add(PredictionWriteDto dto)
    {
        int id = Interlocked.Increment(ref _lastId);
        var prediction = new Prediction(
            id,
            Catalogs.Normalize(Catalogs.Signs, dto.Sign)!,
            Catalogs.Normalize(Catalogs.Categories, dto.Category)!,
            dto.Text!.Trim(),
            dto.LuckyNumber!.Value,
            DateTime.Now,
            null);

        _items[id] = prediction;
        return prediction;
    }

    public Prediction? Replace(int id, PredictionWriteDto dto)
    {
        if (!_items.TryGetValue(id, out var existing))
        {
            return null;
        }

        var updated = existing with
        {
            Sign = Catalogs.Normalize(Catalogs.Signs, dto.Sign)!,
            Category = Catalogs.Normalize(Catalogs.Categories, dto.Category)!,
            Text = dto.Text!.Trim(),
            LuckyNumber = dto.LuckyNumber!.Value,
            UpdatedAt = DateTime.Now,
        };

        _items[id] = updated;
        return updated;
    }

    public Prediction? Patch(int id, PredictionPatchDto dto)
    {
        if (!_items.TryGetValue(id, out var existing))
        {
            return null;
        }

        var updated = existing with
        {
            Sign = Catalogs.Normalize(Catalogs.Signs, dto.Sign) ?? existing.Sign,
            Category = Catalogs.Normalize(Catalogs.Categories, dto.Category)
                       ?? existing.Category,
            Text = string.IsNullOrWhiteSpace(dto.Text) ? existing.Text : dto.Text.Trim(),
            LuckyNumber = dto.LuckyNumber ?? existing.LuckyNumber,
            UpdatedAt = DateTime.Now,
        };

        _items[id] = updated;
        return updated;
    }

    public bool Delete(int id) => _items.TryRemove(id, out _);

    public Prediction? GetRandom(string? sign)
    {
        var pool = string.IsNullOrWhiteSpace(sign)
            ? _items.Values.ToList()
            : _items.Values
                .Where(p => string.Equals(p.Sign, sign, StringComparison.OrdinalIgnoreCase))
                .ToList();

        return pool.Count == 0 ? null : pool[Random.Shared.Next(pool.Count)];
    }

    private static IEnumerable<Prediction> Order<TKey>(
        IEnumerable<Prediction> source, Func<Prediction, TKey> key, bool desc) =>
        desc ? source.OrderByDescending(key) : source.OrderBy(key);

    private void Seed()
    {
        (string Sign, string Category, string Text, int Number)[] seed =
        {
            ("Овен", "общий", "День подходит для решительных действий и новых начинаний.", 7),
            ("Овен", "работа", "Усилия последних дней будут замечены руководителем.", 14),
            ("Телец", "здоровье", "Не забывайте о перерывах: короткая прогулка вернёт концентрацию.", 3),
            ("Близнецы", "любовь", "Короткий разговор с близким человеком укрепит отношения.", 21),
            ("Рак", "общий", "Звёзды советуют сосредоточиться на одной главной задаче.", 9),
            ("Лев", "работа", "Командная работа принесёт больше результата, чем действия в одиночку.", 33),
            ("Дева", "здоровье", "Полезно раньше лечь спать: организм нуждается в восстановлении.", 5),
            ("Весы", "любовь", "Хороший момент, чтобы сделать первый шаг навстречу.", 12),
            ("Скорпион", "общий", "Удачный день для учёбы: новая информация усваивается легко.", 28),
            ("Стрелец", "работа", "Финансовые решения лучше отложить до конца недели.", 17),
        };

        foreach (var (sign, category, text, number) in seed)
        {
            int id = Interlocked.Increment(ref _lastId);
            _items[id] = new Prediction(
                id, sign, category, text, number, DateTime.Now, null);
        }
    }
}
