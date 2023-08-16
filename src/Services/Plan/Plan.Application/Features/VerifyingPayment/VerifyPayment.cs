using System.Globalization;
using BuildingBlocks.Core.Utilities;
using BuildingBlocks.Payment;
using MongoDB.Driver;

namespace Plan.Application.Features.VerifyingPayment;

public record VerifyPayment(string SubId, string Authority) : ICommand<VerifyPaymentResponse>;

public class VerifyPaymentValidator : AbstractValidator<VerifyPayment>
{
    public VerifyPaymentValidator()
    {
        RuleFor(x => x.SubId).NotEmpty().WithMessage("آیدی معتبر وارد کنید");
        RuleFor(x => x.Authority).NotEmpty().WithMessage("شناسه پرداخت را وارد کنید");
    }
}

public class VerifyPaymentHandler : ICommandHandler<VerifyPayment, VerifyPaymentResponse>
{
    private readonly PlanDbContext _context;
    private readonly IZarinPalFactory _zarinPalFactory;

    public VerifyPaymentHandler(PlanDbContext context, IZarinPalFactory zarinPalFactory)
    {
        _context = context;
        _zarinPalFactory = zarinPalFactory;
    }

    public async Task<VerifyPaymentResponse> Handle(VerifyPayment request, CancellationToken cancellationToken)
    {
        var subscription = _context.PlanSubscription.AsQueryable().FirstOrDefault(x => x.Id == request.SubId);

        if (subscription is null)
            throw new NotFoundException("اشتراک مورد نظر پیدا نشد");

        var verificationResponse = await _zarinPalFactory.CreateVerificationRequest(request.Authority,
                    subscription.Price.ToString(CultureInfo.InvariantCulture));

        if (!(verificationResponse.Status >= 100))
            throw new BadRequestException("پرداخت با موفقیت انجام نشد. درصورت کسر وجه از حساب، مبلغ تا 24 ساعت دیگر به حساب شما بازگردانده خواهد شد.");

        subscription.State = PlanSubscriptionState.Subscribed;
        subscription.SubscriptionStart = DateTime.Now;
        subscription.SubscriptionExpire = DateTime.Now.AddMonths(1);

        subscription.RefId = verificationResponse.RefID;
        subscription.IssueTrackingNo = Generator.IssueTrackingCode();

        await _context.PlanSubscription.ReplaceOneAsync(
            Builders<PlanSubscription>.Filter.Eq(x => x.Id, subscription.Id),
            subscription);

        return new VerifyPaymentResponse(subscription.IssueTrackingNo);
    }
}