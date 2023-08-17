namespace Ad.Application.Features.Ad.DeletingAd;

public record DeleteAd(long Id) : ICommand;

public class DeleteAdValidator : AbstractValidator<DeleteAd>
{
    public DeleteAdValidator()
    {
        RuleFor(x => x.Id)
            .RequiredValidator("شناسه");
    }
}

public class DeleteAdHandler : ICommandHandler<DeleteAd>
{
    private readonly IAdDbContext _context;
    public DeleteAdHandler(IAdDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAd request, CancellationToken cancellationToken)
    {
        // get ad model from db
        var adModel = await _context.Ads.FindAsync(request.Id);

        // check for null
        AdNotFoundException.ThrowIfNull(adModel);

        adModel.IsDelete = true;

        _context.Ads.Update(adModel);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}