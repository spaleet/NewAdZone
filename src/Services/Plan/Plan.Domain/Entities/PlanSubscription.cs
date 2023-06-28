using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Persistence.Mongo.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Plan.Domain.Entities;

public class PlanSubscription : MongoEntityBase
{
    [Display(Name = "پلن")]
    [BsonElement("planId")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string PlanId { get; set; }

    [Display(Name = "کاربر")]
    [BsonElement("userId")]
    public string UserId { get; set; }

    [Display(Name = "قیمت")]
    [BsonElement("price")]
    public decimal? Price { get; set; }

    [Display(Name = "وضعیت")]
    [BsonElement("state")]
    //[BsonRepresentation(MongoDB.Bson.BsonType.Boolean)]
    public PlanSubscriptionState State { get; set; }

    [Display(Name = "کد پیگیری")]
    [BsonElement("issueTrackingNo")]
    [Required]
    [MaxLength(10)]
    public string IssueTrackingNo { get; set; }

    [Display(Name = "کد بازگشت درگاه پرداخت")]
    [BsonElement("refId")]
    public long RefId { get; set; }

    [Display(Name = "شروع اشتراک")]
    [BsonElement("subscriptionStart")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime SubscriptionStart { get; set; }

    [Display(Name = "انقضای اشتراک")]
    [BsonElement("subscriptionExpire")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime SubscriptionExpire { get; set; }
}

public enum PlanSubscriptionState : byte
{
    Pending = 0,
    Subscribed = 1,
    Canceled = 2
}