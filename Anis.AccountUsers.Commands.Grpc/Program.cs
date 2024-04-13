using Anis.AccountUsers.Commands.Application;
using Anis.AccountUsers.Commands.Grpc.ExceptionHandler;
using Anis.AccountUsers.Commands.Grpc.Interceptors;
using Anis.AccountUsers.Commands.Grpc.Services;
using Anis.AccountUsers.Commands.Grpc.Validators.Main;
using Anis.AccountUsers.Commands.Infra;
using Anis.AccountUsers.Commands.Infra.Persistance;
using Anis.AccountUsers.Commands.Infra.Services.Logger;
using Calzolari.Grpc.AspNetCore.Validation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = LoggerServiceBuilder.Build();

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfraServices(builder.Configuration);
builder.Services.AddGrpc(option =>
{
    option.Interceptors.Add<ThreadCultureInterceptor>();

    option.EnableMessageValidation();

    option.Interceptors.Add<ExceptionHandlingInterceptor>();
});
builder.Services.AddAppValidators();
builder.Host.UseSerilog();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}
app.MapGrpcService<AccountUsersService>();
app.MapGrpcService<AccountUsersDemoEventsService>();
app.MapGrpcService<AccountUsersEventHistoryService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
public partial class Program { }
