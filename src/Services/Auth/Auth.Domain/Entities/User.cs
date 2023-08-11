using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Persistence.Ef.Base;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities;

public class User : IdentityUser<Guid>, AuditableBase
{
    public User()
    {

    }

    [Display(Name = "نام")]
    [Required]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [Required]
    public string LastName { get; set; }

    [Display(Name = "بیوگرافی")]
    [MaxLength(250)]
    [MinLength(25)]
    public string Bio { get; set; }

    [Display(Name = "آواتار")]
    [Required]
    public string Avatar { get; set; }

    public string SerialNumber { get; set; } = Guid.NewGuid().ToString("N");

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public DateTime LastUpdateDate { get; set; } = DateTime.Now;

    //================================== Relations
    public virtual ICollection<AuthToken> AuthTokens { get; set; }
}
