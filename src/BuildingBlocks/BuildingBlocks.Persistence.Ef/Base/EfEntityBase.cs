using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingBlocks.Persistence.Ef.Base;

public interface AuditableBase
{
    public DateTime CreateDate { get; set; }

    public DateTime LastUpdateDate { get; set; }
}

public abstract class EfEntityBase<TId> : AuditableBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public DateTime LastUpdateDate { get; set; } = DateTime.Now;
}
