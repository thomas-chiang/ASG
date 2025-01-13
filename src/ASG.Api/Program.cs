using ASG.Application;
using ASG.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    // builder.Services.AddProblemDetails(); used with "app.UseExceptionHandler();" to prevents exposing detailed exception info to client

    //customized Dependency Injection
    builder.Services.AddApplication().AddInfrastructure();
}

var app = builder.Build();
{
    // app.UseExceptionHandler(); // must first used builder.Services.AddProblemDetails();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}