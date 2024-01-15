using auto_checkin;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder
    .Host
    .UseSerilog(
        (context, _, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
            // To allow to add custom properties into the context
            //configuration.Enrich.FromGlobalLogContext();
        }
    );
builder.Logging.ClearProviders();

#region logger
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    //.MinimumLevel.Verbose()
    .CreateLogger();
Log.Logger = logger;
builder.Logging.AddSerilog(logger);
#endregion

builder.Services.AddSingleton<WebsocketHandler>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(5)
};

app.UseWebSockets(webSocketOptions);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



try
{
    Log.Information("Starting web host");
    app.UseSerilogRequestLogging();
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}