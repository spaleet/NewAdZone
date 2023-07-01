namespace BuildingBlocks.Payment;

public class VerificationRequest
{
    public int Amount { get; set; }
    public string MerchantID { get; set; }
    public string Authority { get; set; }
}