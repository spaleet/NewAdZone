using Ad.Application.Clients;
using Ad.Application.Dtos;
using Ad.Application.Extensions;
using AutoMapper;
using BuildingBlocks.Core.Exceptions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ad.Application.Features.Ad.PostingAd;

public record PostAd : ICommand<AdDto>
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

public class PostAdHandler : ICommandHandler<PostAd, AdDto>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPlanClient _planClient;
    private readonly ILogger<PostAdHandler> _logger;

    public PostAdHandler(IAdDbContext context, IMapper mapper, IPlanClient planClient, ILogger<PostAdHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _planClient = planClient;
        _logger = logger;
    }

    public async Task<AdDto> Handle(PostAd request, CancellationToken cancellationToken)
    {
        int adsPosted = await _context.Ads.CountAsync(x => x.UserId == request.UserId);

        // check user plan limit
        bool verifyPlan = await _planClient.VerifyPlanLimit(request.UserId, adsPosted);

        _logger.LogInformation("Verify Plan result: {0} for user Id : {1}", verifyPlan, request.UserId);

        if (!verifyPlan)
            throw new BadRequestException("شما بیشتر از حداکثر تعداد آگهی در پلن انتخاب شده، آگهی ثبت کرده اید! پلن را ارتقا دهید ");

        // if ad is paid but price not entered!
        if (request.SaleState == SaleStatus.Paid && request.Price == 0)
            throw new BadRequestException("لطفا قیمت را وارد کنید یا وضعیت فروش را تغییر دهید");

        // map ad model
        var createdAd = _mapper.Map<Domain.Entities.Ad>(request);

        // join tags
        createdAd.Tags = await _context.AdCategories.JoinTags(createdAd.CategoryId);

        // upload image
        string uploadFileName = request.ImageSource.UploadImage(AdPathConsts.Ad, width: 500, height: 500);

        createdAd.MainImage = uploadFileName;

        // fix price
        if (createdAd.SaleState != SaleStatus.Paid)
            createdAd.Price = 0;

        await _context.Ads.AddAsync(createdAd);
        await _context.SaveChangesAsync();

        return _mapper.Map<AdDto>(createdAd);
    }
}