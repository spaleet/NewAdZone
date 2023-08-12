using Ad.Application.Interfaces;
using BuildingBlocks.Core.CQRS.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using BuildingBlocks.Core.Utilities.ImageRelated;
using BuildingBlocks.Core.Exceptions.Base;
using Ad.Application.Exceptions;

namespace Ad.Application.Features.AdGallery.UploadingGallery;

public record UploadGallery(long AdId, IFormFile ImageSource) : ICommand;

public class UploadGalleryValidator : AbstractValidator<UploadGallery>
{
    public UploadGalleryValidator()
    {
        RuleFor(x => x.ImageSource)
            .Must(x => x.IsImage())
            .WithMessage("لطفا عکس معتبر وارد کنید");
        //TODO MAX SIZE CHECk
    }
}

public class UploadGalleryHandler : ICommandHandler<UploadGallery>
{
    private readonly IAdDbContext _context;

    public UploadGalleryHandler(IAdDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UploadGallery request, CancellationToken cancellationToken)
    {
        // get ad model from db
        var adModel = await _context.Ads.FindAsync(request.AdId);

        // check for null
        AdNotFoundException.ThrowIfNull(adModel);

        // upload new image
        string uploadFileName = request.ImageSource.UploadImage("wwwroot/upload/ad_gallery/", width: 500, height: 500);

        await _context.AdGalleries.AddAsync(new Domain.Entities.AdGallery
        {
            AdId = adModel.Id,
            ImageSrc = uploadFileName,
        });
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}