namespace FinanceTracker.Domain.Domain;

public enum TransactionType { Income, Expense }

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public TransactionType Type { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
