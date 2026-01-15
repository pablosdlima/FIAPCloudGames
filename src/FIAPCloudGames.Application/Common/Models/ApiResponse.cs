// FIAPCloudGames.Application.Common.Models/ApiResponse.cs
// Este código é o que você me forneceu e está correto para o propósito.
using System.Text.Json;

namespace FIAPCloudGames.Application.Common.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; } // Permitir que Data seja nulo
        public Dictionary<string, string[]>? Errors { get; set; } // Permitir que Errors seja nulo
        public DateTime Timestamp { get; set; }
        // TraceId não está em ApiResponse<T> diretamente, mas em ErrorDetails

        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operação realizada com sucesso")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, Dictionary<string, string[]>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors ?? []
            };
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; } // Permitir que Errors seja nulo
        public DateTime Timestamp { get; set; }
        public string? TraceId { get; set; } // Permitir que TraceId seja nulo

        public ErrorDetails()
        {
            Timestamp = DateTime.UtcNow;
        }

        public static ErrorDetails ErrorResponse(int statusCode, string message, Dictionary<string, string[]>? errors = null, string? traceId = null)
        {
            return new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow,
                TraceId = traceId
            };
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
