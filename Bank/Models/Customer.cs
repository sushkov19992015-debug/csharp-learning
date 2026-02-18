namespace Bank.Models;
public class Customer
{
    public Guid Id { get; } = Guid.NewGuid();    
    public string FirstName { get; init; }
    public string LastName  { get; init; }
    public Customer(string firstName, string lastName) 
    {  
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName  = lastName  ?? throw new ArgumentNullException(nameof(lastName));
    }
    public override string ToString() => $"{FirstName} {LastName}";
}