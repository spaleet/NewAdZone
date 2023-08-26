namespace Ad.Application.Clients;
public interface IUserClient
{
    Task<bool> VerifyRole(string userId);
}
