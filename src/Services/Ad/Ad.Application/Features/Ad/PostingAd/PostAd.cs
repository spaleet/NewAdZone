﻿using Ad.Application.Clients;
using AutoMapper;
using BuildingBlocks.Core.Exceptions.Base;
using Microsoft.AspNetCore.Http;

namespace Ad.Application.Features.Ad.PostingAd;

public record PostAd : ICommand
{
    public string UserId { get; set; }

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
            .RequiredValidator("عنوان")
            .MaxLengthValidator("عنوان", 75);

        RuleFor(x => x.Description)
            .MinLengthValidator("توضیحات", 50)
            .MaxLengthValidator("توضیحات", 1000);

        RuleFor(x => x.ImageSource)
            .MaxFileSizeValidator(MaxFileSize.Megabyte(3), true);
    }
}

public class PostAdHandler : ICommandHandler<PostAd>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUserClient _userClient;
    private readonly IPlanClient _planClient;

    public PostAdHandler(IAdDbContext context, IMapper mapper, IPlanClient planClient, IUserClient userClient)
    {
        _context = context;
        _mapper = mapper;
        _userClient = userClient;
        _planClient = planClient;
    }

    public async Task<Unit> Handle(PostAd request, CancellationToken cancellationToken)
    {
        // check user role
        bool verifyRole = await _planClient.VerifyPlanLimit(request.UserId);

        if (!verifyRole)
            throw new BadRequestException("اطلاعات حساب کاربری شما کامل نیست");
        
        // check user plan limit
        bool verifyPlan = await _planClient.VerifyPlanLimit(request.UserId);

        if (!verifyPlan)
            throw new BadRequestException("شما بیشتر از حداکثر تعداد آگهی در پلن انتخاب شده، آگهی ثبت کرده اید! پلن را ارتقا دهید ");

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
        string uploadFileName = request.ImageSource.UploadImage(AdPathConsts.Ad, width: 500, height: 500);

        createdAd.MainImage = uploadFileName;

        // fix price
        if (createdAd.SaleState != SaleStatus.Paid)
            createdAd.Price = 0;

        await _context.Ads.AddAsync(createdAd);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}