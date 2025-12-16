using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Core.Utilities.ExternalServices.OpenAI
{
    public class OpenAIClient : IOpenAIClient
    {
        private readonly HttpClient _httpClient;

        public OpenAIClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetChatCompletionAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default)
        {
            var requestBody = new ChatCompletionRequest
            {
                Messages = new List<ChatMessage>
                {
                    new ChatMessage { Role = "system", Content = systemPrompt },
                    new ChatMessage { Role = "user", Content = userPrompt }
                }
            };

            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            using var response = await _httpClient.PostAsJsonAsync("chat/completions", requestBody, jsonOptions, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidOperationException($"OpenAI isteği başarısız oldu. StatusCode: {(int)response.StatusCode}, İçerik: {errorContent}");
            }

            var completionResponse = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(jsonOptions, cancellationToken: cancellationToken);
            var content = completionResponse?.Choices?.FirstOrDefault()?.Message?.Content;

            return content ?? string.Empty;
        }

        private sealed class ChatCompletionRequest
        {
            [JsonPropertyName("model")]
            public string Model { get; set; } = "gpt-3.5-turbo";
            [JsonPropertyName("messages")]
            public List<ChatMessage> Messages { get; set; }
            [JsonPropertyName("temperature")]
            public decimal Temperature { get; set; } = 0.2m;
            [JsonPropertyName("response_format")]
            public ChatResponseFormat ResponseFormat { get; set; } = new ChatResponseFormat { Type = "json_object" };
        }

        private sealed class ChatResponseFormat
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }
        }

        private sealed class ChatCompletionResponse
        {
            [JsonPropertyName("choices")]
            public IReadOnlyList<ChatChoice> Choices { get; set; }
        }

        private sealed class ChatChoice
        {
            [JsonPropertyName("message")]
            public ChatMessage Message { get; set; }
        }

        private sealed class ChatMessage
        {
            [JsonPropertyName("role")]
            public string Role { get; set; }
            [JsonPropertyName("content")]
            public string Content { get; set; }
        }
    }
}

