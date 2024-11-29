using Microsoft.EntityFrameworkCore;
using Demo_Code_First.Models;
namespace Demo_Code_First
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);  // Thời gian session hết hạn
                options.Cookie.HttpOnly = true;  // Cookie chỉ có thể truy cập qua HTTP
            });



            var connection = builder.Configuration.GetConnectionString("connectionString");
            builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connection));

            // Add services to the container.
            builder.Services.AddControllersWithViews();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
