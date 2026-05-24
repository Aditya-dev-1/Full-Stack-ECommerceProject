using ECommerceApplication.StartupExtensions;
using ECommerceApplication.Middleware;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureServices(builder.Configuration);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration) //reads configuration settings (appsettings.json)
    .ReadFrom.Services(services); //reads all services from IServiceCollection
});


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingMiddleware();
}

app.UseHsts(); //enforce https
app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
