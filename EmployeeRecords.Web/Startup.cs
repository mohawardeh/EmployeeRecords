using EmployeeRecords.Data.Repositories;
using EmployeeRecords.Data.Repositories.SqlServerRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecords.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(setupAction => {
                // return 406 status code (not aceeptable) when unsupported media type is requested
                // return default media type if accept header is not exist
                setupAction.ReturnHttpNotAcceptable = true;
            });
            // add all mapping profiles from all assemblies in current domain
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Repository configuration
            services.AddTransient(typeof(SqlConnection),factory=> { return new SqlConnection(Configuration.GetConnectionString("HrTestDbConnection")); });
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IFileRepository,FileRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
