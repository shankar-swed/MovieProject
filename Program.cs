using CodWizardsMovieShop.Data;
using CodWizardsMovieShop.Services;
using Microsoft.EntityFrameworkCore;


namespace CodWizardsMovieShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
			builder.Services.AddDistributedMemoryCache(); // Required for session state

			builder.Services.AddSession();
			builder.Services.AddHttpContextAccessor();//checking session in cshtml so needed to add this



			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			//var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
            builder.Services.AddDbContext<CWDBContext>(o => o.UseSqlServer(connectionString));

            //Add user services here
            builder.Services.AddScoped<ICustomerServices, CustomerServices>();
            builder.Services.AddScoped<IMovieServices, MovieServices>();
            builder.Services.AddScoped<IOrderServicescs, OrderServices>();
            


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
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Customer}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
