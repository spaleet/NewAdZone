using System.ComponentModel.DataAnnotations;

namespace Ticket.Domain.Enums;

public enum TicketDepartmentEnum
{
    [Display(Name = "پشتیبانی")]
    Support,

    [Display(Name = "فنی")]
    Technical
}

public enum TicketPriorityEnum
{
    [Display(Name = "کم")]
    Low,

    [Display(Name = "متوسط")]
    Medium,

    [Display(Name = "زیاد")]
    High
}

public enum TicketStateEnum
{
    [Display(Name = "در حال بررسی")]
    UnderProgress,

    [Display(Name = "پاسخ داده شده")]
    Answered,

    [Display(Name = "بسته شده")]
    Closed
}