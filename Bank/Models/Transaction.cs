namespace Bank.Models;
public class Transaction
{
    public DateTime Date { get; init; }
    public double Amount { get; init; } // >0 – пополнение, <0 – снятие
    public string Note { get; init; }
    public Transaction(DateTime date, double amount, TransactionType type, string note = "")
    {
         Date = date;
         Amount = amount;
         Note = note;
         Type = type;
    }
    public override string ToString() => $"{Date:yyyy-MM-dd HH:mm} | {Amount,10:C2} | {Note,-12} | {Type}";
    public TransactionType Type { get; set; }
}