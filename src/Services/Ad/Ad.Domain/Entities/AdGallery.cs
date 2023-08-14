using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BuildingBlocks.Persistence.Ef.Base;

namespace Ad.Domain.Entities;
public class AdGallery : EfEntityBase<Guid>
{
    [Display(Name = "آگهی")]
    public long AdId { get; set; }

    [Display(Name = "تصویر")]
    [Required]
    public string ImageSrc { get; set; }

    //================================== Relations
    [ForeignKey(nameof(AdId))]
    public virtual Ad Ad { get; set; }
}
