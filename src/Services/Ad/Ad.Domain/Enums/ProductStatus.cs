using System.ComponentModel.DataAnnotations;

namespace Ad.Domain.Enums;

public enum ProductStatus
{
    /// <summary>
    /// نو
    /// </summary>
    [Display(Name = "نو")]
    New,

    /// <summary>
    /// درحد نو
    /// </summary>
    [Display(Name = "درحد نو")]
    AlmostNew,

    /// <summary>
    /// کارکرده
    /// </summary>
    [Display(Name = "کارکرده")]
    Used
}