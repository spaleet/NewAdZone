using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Persistence.Ef.Base;
public abstract class EfEntityBase
{
    [Key]
    public long Id { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public DateTime LastUpdateDate { get; set; } = DateTime.Now;
}
