using auto_checkin;
using auto_checkin.Persistances;
using Microsoft.EntityFrameworkCore;
using PNN.Identity.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);




#region logger
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
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    //.MinimumLevel.Verbose()
    .CreateLogger();
Log.Logger = logger;
builder.Logging.AddSerilog(logger);
#endregion
#region websoket
builder.Services.AddSingleton<WebsocketHandler>();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(5)
};

#endregion
#region dbcontext
// dotnet ef migrations add "init" --project auto_checkin --context ApplicationDbContext --startup-project auto_checkin --output-dir Persistances/Migrations
//dotnet ef database update --project Identity.Infrastructure --startup-project Identity.WebApi --context ApplicationDbContext

builder.Services.AddSimpleIdentity(builder.Configuration);
var appDb = builder.Configuration.GetSection("AppDb").Get<AppDbOption>();
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(option =>
{
    option.UseNpgsql($"Server={appDb.Server};Port={appDb.Port};User Id={appDb.UserName};Password={appDb.Password};Database={appDb.Database}");
    option.UseLoggerFactory(LoggerFactory.Create(loggingBuilder =>
    {
        loggingBuilder.AddSerilog();
    }
    ));
});
builder.Services.AddScoped<ApplicationDbContext>();
#endregion





// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseWebSockets(webSocketOptions);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentity();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
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