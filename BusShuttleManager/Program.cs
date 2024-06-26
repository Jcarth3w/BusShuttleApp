using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusShuttleManager.Data;
using BusShuttleManager.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy =>
    {
        policy.RequireRole("Manager");
    });

    options.AddPolicy("ActivatedDriver", policy =>
    {
        policy.RequireAuthenticatedUser(); 
        policy.RequireRole("Driver");
        policy.RequireClaim("IsActivated", "true"); 
    });
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<IDriverService, DriverServices>();
builder.Services.AddScoped<IBusService, BusServices>();
builder.Services.AddScoped<IStopService, StopServices>();
builder.Services.AddScoped<ILoopService, LoopServices>();
builder.Services.AddScoped<IRouteService, RouteServices>();
builder.Services.AddScoped<IEntryService, EntryServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.MapRazorPages();

app.Run();
