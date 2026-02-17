using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Models;

/// <summary>
/// Банковский счёт. Хранит список транзакций и текущий баланс.
/// </summary>
public class Account
{
    // ---------- свойства ----------
    public Guid   Id      { get; } = Guid.NewGuid();
    public string Number  { get; init; }          // например "RU00123456789"
    public Customer Owner { get; init; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyList<Transaction> Transactions => _transactions;

    public double Balance { get; private set; }

    // ---------- конструктор ----------
    public Account(string number, Customer owner, double initialBalance = 0)
    {
        Number = number ?? throw new ArgumentNullException(nameof(number));
        Owner  = owner  ?? throw new ArgumentNullException(nameof(owner));

        if (initialBalance != 0)
        {
            Deposit(initialBalance, "Initial deposit");
        }
    }

    // ---------- операции ----------
    /// <summary>
    /// Пополнение счёта.
    /// </summary>
    public void Deposit(double amount, string note = "Deposit")
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма должна быть > 0", nameof(amount));

        Balance += amount;
        _transactions.Add(new Transaction(DateTime.UtcNow, amount, note));
    }

    /// <summary>
    /// Снятие средств со счёта.
    /// </summary>
    public void Withdraw(double amount, string note = "Withdraw")
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма должна быть > 0", nameof(amount));

        if (amount > Balance)
            throw new InvalidOperationException("Недостаточно средств");

        Balance -= amount;
        _transactions.Add(new Transaction(DateTime.UtcNow, -amount, note));
    }

    // ---------- выписка ----------
    /// <summary>
    /// Формирует текстовую выписку по всем транзакциям счёта.
    /// </summary>
    public string GetStatement()
    {
        var sb = new StringBuilder();

        // заголовок
        sb.AppendLine($"Счёт № {Number} (владелец: {Owner})");
        sb.AppendLine($"Текущий баланс: {Balance:C2}");
        sb.AppendLine();

        // таблица транзакций
        sb.AppendLine("Дата                |      Сумма      | Примечание");
        sb.AppendLine(new string('-', 50));

        foreach (var tr in _transactions)
        {
            sb.AppendLine(tr.ToString());
        }

        return sb.ToString();
    }
}