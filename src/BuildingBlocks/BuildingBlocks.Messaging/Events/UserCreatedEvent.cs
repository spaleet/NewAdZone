namespace BuildingBlocks.Messaging.Events;

public class UserCreatedEvent
{
    public UserCreatedEvent()
    {
    }

    public UserCreatedEvent(string userId)
    {
        UserId = userId;
        CreateDate = DateTime.UtcNow;
    }

    public string UserId { get; set; }

    public DateTime CreateDate { get; set; }
}