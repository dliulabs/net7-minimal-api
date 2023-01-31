using System.Diagnostics;
using Microsoft.AspNetCore.HeaderPropagation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddHttpClient("PropagateHeaders")
    .AddHeaderPropagation();

builder.Services.AddHeaderPropagation(options => {
    options.Headers.Add("X-TraceId");
});

builder.Services.AddApplicationInsightsTelemetry();

builder.Logging.AddJsonConsole();

var app = builder.Build();

app.MapGet("/", (LinkGenerator linker) =>
    $"The link to the hello route is {linker.GetPathByName("hi", values: null)}");

app.MapGet("/hello", () => "Hello named route")
    .WithName("hi");

app.MapGet("/users/{userId}/books/{bookId}",
    (string userId, string bookId) => $"The user id is {userId} and book id is {bookId}");

app.MapGet("/todos/{id:int}", (int id) => $"db.Todos.Find({id})");

app.MapGet("/todos/{text}", (string text) => $"db.Todos.Where(t => t.Text.Contains({text}))");

app.MapGet("/posts/{*rest}", (string rest) => $"Routing to {rest}");

app.MapGet("/orders/{slug:regex(^[a-z0-9_-]+$)}", (string slug) => $"Orders slug {slug}");

// call external API using HttpClient
app.MapGet("test-httpclient", async(HttpClient http) => {
    return await http.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
});

// call external API using IHttpClientFactory
app.MapGet("test-clientfactory", async(IHttpClientFactory httpClientFactory) => {
    var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/posts");
    var httpClient = httpClientFactory.CreateClient();
    var response = await httpClient.SendAsync(request);
    return await response.Content.ReadFromJsonAsync<List<Post>>();
});

app.Logger.LogInformation("API started");

app.UseHeaderPropagation();

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
// The following line enables Application Insights telemetry collection.

app.Run();

class Post {
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}