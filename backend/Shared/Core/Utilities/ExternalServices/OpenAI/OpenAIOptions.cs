namespace Core.Utilities.ExternalServices.OpenAI
{
    public class OpenAIOptions
    {
        public string ApiKey { get; set; }
        public string Model { get; set; } = "gpt-3.5-turbo";
        public string BaseUrl { get; set; } = "https://api.openai.com/v1/";
        public string Organization { get; set; }
    }
}

