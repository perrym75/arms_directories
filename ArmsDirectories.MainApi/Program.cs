using ArmsDirectories.DAL.PostgreSql;
using ArmsDirectories.DAL.PostgreSql.Repositories.Base;
using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Common.Utilities;
using MainApi.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<MainApiDbContext>("arms-directories");
builder.EnrichNpgsqlDbContext<MainApiDbContext>();

builder.Services.AddAllImplementations(typeof(IRepository<,>), ServiceLifetime.Scoped);
builder.Services.AddScoped<IUnitOfWork, BaseUnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using (var scope = app.Services.CreateAsyncScope())
    {
        await using var db = scope.ServiceProvider.GetRequiredService<MainApiDbContext>();
        await db.Database.MigrateAsync();
    }
    
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();