using Microsoft.AspNetCore.SignalR;
using EinkaufslisteAPI.Hubs;
using Microsoft.EntityFrameworkCore;
using EinkaufslisteAPI.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();  


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ShoppingDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();


app.UseCors("AllowAllOrigins");


app.MapHub<ShoppingListHub>("/shoppingListHub");  


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();


app.MapControllers();


app.Run();