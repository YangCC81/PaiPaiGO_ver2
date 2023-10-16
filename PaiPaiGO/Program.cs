using Microsoft.EntityFrameworkCore;
using PaiPaiGO.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PaiPaiGoContext>(
      options => options.UseSqlServer(builder.Configuration.GetConnectionString("PaiPaiGoConnstring")));
//設置Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//設置Session，要在UseRouting()前
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=CC_Members}/{action=Login}/{id?}");
//pattern: "{controller=HS_Pending}/{action=PendingOrder_Pai}/{id?}");
//pattern: "{controller=Yu_Calendar}/{action=Yu_Calendar}/{id?}");
//pattern: "{controller=YU_Home}/{action=Index}/{id?}");
//pattern: "{controller=YH_CasePages}/{action=YH_CasePage}/{id?}");
app.Run();

