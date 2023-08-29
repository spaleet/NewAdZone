namespace Ad.Application.Clients;
public interface IPlanClient
{
    Task<bool> VerifyPlanLimit(string userId, int usedQuota);
}
