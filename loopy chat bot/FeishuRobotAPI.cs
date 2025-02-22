using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace loopy_chat_bot
{
    public struct RobotRequestBody
    {
        [JsonInclude]
        public string msg_type;
        [JsonInclude]
        public RobotRquestContent content;
    }

    public struct RobotRquestContent
    {
        [JsonInclude]
        public string text;
    }

    public static class FeishuRobotAPI
    {
        public static HttpClient httpClient = new HttpClient();
        public static string webhookUrl = "https://open.feishu.cn/open-apis/bot/v2/hook/235b2472-6cd7-4e28-b553-e601ebea24e6";
        public static async Task SendMsg(string msg)
        {
            Console.WriteLine($"send robot msg: {msg}", ConsoleColor.Green);

            var request = new HttpRequestMessage(HttpMethod.Post, webhookUrl);
            var body = new RobotRequestBody()
            {
                msg_type = "text",
                content = new RobotRquestContent()
                {
                    text = msg
                }
            };

            var contentStr = JsonSerializer.Serialize(body);
            Console.WriteLine($"send robot content: {contentStr}");
            request.Content = new StringContent(contentStr);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public static async Task TestSendMsg()
        {
            await SendMsg("test msg!!!");
        }
    }
}
