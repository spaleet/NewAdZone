﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BuildingBlocks.Core.Utilities;
using BuildingBlocks.Persistence.Ef.Base;

namespace Ad.Domain.Entities;

public class AdCategory : EfEntityBase<long>
{
    public AdCategory()
    {
    }

    public AdCategory(string title, long? parentId = null)
    {
        Title = title;
        ParentId = parentId;
        Slug = title.ToSlug();
    }

    [Display(Name = "نام گروه")]
    [Required]
    public string Title { get; set; }

    [Display(Name = "سردسته")]
    public long? ParentId { get; set; }

    [Required]
    public string Slug { get; set; }

    //================================== Relations
    [ForeignKey(nameof(ParentId))]
    public virtual AdCategory ParentCategory { get; set; }

    public virtual ICollection<Ad> Ads { get; set; }
}