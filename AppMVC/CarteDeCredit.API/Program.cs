using CarteDeCredit.API.Data;
using CarteDeCredit.API.Interfaces;
using CarteDeCredit.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<CarteDeCreditDBContext>(options =>
     options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

builder.Services.AddTransient<IGenerateurNumeroCarte, GenerateurNumeroCarte>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
