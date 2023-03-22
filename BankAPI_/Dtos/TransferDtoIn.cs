namespace BankAPI_.Dtos;

public class TransferDtoIn
{
    public string FromAccountNum { get; set; } = string.Empty;
    public string FromClientDocNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string ToAccountNum { get; set;} = string.Empty;
    public string ToClientDocNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}