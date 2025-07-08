using FinanceTracker.Domain.Domain;
using FinanceTracker.Infrastructure.Data;

namespace FinanceTracker.Application.Services;

public class TransactionService
{
    private readonly AppDbContext _db;
    public TransactionService(AppDbContext db) => _db = db;

    public void Add(Transaction t)
    {
        var categoryExists = _db.Categories.Any(c => c.Id == t.CategoryId);
        if (!categoryExists)
            throw new ArgumentException($"Категория с ID '{t.CategoryId}' не найдена.");

        _db.Transactions.Add(t);
        _db.SaveChanges();
    }

    public decimal GetBalance()
    {
        var inc = _db.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var exp = _db.Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
        return inc - exp;
    }

    public List<Transaction> GetAll()
    {
        return _db.Transactions.ToList();
    }
}
