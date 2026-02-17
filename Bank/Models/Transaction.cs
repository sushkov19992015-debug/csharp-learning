namespace Bank.Models;
public class Transaction
{
    public DateTime Date { get; init; }
    public double Amount { get; init; } // >0 – пополнение, <0 – снятие
    public string Note { get; init; }
    public Transaction(DateTime date, double amount, string note = "")
    {
         Date = date;
         Amount = amount;
         Note = note;
    }
    public override string ToString() => $"{Date:yyyy-MM-dd HH:mm}";
}