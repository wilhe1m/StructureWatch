using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using wilhe1m.StructureWatch.Services;

namespace  wilhe1m.StructureWatch
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration){
             Configuration = configuration;
          
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //SETUP 
           
            EVESwagger.InitClient(new SwaggerConfig(Configuration.GetSection("EsiConfig")));

            services.Configure<CookiePolicyOptions>(options =>{
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;


                
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
                options => {
                options.LoginPath= new PathString("/api/SSO/Login");
                options.LogoutPath= new PathString("/api/SSO/logout");
                options.AccessDeniedPath= new PathString("/Unauthorized");
                options.Cookie.Name = "StructureWatch_Auth";
              
                }
            );
           
            services.AddControllers();
            services.AddRazorPages()
                    .AddRazorRuntimeCompilation();

            services.AddDbContext<Models.StructureContext>(options =>options.UseSqlite());
    
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else{
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithRedirects("/Status?code={0}");
              
            }
            app.UseHttpsRedirection();
           
            app.UseStaticFiles();

            app.UseCookiePolicy();
           
            app.UseAuthentication();
           
            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
