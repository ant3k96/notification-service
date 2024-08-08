using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Notification.Api.Controllers.ErrorResponse;
using NotificationApi.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(typeof(INotificationHandler<>)));

builder.Services.AddNotificationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(c => c.Run(async httpContext =>
{
    var exception = httpContext
        .Features
        .Get<IExceptionHandlerPathFeature>()
        ?.Error;

    await httpContext.Response.WriteAsJsonAsync(new HttpErrorResponse()
    {
        ErrorMessage = exception?.Message
                       ?? "Request failed without an exception, see logs for more information"
    });
}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
