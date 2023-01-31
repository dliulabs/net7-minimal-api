using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new [] {
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching"
};

app.MapGet("/weatherforecast", () => {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithTags("WeatherGroup")
    .WithOpenApi();

app
    .MapGet("/todos", () => {
        var todos = new [] {
        new Todo(
        1, "Task 1", "Something to do", "Desc", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(4))
        )
        };
        return TypedResults.Ok(todos);
    })
    .Produces<IList<Todo>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithTags("TodoGroup")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) {
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
record Todo(int ToDoId, string Title, string Description, string Category, DateOnly AssignedDate, DateOnly DueDate) {
    public int TaskDays => DueDate.DayNumber - AssignedDate.DayNumber;
}