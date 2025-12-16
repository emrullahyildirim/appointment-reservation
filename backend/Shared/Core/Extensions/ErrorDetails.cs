using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Extensions
{
    /// <summary>
    /// Standart API response formatı (data içeren)
    /// </summary>
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T? Data { get; set; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public object? Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Başarılı response oluşturur
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "İşlem başarılı.")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        /// <summary>
        /// Hata response'u oluşturur
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, object? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }

    /// <summary>
    /// Standart API response formatı (data içermeyen)
    /// </summary>
    public class ApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public object? Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Başarılı response oluşturur
        /// </summary>
        public static ApiResponse SuccessResponse(string message = "İşlem başarılı.")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Errors = null
            };
        }

        /// <summary>
        /// Hata response'u oluşturur
        /// </summary>
        public static ApiResponse ErrorResponse(string message, object? errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }

}
