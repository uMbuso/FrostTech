using FridgeManagement.DAL;
using FridgeManagement.DAL.Models;
using FridgeManagementSystem.BLL.Repositories;
using FridgeManagementSystem.BLL.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddControllersWithViews();
    services.AddCors();
    services.AddCookiePolicy(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddControllers().AddJsonOptions(x =>
    {
        // Serialize enums as strings in api responses
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // Ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(config => config.AddGlobalIgnore("Item"), AppDomain.CurrentDomain.GetAssemblies());

    // Configure strongly typed settings object
    services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

    // Configure DI for application services
    services.AddSingleton<DbContext>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IRoleRepository, RoleRepository>();
    services.AddScoped<IRoleService, RoleService>();
    services.AddScoped<IUserRoleRepository, UserRoleRepository>();
    services.AddScoped<IAddressRepository, AddressRepository>();
    services.AddScoped<IProfileRequestRepository, ProfileRequestRepository>();
    services.AddScoped<IFridgeAllocationRepository,  FridgeAllocationRepository>();
    services.AddScoped<IFridgeAllocationService, FridgeAllocationService>();
}

var app = builder.Build();

// Ensure database and tables exist
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DbContext>();
    await context.Init();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
