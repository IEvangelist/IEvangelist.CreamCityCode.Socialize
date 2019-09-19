using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEvangelist.CreamCityCode.Socialize.Configuration;
using IEvangelist.CreamCityCode.Socialize.Providers;
using IEvangelist.CreamCityCode.Socialize.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace IEvangelist.CreamCityCode.Socialize
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
            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddResponseCompression(
                options => options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif"
                    }));

            // Map services
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddSingleton<IContainerProvider, ContainerProvider>();

            // Map appsettings.json to class options
            services.Configure<ImageRepositoryOptions>(Configuration.GetSection(nameof(ImageRepositoryOptions)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection()
               .UseResponseCompression()
               .UseStaticFiles(new StaticFileOptions
               {
                   // 6 hour cache
                   OnPrepareResponse =
                       _ => _.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=21600"
               });

            app.UseMvc(routes => 
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}