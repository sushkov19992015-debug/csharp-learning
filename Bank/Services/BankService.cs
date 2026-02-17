using System.Collections.Generic;
using System.Linq;
using Bank.Models;

namespace Bank.Services;

/// <summary>
/// Сервис бизнес‑логики банка. Хранит все счета в памяти.
/// </summary>
public class BankService
{
    private readonly List<Account> _accounts = new();

    // ----- CRUD для счетов -----
    public Account OpenAccount(string number, Customer owner, double initialBalance = 0)
    {
        if (_accounts.Any(a => a.Number == number))
            throw new ArgumentException($"Счёт с номером {number} уже существует.");

        var acc = new Account(number, owner, initialBalance);
        _accounts.Add(acc);
        return acc;
    }

    public Account GetAccount(string number)
        => _accounts.FirstOrDefault(a => a.Number == number)
           ?? throw new KeyNotFoundException($"Счёт {number} не найден.");

    public IReadOnlyList<Account> GetAllAccounts() => _accounts;

    // ----- поиск и сортировка -----
    public Account FindByNumberLinear(string number)   // линейный поиск
        => _accounts.FirstOrDefault(a => a.Number == number);

    public Account FindByNumberBinary(string number)   // бинарный поиск, требует сортировки
    {
        var sorted = _accounts.OrderBy(a => a.Number).ToArray();
        int left = 0, right = sorted.Length - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int cmp = string.Compare(sorted[mid].Number, number, StringComparison.Ordinal);
            if (cmp == 0) return sorted[mid];
            if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }
        return null; // не найдено
    }

    public void SortAccountsByBalance()
    {
        // простая сортировка «пузырьком», чтобы продемонстрировать алгоритм
        for (int i = 0; i < _accounts.Count - 1; i++)
        {
            for (int j = 0; j < _accounts.Count - i - 1; j++)
            {
                if (_accounts[j].Balance > _accounts[j + 1].Balance)
                {
                    var tmp = _accounts[j];
                    _accounts[j] = _accounts[j + 1];
                    _accounts[j + 1] = tmp;
                }
            }
        }
    }

    // ----- операции над счётом -----
    public void Deposit(string accountNumber, double amount, string note = "Deposit")
    {
        var acc = GetAccount(accountNumber);
        acc.Deposit(amount, note);
    }

    public void Withdraw(string accountNumber, double amount, string note = "Withdraw")
    {
        var acc = GetAccount(accountNumber);
        acc.Withdraw(amount, note);
    }
}