
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PartnerManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PartnerManagementContext") ?? throw new InvalidOperationException("Connection string 'PartnerManagementContext' not found.")));
builder.Services.AddDbContext<BookClassificationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookClassificationContext") ?? throw new InvalidOperationException("Connection string 'BookClassificationContext' not found.")));
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);//Session Timeout.
});

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
