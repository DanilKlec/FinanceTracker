using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Domain;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<TransactionService>();
services.AddSingleton<CategoryServiceNoDb>();

#region DbConnect
//var config = new ConfigurationBuilder()
//    .SetBasePath(AppContext.BaseDirectory)
//    .AddJsonFile("appsettings.json", optional: false)
//    .Build();

//var connectionString = config.GetConnectionString("Default");

services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

//services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//services.AddScoped<TransactionService>();
//services.AddScoped<CategoryService>();

var serviceProvider = services.BuildServiceProvider();

//// Применяем миграции
//using (var scope = serviceProvider.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    db.Database.Migrate();
//}

#endregion

// Получаем сервисы
var transactionService = serviceProvider.GetRequiredService<TransactionService>();
var categoryService = serviceProvider.GetRequiredService<CategoryServiceNoDb>();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Личный учёт финансов ===");
    Console.WriteLine("1. Добавить транзакцию");
    Console.WriteLine("2. Показать баланс");
    Console.WriteLine("3. Показать все транзакции");
    Console.WriteLine("4. Показать все категории");
    Console.WriteLine("5. Добавить категорию");
    Console.WriteLine("6. Выход");
    Console.Write("Выбор: ");
    var key = Console.ReadKey().Key;
    Console.WriteLine();

    switch (key)
    {
        case ConsoleKey.D1:
            AddTransaction(transactionService);
            break;

        case ConsoleKey.D2:
            var balance = transactionService.GetBalance();
            Console.WriteLine($"💰 Текущий баланс: {balance:C}");
            Pause();
            break;

        case ConsoleKey.D3:
            var transactions = transactionService.GetAll();
            Console.WriteLine("📄 Транзакции:");
            foreach (var t in transactions)
            {
                Console.WriteLine($"{t.Date:dd.MM.yyyy} | {t.Type} | {t.Amount:C} | {t.Description} | Категория ID: {t.CategoryId}");
            }
            Pause();
            break;

        case ConsoleKey.D4:
            var categories = categoryService.GetAll();
            Console.WriteLine("📁 Категории:");
            foreach (var c in categories)
            {
                Console.WriteLine($"{c.Id}. {c.Name}");
            }
            Pause();
            break;

        case ConsoleKey.D5:
            Console.Write("Введите название новой категории: ");
            var name = Console.ReadLine();
            try
            {
                categoryService.Add(name!);
                Console.WriteLine("✅ Категория добавлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Ошибка: {ex.Message}");
            }
            Pause();
            break;

        case ConsoleKey.D6:
            Console.WriteLine("👋 До свидания!");
            return;

        default:
            Console.WriteLine("❗ Неверный ввод. Попробуйте снова.");
            Pause();
            break;
    }
}

// Вспомогательные методы
void AddTransaction(TransactionService ts)
{
    try
    {
        Console.Write("Сумма: ");
        var amount = decimal.Parse(Console.ReadLine()!);

        Console.Write("Тип (1 — Доход, 2 — Расход): ");
        var type = Console.ReadLine() == "1" ? TransactionType.Income : TransactionType.Expense;

        Console.Write("Описание: ");
        var description = Console.ReadLine();

        Console.Write("ID категории: ");
        var categoryId = int.Parse(Console.ReadLine()!);

        var transaction = new Transaction
        {
            Amount = amount,
            Type = type,
            Description = description,
            Date = DateTime.Now,
            CategoryId = categoryId
        };

        ts.Add(transaction);
        Console.WriteLine("✅ Транзакция добавлена!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❗ Ошибка: {ex.Message}");
    }
    Pause();
}

void Pause()
{
    Console.WriteLine("\nНажмите любую клавишу...");
    Console.ReadKey();
}