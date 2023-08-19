using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Persistence.Mongo.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ticket.Domain.Enums;

namespace Ticket.Domain.Entities;
public class Ticket : MongoEntityBase
{
    [Display(Name = "کاربر")]
    [BsonElement("userId")]
    public string UserId { get; set; }

    [Display(Name = "عنوان")]
    [BsonElement("title")]
    public string Title { get; set; }

    [Display(Name = "بخش موردنظر")]
    [BsonElement("ticketSection")]
    [BsonRepresentation(BsonType.String)] 
    public TicketSectionEnum TicketSection { get; set; }

    [Display(Name = "اولویت")]
    [BsonElement("ticketPriority")]
    [BsonRepresentation(BsonType.String)]
    public TicketPriorityEnum TicketPriority { get; set; }

    [Display(Name = "وضعیت")]
    [BsonElement("ticketState")]
    [BsonRepresentation(BsonType.String)]
    public TicketStateEnum TicketState { get; set; }

    [Display(Name = "خوانده شده توسط کاربر")]
    [BsonElement("isReadByUser")]
    public bool IsReadByUser { get; set; }

    [Display(Name = "خوانده شده توسط ادمین")]
    [BsonElement("isReadByAdmin")]
    public bool IsReadByAdmin { get; set; }

    //================================== Relations

    [BsonElement("messages")]
    public List<TicketMessage> Messages { get; set; }
}
