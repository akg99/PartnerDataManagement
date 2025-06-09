using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace PartnerDataManagement.Web
{
    public class AzureOpenAIService(IConfiguration configuration)
    {
        private readonly AzureOpenAIClient _client = new AzureOpenAIClient(
            new Uri(configuration.GetValue<string>("AzureOpenAI:Endpoint") ?? throw new ArgumentNullException("no config value for AzureOpenAI Endpoint"), UriKind.Absolute),
            new AzureKeyCredential(
                configuration.GetValue<string>("AzureOpenAI:ApiKey") ?? throw new ArgumentNullException("no config value for AzureOpenAI ApiKey")
            )
        );

        public async Task<string> GetResponse(string prompt)
        {
            try
            {
                var chatCompletionOptions = new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    MaxOutputTokenCount = 100,
                };

                var chatClient = _client.GetChatClient("gpt-35-turbo"); // Replace with your model name
                var response = await chatClient.CompleteChatAsync(
                    [
                        new SystemChatMessage("You are a helpful assistant."),
                        new UserChatMessage(prompt)
                    ],
                    chatCompletionOptions
                );
                return response.Value.Content.ToString();
            }
            catch (Exception ex)
            {
                return $"Error processing request: {ex.Message}";
            }
        }
    }
}
