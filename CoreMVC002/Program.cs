namespace CoreMVC002;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSession();

        var app = builder.Build();
        app.UseSession();

        // 確保有這一行預設路由（讓網址可以對應到 Controller）
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();
        app.UseSession();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        //
        app.MapControllerRoute(
            name: "GuessNumber",
            pattern: "GuessNumber",
            defaults: new { controller = "Game", action = "Index" });
        //
        app.Run();
    }
}
