using System.Text.Json.Serialization;

namespace BuildingBlocks.Core.Web;

public record ApiResult
{
    public ApiResult(short status, string message, object? data = null)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    [JsonPropertyName("status")] public short Status { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }

    [JsonPropertyName("data")] public object? Data { get; set; }
}