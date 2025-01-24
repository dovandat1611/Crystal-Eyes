using Crystal_Eyes_Controller.Dtos.Email;
using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Mapper;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.Repositories;
using Crystal_Eyes_Controller.Services;
using Crystal_Eyes_Controller.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<CrystalEyesDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Default_Connection"))
);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromDays(1);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// REPOSITORIES
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// SERVICE
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IMailSystemService, MailSystemService>();
builder.Services.AddScoped<IExcelService, ExcelService>();


// Configure MailSettings
var mailSettingsSection = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailSettingsSection); // register for dependency 

var app = builder.Build();

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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
