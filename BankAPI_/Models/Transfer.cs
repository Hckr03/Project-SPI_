using System.Text.Json.Serialization;

namespace BankAPI_.Models;

public class Transfer
{
    public Guid Id { get; set; }
    public Account Account { get; set; } = new Account();
    [JsonIgnore]
    public Client Client { get; set; } = new Client();
    public DateTime Date { get; set; } = new DateTime();
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}