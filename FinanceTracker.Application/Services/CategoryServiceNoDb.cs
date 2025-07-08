using FinanceTracker.Domain.Domain;

namespace FinanceTracker.Application.Services;

public class CategoryServiceNoDb
{

    private readonly List<Category> _categories = new()
    {
        new() { Id = 1, Name = "Еда" },
        new() { Id = 2, Name = "Транспорт" },
        new() { Id = 3, Name = "Зарплата" }
    };

    public IEnumerable<Category> GetAll() => _categories;

    public void Add(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название категории не может быть пустым.");

        if (_categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Категория с названием '{name}' уже существует.");

        int newId = _categories.Count == 0 ? 1 : _categories.Max(c => c.Id) + 1;
        _categories.Add(new Category { Id = newId, Name = name });
    }
}
