using System.ComponentModel.DataAnnotations;

namespace Ad.Domain.Enums;
public enum SaleStatus
{
    /// <summary>
    /// نقدی
    /// </summary>
    [Display(Name = "نقدی")]
    Paid,

    /// <summary>
    /// توافقی
    /// </summary>
    [Display(Name = "توافقی")]
    Adaptive,

    /// <summary>
    /// معاوضه
    /// </summary>
    [Display(Name = "معاوضه")]
    Exchange,
}
