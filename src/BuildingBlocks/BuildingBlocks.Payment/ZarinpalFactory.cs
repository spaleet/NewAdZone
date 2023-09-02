using System.Text;
using Newtonsoft.Json;

namespace BuildingBlocks.Payment;

public class ZarinPalFactory : IZarinPalFactory
{
    //private readonly IConfiguration _configuration;

    private string Prefix { get; }
    private string MerchantId { get; }

    //public ZarinPalFactory(IConfiguration configuration)
    //{
    //    _configuration = configuration;
    //    Prefix = _configuration.GetSection("payment")["method"];
    //    MerchantId = _configuration.GetSection("payment")["merchant"];
    //}

    public ZarinPalFactory()
    {
        Prefix = "sandbox";
        MerchantId = "c632f574-bd37-15e7-99ca-000c295eb9d3";
    }

    public async Task<PaymentResponse> CreatePaymentRequest(string callBackUrl, string amount, string email,
        string orderId)
    {
        amount = amount.Replace(",", "");
        int finalAmount = int.Parse(amount);

        using (var httpClient = new HttpClient())
        {
            string content = JsonConvert.SerializeObject(new PaymentRequest
            {
                Email = email,
                Mobile = "09123456789",
                CallbackURL = $"{callBackUrl}",
                Description = $"خرید سفارش کد : {orderId}",
                Amount = finalAmount,
                MerchantID = MerchantId
            });

            using (var httpResponseMessage = await httpClient.PostAsync(
                       $"https://{Prefix}.zarinpal.com/pg/rest/WebGate/PaymentRequest.json",
                       new StringContent(content, Encoding.UTF8, "application/json")))
                return JsonConvert.DeserializeObject<PaymentResponse>(await httpResponseMessage.Content
                    .ReadAsStringAsync());
        }
    }

    public async Task<VerificationResponse> CreateVerificationRequest(string authority, string amount)
    {
        amount = amount.Replace(",", "");
        int finalAmount = int.Parse(amount);

        using (var httpClient = new HttpClient())
        {
            string content = JsonConvert.SerializeObject(new VerificationRequest
            {
                MerchantID = MerchantId,
                Amount = finalAmount,
                Authority = authority
            });

            using (var httpResponseMessage = await httpClient.PostAsync(
                       $"https://{Prefix}.zarinpal.com/pg/rest/WebGate/PaymentVerification.json",
                       new StringContent(content, Encoding.UTF8, "application/json")))
                return JsonConvert.DeserializeObject<VerificationResponse>(await httpResponseMessage.Content
                    .ReadAsStringAsync());
        }
    }
}