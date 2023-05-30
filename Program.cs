
using Microsoft.EntityFrameworkCore;
using SrmBook.Data;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookInventoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookInventoryContext") ?? throw new InvalidOperationException("Connection string 'BookInventoryContext' not found.")));
builder.Services.AddDbContext<PartnerManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PartnerManagementContext") ?? throw new InvalidOperationException("Connection string 'PartnerManagementContext' not found.")));
builder.Services.AddDbContext<BookClassificationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookClassificationContext") ?? throw new InvalidOperationException("Connection string 'BookClassificationContext' not found.")));
builder.Services.AddDbContext<BookUserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookUserContext") ?? throw new InvalidOperationException("Connection string 'BookUserContext' not found.")));
builder.Services.AddControllersWithViews();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);//세션 시간
});

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
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
