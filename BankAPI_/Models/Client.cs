using System.ComponentModel.DataAnnotations;

namespace BankAPI_.Models;

public class Client
{
    [Key]
    public string ClientDocNum { get; set; } = string.Empty;
    public string DocType { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
}