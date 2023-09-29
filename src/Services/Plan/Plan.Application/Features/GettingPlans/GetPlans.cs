using MongoDB.Driver;
using Plan.Application.Dtos;

namespace Plan.Application.Features.GettingPlans;

public record GetPlans : IQuery<GetPlansResponse>;

public class GetPlansHandler : IQueryHandler<GetPlans, GetPlansResponse>
{
    private readonly PlanDbContext _context;
    private readonly IMapper _mapper;

    public GetPlansHandler(PlanDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetPlansResponse> Handle(GetPlans request, CancellationToken cancellationToken)
    {
        var plans = await _context.Plans.AsQueryable().ToListAsync();

        var mappedPlans = plans.Select(x => _mapper.Map(x, new PlanDto())).ToList();

        return new GetPlansResponse(mappedPlans);
    }
}