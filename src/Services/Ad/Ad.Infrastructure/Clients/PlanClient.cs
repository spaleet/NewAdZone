using Ad.Application.Clients;
using Microsoft.AspNetCore.WebUtilities;

namespace Ad.Infrastructure.Clients;
public class PlanClient : IPlanClient
{
    private readonly HttpClient _client;

    public PlanClient(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<bool> VerifyPlanLimit(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        var query = new Dictionary<string, string>()
        {
            ["uid"] = userId
        };

        string uri = QueryHelpers.AddQueryString("", query);

        var res = await _client.GetAsync(uri);

        return res.StatusCode == System.Net.HttpStatusCode.OK;
    }
}
