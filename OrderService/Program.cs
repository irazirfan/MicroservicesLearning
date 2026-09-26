using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Http.Resilience;
using Polly;
using OrderService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlite("Data Source=/data/orders.db"));

builder.Services.AddControllers();

builder.Services.AddHttpClient("ProductServiceClient", client =>
{
    /*Request starts > Wait for response > 5 seconds maximum > If still waiting → timeout*/
    //client.Timeout = TimeSpan.FromSeconds(5);
    client.Timeout = TimeSpan.FromSeconds(5);
})
.AddStandardResilienceHandler(options =>
{
    /*MaxRetryAttempts = 3 means 3 retries after the initial attempt, not 3 total attempts.
    Exponential means the delay increases between attempts.
    UseJitter = true adds a small amount of randomness to the delays, which helps prevent many services from retrying simultaneously.*/
    //options.Retry.MaxRetryAttempts = 3;
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromSeconds(1);
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.UseJitter = true;
    /*FailureRatio = 0.5 → circuit can open when about 50 % of requests are failures.
    MinimumThroughput = 10 → don't make a circuit decision until at least 10 attempts have occurred.
    SamplingDuration = 30 seconds → look at failures over a 30 - second window.
    BreakDuration = 15 seconds → once opened, stop sending requests for 15 seconds.*/
    options.CircuitBreaker.FailureRatio = 0.5;
    options.CircuitBreaker.MinimumThroughput = 10;
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();