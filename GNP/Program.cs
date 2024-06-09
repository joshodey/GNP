using GNP.Configuration;
using GNP.Context;
using GNP.IRepository;
using GNP.Models;
using GNP.Repository;
using GNP.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<ApplicationForm, int>, Repository<ApplicationForm, int>>();
builder.Services.AddScoped<IRepository<Admin, long>, Repository<Admin, long>>();
builder.Services.AddScoped<IRepository<Applicant, long>, Repository<Applicant, long>>();
builder.Services.AddScoped<IEmailService, EmailService>();
<<<<<<< HEAD

builder.Services.ConfigureSession();

builder.Services.AddIdentity<User, Role>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
    

=======
>>>>>>> 5ad80b737bc20c74acb57f6aaf35ab33d2c082dd

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
