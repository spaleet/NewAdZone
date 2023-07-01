namespace BuildingBlocks.Payment;

public interface IZarinPalFactory
{
    Task<PaymentResponse> CreatePaymentRequest(string callBackUrl, string amount, string email, string orderId);

    Task<VerificationResponse> CreateVerificationRequest(string authority, string price);
}