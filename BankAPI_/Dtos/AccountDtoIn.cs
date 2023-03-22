namespace BankAPI_.Dtos;

public class AccountDtoIn
{
    public string AccountNum { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string ClientDocNum { get; set; } = string.Empty;
    public decimal Balance { get; set; } = default;
    public string BankCode { get; set; } = string.Empty;
}