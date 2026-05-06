using FCG.Api.Middleware;
using FCG.Domain.Entities;
using FCG.Infrastructure.Persistence;
using FGC.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFcgServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCG API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FcgDbContext>();
    db.Database.Migrate();

    if (!db.Users.Any(_ => _.Role == FCG.Domain.Enums.UserRole.Admin))
    {
        var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin@1234");
        var adminUser = new User("Administrator", "admin@fcg.com", adminHash);
        adminUser.PromoteToAdmin();
        adminUser.Activate();

        db.Users.Add(adminUser);
        db.SaveChanges();
    }
}

app.Run();
