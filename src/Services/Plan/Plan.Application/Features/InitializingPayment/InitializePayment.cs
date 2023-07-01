namespace Plan.Application.Features.InitializingPayment;

public record InitializePayment(string SubId, decimal Price, string CallBackUrl);
