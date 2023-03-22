namespace BankAPI_.Models;

public class Bank
{
    public Guid Id { get; set; }
    public String BankCode { get; set; } = string.Empty;
    public String Name { get; set; } = string.Empty;
    public String Address { get; set; } = string.Empty;
}