using System.Text.Json;
using Ad.Application.Clients;
using Ad.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Ad.Infrastructure.Clients;
public class PlanClient : IPlanClient
{
    private readonly HttpClient _client;
    private readonly UserClientOptions _settings;

    public PlanClient(HttpClient httpClient, UserClientOptions settings)
    {
        _client = httpClient;
        _settings = settings;
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
