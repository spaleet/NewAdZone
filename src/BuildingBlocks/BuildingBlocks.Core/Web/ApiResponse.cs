using System.Text.Json.Serialization;

namespace BuildingBlocks.Core.Web;
public record ApiResult<T>
{
    public ApiResult(short status, string message, T data)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    [JsonPropertyName("status")] public short Status { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }

    [JsonPropertyName("data")] public T Data { get; set; }
}

public record ApiResult
{
    public ApiResult(short status, string message)
    {
        Status = status;
        Message = message;
    }

    [JsonPropertyName("status")] public short Status { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }
}

public static class ApiResponse
{
    public static ApiResult<T> Success<T>(T result, string message = "عملیات با موفقیت انجام شد")
    {
        return new ApiResult<T>(200, message, result);
    }

    public static ApiResult Success(string message = "عملیات با موفقیت انجام شد")
    {
        return new ApiResult(200, message);
    }

    public static ApiResult Error(string message)
    {
        return new ApiResult(400, message);
    }
}