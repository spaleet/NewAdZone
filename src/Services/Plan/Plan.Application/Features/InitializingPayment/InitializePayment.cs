using System.Globalization;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Commands;
using BuildingBlocks.Core.Exceptions.Base;
using BuildingBlocks.Payment;
using FluentValidation;
using MediatR;
using MongoDB.Driver;
using Plan.Application.Features.CreatingNewPlan;
using Plan.Application.Features.SubscribingPlan;
using Plan.Domain.Entities;
using Plan.Infrastructure.Context;

namespace Plan.Application.Features.InitializingPayment;

public record InitializePayment(string SubId, decimal Price, string CallBackUrl) : ICommand<InitializePaymentResponse>;

public class InitializePaymentValidator : AbstractValidator<InitializePayment>
{
    public InitializePaymentValidator()
    {
        RuleFor(x => x.SubId).NotEmpty().WithMessage("آیدی را وارد کنید");
        RuleFor(x => x.CallBackUrl).NotEmpty().WithMessage("آدرس برگشت را وارد کنید");
    }
}

public class InitializePaymentHandler : ICommandHandler<InitializePayment, InitializePaymentResponse>
{
    private readonly PlanDbContext _context;
    private readonly IZarinPalFactory _zarinPalFactory;

    public InitializePaymentHandler(PlanDbContext context, IZarinPalFactory zarinPalFactory)
    {
        _context = context;
        _zarinPalFactory = zarinPalFactory;
    }

    public async Task<InitializePaymentResponse> Handle(InitializePayment request, CancellationToken cancellationToken)
    {
        var planSub = _context.PlanSubscription.AsQueryable().FirstOrDefault(x => x.Id == request.SubId);

        if (planSub is null)
            throw new NotFoundException("اشتراک مورد نظر پیدا نشد");

        if (planSub.State == PlanSubscriptionState.Subscribed || planSub.State == PlanSubscriptionState.Canceled)
            throw new BadRequestException("اشتراک مورد نظر فعال است");

        var paymentResponse = await _zarinPalFactory.CreatePaymentRequest(request.CallBackUrl,
                                                                          planSub.Price.ToString(CultureInfo.InvariantCulture),
                                                                          string.Empty,
                                                                          planSub.Id);

        string redirectUrl = "https://sandbox.zarinpal.com/pg/StartPay/" + paymentResponse.Authority;

        return new InitializePaymentResponse(redirectUrl);
    }
}