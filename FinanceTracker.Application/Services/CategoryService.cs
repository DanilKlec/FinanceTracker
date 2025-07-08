using FinanceTracker.Domain.Domain;
using FinanceTracker.Infrastructure.Data;

namespace FinanceTracker.Application.Services;

public class CategoryService
{
    private readonly AppDbContext _db;
    public CategoryService(AppDbContext db) => _db = db;

    public List<Category> GetAll()
    {
        return _db.Categories.ToList();
    }
}
