using Ad.Application.Interfaces;
using Ad.Domain.Enums;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Commands;
using BuildingBlocks.Core.Exceptions.Base;
using BuildingBlocks.Core.Utilities.ImageRelated;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Ad.Application.Features.Ad.PostingAd;

public record PostAd : ICommand
{
    public long UserId { get; set; }

    public long SelectedCategory { get; set; }
    
    public string Title { get; set; }

    public SaleStatus SaleState { get; set; }

    public ProductStatus ProductState { get; set; }

    public string Description { get; set; }

    public decimal? Price { get; set; } = 0;

    public IFormFile ImageSource { get; set; }
}

public class PostAdValidator : AbstractValidator<PostAd>
{
    public PostAdValidator()
    {
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
            .Must(x => x.IsImage())
            .WithMessage("لطفا عکس معتبر وارد کنید");
        //TODO MAX SIZE CHECk
    }
}

public class PostAdHandler : ICommandHandler<PostAd>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public PostAdHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(PostAd request, CancellationToken cancellationToken)
    {
        // TODO CHECK USER!!
        // TODO CHECK USER LIMIT & ROLE

        // if ad is paid but price not entered!
        if (request.SaleState == SaleStatus.Paid && request.Price == 0)
            throw new BadRequestException("لطفا قیمت را وارد کنید یا وضعیت فروش را تغییر دهید");

        // map ad model
        var createdAd = _mapper.Map<Domain.Entities.Ad>(request);

        // map Tags based on categories
        // get category and parent category titles to an string array splitted by space
        string[] categories = await _context.AdCategories
            .Include(x => x.ParentCategory)
            .Where(x => x.Id == createdAd.CategoryId)
            .Select(x => $"{x.Title} {x.ParentCategory.Title ?? string.Empty}".Trim().Split())
            .FirstOrDefaultAsync();

        // seperate titles by comma
        createdAd.Tags = string.Join(",", categories);

        // upload image
        string uploadFileName = request.ImageSource.UploadImage("wwwroot/upload/ad/", width: 500, height: 500);

        createdAd.MainImage = uploadFileName;

        // fix price
        if (createdAd.SaleState != SaleStatus.Paid)
            createdAd.Price = 0;

        await _context.Ads.AddAsync(createdAd);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}