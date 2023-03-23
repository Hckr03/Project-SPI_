using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankAPI_.Models;

public class Client
{
    [Key]
    public string ClientDocNum { get; set; } = string.Empty;
    public string DocType { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
}