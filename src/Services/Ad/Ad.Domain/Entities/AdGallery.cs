using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Persistence.Ef.Base;

namespace Ad.Domain.Entities;
public class AdGallery : EfEntityBase
{
    [Display(Name = "آگهی")]
    public long AdId { get; set; }

    [Display(Name = "تصویر")]
    [Required]
    public string ImageSrc { get; set; }

    //================================== Relations
    public virtual Ad Ad { get; set; }
}
