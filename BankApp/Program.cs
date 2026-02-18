using System;
using Bank.Models;
using Bank.Services;

namespace BankApp;

class Program
{
    static void Main()
    {
        var service = new BankService();

        Console.WriteLine("=== Пример работы банковской библиотеки ===");

        // 1️⃣ Создаём клиентов
        var alice = new Customer("Alice", "Johnson");
        var bob   = new Customer("Bob", "Smith");

        // 2️⃣ Открываем счета
        var acc1 = service.OpenAccount("RU001", alice, 1000);
        var acc2 = service.OpenAccount("RU002", bob,   500);

        // 3️⃣ Операции
        service.Deposit("RU001", 250, "Salary");
        service.Withdraw("RU002", 100, "ATM cash");

        // 4️⃣ Вывод выписок
        Console.WriteLine("\n>>> Выписка для Alice:");
        Console.WriteLine(acc1.GetStatement());

        Console.WriteLine("\n>>> Выписка для Bob:");
        Console.WriteLine(acc2.GetStatement());

        // 5️⃣ Сортируем по балансу (пузырёк) и выводим порядок
        service.SortAccountsByBalance();
        Console.WriteLine("\n>>> Счета после сортировки по балансу:");
        foreach (var acc in service.GetAllAccounts())
        {
            Console.WriteLine($"{acc.Number} – {acc.Owner} – {acc.Balance:C2}");
        }

        // 6️⃣ Демонстрация бинарного поиска
        var found = service.FindByNumberBinary("RU001");
        Console.WriteLine(found != null
            ? $"\nСчёт найден бинарным поиском: {found.Number}, баланс {found.Balance:C2}"
            : "\nСчёт не найден бинарным поиском");

        // Начисление процентов
        Console.WriteLine("\n>>> Начисление процентов:");
        service.ApplyInterestToAll(0.05, 6); // 5% годовых на 6 месяцев
        
        Console.WriteLine("\n>>> Итоговые балансы:");
        foreach (var acc in service.GetAllAccounts())
        {
            Console.WriteLine($"{acc.Number} – {acc.Owner} – {acc.Balance:C2}");
        }

        //Сохранение в CSV
        service.ExportStatementToCsv();
    }
}