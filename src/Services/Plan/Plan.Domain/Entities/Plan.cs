using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Persistence.Mongo.Base;
using MongoDB.Bson.Serialization.Attributes;

namespace Plan.Domain.Entities;

public class Plan : MongoEntityBase
{
    [Display(Name = "نام پلن")]
    [BsonElement("title")]
    [Required]
    public string Title { get; set; }

    [Display(Name = "تعداد آگهی های قابل بارگزاری")]
    [BsonElement("adQuota")]
    public int AdQuota { get; set; }

    [Display(Name = "قیمت پلن")]
    [BsonElement("price")]
    public decimal? Price { get; set; }

    [Display(Name = "حساب")]
    [BsonElement("userId")]
    public string UserId { get; set; }
}
