using Ad.Application.Extensions;
using AutoMapper;
using BuildingBlocks.Core.Exceptions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

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
            .RequiredValidator("شناسه");

        RuleFor(x => x.Title)
            .RequiredValidator("عنوان")
            .MaxLengthValidator("عنوان", 75);

        RuleFor(x => x.Description)
            .MinLengthValidator("توضیحات", 50)
            .MinLengthValidator("توضیحات", 1000);

        RuleFor(x => x.ImageSource)
            .MaxFileSizeValidator(MaxFileSize.Megabyte(3), false);
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
            // join new tags
            adModel.Tags = await _context.AdCategories.JoinTags(request.SelectedCategory);
        }

        if (request.ImageSource is not null)
        {
            // delete prevoius image
            ImageHelper.DeleteImage(adModel.MainImage, AdPathConsts.Ad);

            // upload new image
            string uploadFileName = request.ImageSource.UploadImage(AdPathConsts.Ad, width: 500, height: 500);

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