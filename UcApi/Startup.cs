using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using entityframework;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.AspNetCore;
using Service.Imp;
using Service.Interface;

namespace UcApi
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
            services.AddMvc();

            //var connection =
            //    @"data source=.;initial catalog=VueDiagnosis;persist security info=True;user id=sa;password=sa_2007;";
            //services.AddDbContext<UcDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddSingleton(typeof(IDesignTimeDbContextFactory<>), typeof(DesignTimeDbContextFactory));
            DIRegister(services);

        }

      

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //TODO  ugly
            using (var context = new UcDbContext())
            {
                context.Database.EnsureCreated();
            }

            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
            });
         

            app.UseMvc();
        }



        private static void DIRegister(IServiceCollection services)
        {
            var types = typeof(IUserService).Assembly.GetTypes();
            var ser = types.Where(t => !t.IsInterface && t.Name.EndsWith("Service"));
            foreach (var service in ser)
            {
                var iService = service.GetInterface("I" + service.Name, true);
                if (iService != null)
                {
                    services.AddTransient(iService, service);
                }
            }
        }
    }
}
