using BuildingBlocks.Core.Utilities.ImageRelated;
using Ad.Application.Interfaces;
using Ad.Domain.Enums;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Commands;
using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ad.Application.Exceptions;

namespace Ad.Application.Features.Ad.EditingAd;

public record EditAd : ICommand
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long SelectedCategory { get; set; }

    public string Title { get; set; }

    public SaleStatus SaleState { get; set; }

    public ProductStatus ProductState { get; set; }

    public string Description { get; set; }

    public decimal? Price { get; set; } = 0;

    public IFormFile? ImageSource { get; set; }
}

public class EditAdValidator : AbstractValidator<EditAd>
{
    public EditAdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("شناسه را وارد کنید");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("عنوان را وارد کنید")
            .MaximumLength(75)
            .WithMessage("عنوان حداکثر 75 کاراکتر است!");

        RuleFor(x => x.Description)
            .MinimumLength(50)
            .WithMessage("توضیحات حداقل 50 کاراکتر است!")
            .MaximumLength(1000)
            .WithMessage("توضیحات حداکثر 1000 کاراکتر است!");

        RuleFor(x => x.ImageSource)
            .Must(x =>
            {
                if (x is not null) return x.IsImage();

                return true;
            })
            .WithMessage("لطفا عکس معتبر وارد کنید");
        //TODO MAX SIZE CHECk
    }
}

public class EditAdHandler : ICommandHandler<EditAd>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public EditAdHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(EditAd request, CancellationToken cancellationToken)
    {
        // get ad model from db
        var adModel = await _context.Ads.FindAsync(request.Id);
         
        // check for null
        AdNotFoundException.ThrowIfNull(adModel);

        // TODO CHECK USER!!
        // TODO CHECK USER LIMIT & ROLE

        // if ad is paid but price not entered!
        if (request.SaleState == SaleStatus.Paid && request.Price == 0)
            throw new BadRequestException("لطفا قیمت را وارد کنید یا وضعیت فروش را تغییر دهید");

        // if title changed set tags again
        if (adModel.CategoryId != request.SelectedCategory)
        {
            // map Tags based on categories
            // get category and parent category titles to an string array splitted by space
            string[] categories = await _context.AdCategories
                .Include(x => x.ParentCategory)
                .Where(x => x.Id == request.SelectedCategory)
                .Select(x => $"{x.Title} {x.ParentCategory.Title ?? string.Empty}".Trim().Split())
                .FirstOrDefaultAsync();

            // seperate titles by comma
            adModel.Tags = string.Join(",", categories);
        }

        if (request.ImageSource is not null)
        {
            // delete prevoius image
            ImageHelper.DeleteImage(adModel.MainImage, "wwwroot/upload/ad/");

            // upload new image
            string uploadFileName = request.ImageSource.UploadImage("wwwroot/upload/ad/", width: 500, height: 500);

            adModel.MainImage = uploadFileName;
        }

        // map ad data
        _mapper.Map(request, adModel);

        // fix price
        if (adModel.SaleState != SaleStatus.Paid)
            adModel.Price = 0;

        _context.Ads.Update(adModel);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}