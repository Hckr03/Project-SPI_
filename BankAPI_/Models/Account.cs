namespace BankAPI_.Models;

public class Account
{
    public Guid Id { get; set; }
    public string AccountNum { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public Client Client { get; set; } = new Client();
    public decimal Balance { get; set; } = default;
    public Bank Bank { get; set; } = new Bank();
    
}