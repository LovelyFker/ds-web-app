using loopy_chat_bot;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// ÅäÖÃÂ·ÓÉ
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Hello from Linux background service!");
});

app.MapGet("/api/data", async context =>
{
    var data = new { Time = DateTime.Now, Message = "Server is running" };
    await context.Response.WriteAsJsonAsync(data);

    Console.WriteLine($"/apt/data response: {data}", ConsoleColor.Green);
});

app.MapGet("/chat/loopy", async context =>
{
    string user = "";
    string msg = "";
    try
    {
        var requestBody = await context.Request.ReadFromJsonAsync<ChatLoopyRequestBody>();
        user = requestBody.user;
        msg = requestBody.msg;
        Console.WriteLine($"user[{user}] chat with loopy send msg[{msg}]", ConsoleColor.Magenta);
    }
    catch(Exception e)
    {
        Console.WriteLine($"read user msg exception: {e}", ConsoleColor.Red);
    }

    if (!string.IsNullOrEmpty(user))
    {
        var resContent = await DeepseekAPI.ChatLoopyRequest(msg);
        await FeishuRobotAPI.SendMsg($"@{user} {resContent}");
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

Console.WriteLine($"app init fin", ConsoleColor.Green);

await app.RunAsync();