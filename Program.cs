using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using MyBlog.DataAccess;


namespace MyBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/admin/signin";
                });

            builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("/Views/Static/{0}.cshtml");
            });

            builder.Services.AddScoped<BlogRepository>();
            builder.Services.AddScoped<BlogCommentRepository>();
            builder.Services.AddScoped<BlogTagRepository>();
            builder.Services.AddScoped<BlogImageRepository>();
            builder.Services.AddScoped<BlogUserRepository>();

            builder.Services.AddControllersWithViews();


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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            app.Run();

        }
    }
}