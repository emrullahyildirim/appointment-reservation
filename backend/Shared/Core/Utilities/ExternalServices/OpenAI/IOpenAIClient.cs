using System.Threading;
using System.Threading.Tasks;

namespace Core.Utilities.ExternalServices.OpenAI
{
    public interface IOpenAIClient
    {
        Task<string> GetChatCompletionAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default);
    }
}

