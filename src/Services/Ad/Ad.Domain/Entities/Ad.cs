using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ad.Domain.Enums;
using BuildingBlocks.Persistence.Ef.Base;

namespace Ad.Domain.Entities;

public class Ad : EfEntityBase
{
    [Display(Name = "آگهی دهنده")]
    public long UserId { get; set; }

    [Display(Name = "دسته بندی")]
    public long CategoryId { get; set; }

    [Display(Name = "عنوان")]
    [Required]
    [MaxLength(75)]
    public string Title { get; set; }

    [Display(Name = "تصویر اصلی")]
    [Required]
    public string MainImage { get; set; }

    [Display(Name = "وضعیت فروش")]
    [Required]
    public SaleStatus SaleState { get; set; }

    [Display(Name = "شرایط محصول")]
    [Required]
    public ProductStatus ProductState { get; set; }

    [Display(Name = "توضیحات")]
    [Required]
    [MinLength(15)]
    public string Description { get; set; }

    [Display(Name = "کلمات کلیدی")]
    public string Tags { get; set; }

    [Display(Name = "قیمت")]
    public decimal Price { get; set; } = 0;

    [Required]
    public string Slug { get; set; }

    //================================== Relations
    [ForeignKey(nameof(CategoryId))]
    public virtual AdCategory AdCategory { get; set; }

    public virtual ICollection<AdGallery> AdGalleries { get; set; }

}
