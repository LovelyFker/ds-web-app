using System.Text.Json;

namespace loopy_chat_bot
{
    public static class DeepseekAPI
    {
        public static HttpClient httpClient = new HttpClient();
        public static async Task<string> ChatLoopyRequest(string chatMsg)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, DeepseekConfig.requestUrl);
            foreach(var header in DeepseekConfig.headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            var body = GenChatLoopyRequestBody(DeepseekConfig.model_V3, chatMsg);
            var bodyStr = JsonSerializer.Serialize(body);
            var content = new StringContent(bodyStr);

            request.Content = content;
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var resContent = "";
            try
            {
                resContent = await response.Content.ReadAsStringAsync();
            }
            catch(Exception e)
            {
                resContent = $"read response exception: {e}";
            }
                
            return resContent;
        }

        public static DeepseekRequestBody GenChatLoopyRequestBody(string model, string msg)
        {
            var body = new DeepseekRequestBody()
            {
                model = model,
                messages = new List<DeepseekMessage>()
                {
                    new DeepseekMessage()
                    {
                        role = "system",
                        content = DeepseekConfig.loopyPrompt
                    },
                    new DeepseekMessage()
                    {
                        role = "user",
                        content = msg
                    }
                },
                stream = false
            };
            return body;
        }
    }
}
