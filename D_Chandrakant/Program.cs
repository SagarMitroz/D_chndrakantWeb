using D_Chandrakant;
using D_Chandrakant.DataModels;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<TailordbContext>();
var connectionString = builder.Configuration.GetConnectionString("Defaultconnection");
builder.Services.AddScoped<AutocompleteService>();
builder.Services.AddScoped<Logger>();
builder.Services.AddDbContext<TailordbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession(); // Miidleware

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=AuthAd}/{action=AdminLogin}/{id?}");
//pattern: "{controller=Admin}/{action=GetGraphbyEmpwork}/{id?}");

app.Run();
