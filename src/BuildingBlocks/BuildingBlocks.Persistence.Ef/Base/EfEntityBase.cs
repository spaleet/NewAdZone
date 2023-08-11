using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Persistence.Ef.Base;

public interface AuditableBase
{
    public bool IsDelete { get; set; }

    public DateTime CreateDate { get; set; } 

    public DateTime LastUpdateDate { get; set; }
}

public abstract class EfEntityBase : AuditableBase
{
    [Key]
    public long Id { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public DateTime LastUpdateDate { get; set; } = DateTime.Now;
}
