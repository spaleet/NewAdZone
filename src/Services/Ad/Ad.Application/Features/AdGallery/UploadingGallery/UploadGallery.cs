﻿using Microsoft.AspNetCore.Http;

namespace Ad.Application.Features.AdGallery.UploadingGallery;

public record UploadGallery(long AdId, IFormFile ImageSource) : ICommand<UploadGalleryResponse>;

public class UploadGalleryValidator : AbstractValidator<UploadGallery>
{
    public UploadGalleryValidator()
    {
        RuleFor(x => x.ImageSource)
            .MaxFileSizeValidator(MaxFileSize.Megabyte(2));
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
        string uploadFileName = request.ImageSource.UploadImage(AdPathConsts.Gallery, width: 500, height: 500);

        // save db
        var gallery = new Domain.Entities.AdGallery
        {
            AdId = adModel.Id,
            ImageSrc = uploadFileName,
            ContentType = request.ImageSource.ContentType
        };

        await _context.AdGalleries.AddAsync(gallery);
        await _context.SaveChangesAsync();

        return new UploadGalleryResponse(gallery.Id.ToString());
    }
}