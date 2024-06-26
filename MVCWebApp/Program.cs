﻿using AzureStorageLibrary;
using AzureStorageLibrary.Services;

namespace MVCWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            ConnectionStrings.AzureStorageConnectionString = builder.Configuration.GetConnectionString("StorageConStr");

            builder.Services.AddScoped(typeof(INoSqlStorage<>),typeof(TableStorage<>));
            builder.Services.AddSingleton<IBlobStorage, BlobStorage>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("https://localhost:7138")
                        .AllowCredentials());
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
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "FirstMvcApp",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ImageHub>("/imageHub");
            });



            app.Run();
        }
    }
}