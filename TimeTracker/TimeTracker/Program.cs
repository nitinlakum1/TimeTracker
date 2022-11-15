using Microsoft.EntityFrameworkCore;
using TimeTracker.Configurations;
using TimeTracker.Helper;
using TimeTracker_Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.ConfigureDependencies();

builder.Services.ConfigureAppSettings(builder.Configuration);

builder.Services.AddJWTConfiguration(builder.Configuration);

builder.Services.AddAutoMapper(typeof(TTMappingProfile));

builder.Services.AddDbContext<TTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TTContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
