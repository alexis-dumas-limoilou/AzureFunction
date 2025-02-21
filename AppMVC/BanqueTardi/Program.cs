using BanqueTardi.Data;
using BanqueTardi.MVC.Interface;
using BanqueTardi.MVC.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

 builder.Services.AddDbContext<ClientContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQLConnection")));

// Azure storage account
builder.Services.AddAzureClients(configure =>
{
    configure.AddQueueServiceClient(builder.Configuration.GetConnectionString("StorageConnectionString"));
});

builder.Services.AddScoped<IStorageServiceHelper, StorageServiceHelper>();

//Inscrire les APIs dans le conteneur d'injection de dépendances
builder.Services.AddHttpClient<ICalculInteretService, CalculInteretServiceProxy>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("URLAPIs:CalculInteretAPI"));
});

builder.Services.AddHttpClient<IAssurancesService, AssurancesServiceProxy>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("URLAPIs:AssurancesAPI"));
});

builder.Services.AddHttpClient<ICarteCreditServices, CarteCreditServiceProxy>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("URLAPIs:CarteCreditAPI"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ClientContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
