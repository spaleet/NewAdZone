using System.Text;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Commands;
using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using MongoDB.Driver;
using Plan.Application.Features.CreatingNewPlan;
using Plan.Domain.Entities;
using Plan.Infrastructure.Context;

namespace Plan.Application.Features.SubscribingPlan;

public record SubscribePlan(string PlanId, string UserId) : ICommand<SubscribePlanResponse>;

public class SubscribePlanValidator : AbstractValidator<SubscribePlan>
{
    public SubscribePlanValidator()
    {
    }
}

public class SubscribePlanHandler : ICommandHandler<SubscribePlan, SubscribePlanResponse>
{
    private readonly PlanDbContext _context;

    public SubscribePlanHandler(PlanDbContext context)
    {
        _context = context;
    }

    public async Task<SubscribePlanResponse> Handle(SubscribePlan request, CancellationToken cancellationToken)
    {
        var plan = _context.Plans.AsQueryable().FirstOrDefault(x => x.Id == request.PlanId);

        if (plan is null)
            throw new NotFoundException("پلن مورد نظر پیدا نشد");

        // TODO : check user exists

        var userWithPlan = _context.PlanSubscription.AsQueryable()
            .Where(x => x.UserId == request.UserId && x.PlanId == request.PlanId)
            .FirstOrDefault();

        if (userWithPlan != null)
            throw new BadRequestException("شما قبلا این پلن را انتخاب کرده اید");

        var subscription = new PlanSubscription
        {
            PlanId = request.PlanId,
            UserId = request.UserId,
            State = PlanSubscriptionState.Pending,
            Price = plan.Price,
            IssueTrackingNo = "0000-0000"
        };

        await _context.PlanSubscription.InsertOneAsync(subscription);

        string subscriptionId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(subscription.Id));

        return new SubscribePlanResponse(subscriptionId, subscription.Price);
    }
}