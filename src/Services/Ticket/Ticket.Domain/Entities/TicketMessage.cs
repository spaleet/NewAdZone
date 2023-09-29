using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Persistence.Mongo.Base;
using MongoDB.Bson.Serialization.Attributes;

namespace Ticket.Domain.Entities;

public class TicketMessage : MongoEntityBase
{
    [Display(Name = "تیکت")]
    [BsonElement("ticketId")]
    public string TicketId { get; set; }

    [Display(Name = "کاربر")]
    [BsonElement("userId")]
    public string UserId { get; set; }

    [Display(Name = "متن")]
    [BsonElement("text")]
    public string Text { get; set; }
}