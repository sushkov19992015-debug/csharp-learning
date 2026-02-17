using System;
using System.Collections.Generic;
using Bank.Models;
using Bank.Services;
using Xunit;

public class BankServiceTests
{
    private readonly BankService _service;
    private readonly Customer _alice;
    private readonly Customer _bob;

    public BankServiceTests()
    {
        _service = new BankService();
        _alice = new Customer("Alice", "Johnson");
        _bob   = new Customer("Bob", "Smith");
        _service.OpenAccount("RU001", _alice, 1000);
        _service.OpenAccount("RU002", _bob,   500);
    }

    [Fact]
    public void OpenAccount_ShouldCreateAccount_WithCorrectBalance()
    {
        var acc = _service.GetAccount("RU001");
        Assert.Equal(1000, acc.Balance);
        Assert.Equal(_alice, acc.Owner);
    }

    [Fact]
    public void Deposit_IncreasesBalance()
    {
        _service.Deposit("RU001", 200);
        var acc = _service.GetAccount("RU001");
        Assert.Equal(1200, acc.Balance);
    }

    [Fact]
    public void Withdraw_DecreasesBalance()
    {
        _service.Withdraw("RU002", 100);
        var acc = _service.GetAccount("RU002");
        Assert.Equal(400, acc.Balance);
    }

    [Fact]
    public void Withdraw_Throws_WhenInsufficientFunds()
    {
        Assert.Throws<InvalidOperationException>(() => _service.Withdraw("RU002", 600));
    }

    [Fact]
    public void FindByNumberLinear_ReturnsCorrectAccount()
    {
        var acc = _service.FindByNumberLinear("RU001");
        Assert.NotNull(acc);
        Assert.Equal(_alice, acc.Owner);
    }

    [Fact]
    public void FindByNumberBinary_ReturnsCorrectAccount()
    {
        var acc = _service.FindByNumberBinary("RU002");
        Assert.NotNull(acc);
        Assert.Equal(_bob, acc.Owner);
    }

    [Fact]
    public void SortAccountsByBalance_SortsAscending()
    {
        // Добавляем счёт с промежуточным балансом
        _service.OpenAccount("RU003", new Customer("Charlie", "Doe"), 750);
        _service.SortAccountsByBalance();

        var all = _service.GetAllAccounts();
        var balances = new List<double>(new double[all.Count]);
        for (int i = 0; i < all.Count; i++) balances[i] = all[i].Balance;

        // Проверяем, что массив отсортирован по возрастанию
        for (int i = 0; i < balances.Count - 1; i++)
            Assert.True(balances[i] <= balances[i + 1]);
    }
}