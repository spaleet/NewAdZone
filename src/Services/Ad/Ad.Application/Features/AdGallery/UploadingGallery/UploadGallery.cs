using Ad.Application.Interfaces;
using BuildingBlocks.Core.CQRS.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using BuildingBlocks.Core.Utilities.ImageRelated;
using BuildingBlocks.Core.Exceptions.Base;
using Ad.Application.Exceptions;

namespace Ad.Application.Features.AdGallery.UploadingGallery;

public record UploadGallery(long AdId, IFormFile ImageSource) : ICommand<UploadGalleryResponse>;

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

public class UploadGalleryHandler : ICommandHandler<UploadGallery, UploadGalleryResponse>
{
    private readonly IAdDbContext _context;

    public UploadGalleryHandler(IAdDbContext context)
    {
        _context = context;
    }

    public async Task<UploadGalleryResponse> Handle(UploadGallery request, CancellationToken cancellationToken)
    {
        // get ad model from db
        var adModel = await _context.Ads.FindAsync(request.AdId);

        // check for null
        AdNotFoundException.ThrowIfNull(adModel);

        // upload new image
        string uploadFileName = request.ImageSource.UploadImage("wwwroot/upload/ad_gallery/", width: 500, height: 500);
        

        // save db
        var gallery = new Domain.Entities.AdGallery
        {
            AdId = adModel.Id,
            ImageSrc = uploadFileName
        };

        await _context.AdGalleries.AddAsync(gallery);
        await _context.SaveChangesAsync();

        return new UploadGalleryResponse(gallery.Id.ToString());
    }
}