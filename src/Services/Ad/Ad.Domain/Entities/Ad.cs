using System.ComponentModel.DataAnnotations;
using Ad.Domain.Enums;

namespace Ad.Domain.Entities;

public class Ad
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

    [Display(Name = "شرح")]
    [Required]
    [MinLength(25)]
    public string Description { get; set; }

    [Display(Name = "کلمات کلیدی")]
    public string Tags { get; set; }

    [Display(Name = "قیمت")]
    public int? Price { get; set; }

    [Display(Name = "محصول ویژه")]
    public bool IsSpecial { get; set; }
}
