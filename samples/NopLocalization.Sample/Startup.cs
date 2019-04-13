using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NopLocalization.Sample.Controllers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NopLocalization.Sample
{
    /// <summary>
    /// Represents metadata provider that adds custom attributes to the model's metadata, so it can be retrieved later
    /// </summary>
    public class NopMetadataProvider : IDisplayMetadataProvider
    {
        /// <summary>
        /// Sets the values for properties of isplay metadata
        /// </summary>
        /// <param name="context">Display metadata provider context</param>
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            //get all custom attributes
            var additionalValues = context.Attributes.OfType<DisplayAttribute>().ToList();

            //and try add them as additional values of metadata
            foreach (var additionalValue in additionalValues)
            {
                if (context.DisplayMetadata.AdditionalValues.ContainsKey(additionalValue.Name))
                    throw new Exception(string.Format("There is already an attribute with the name '{0}' on this model", additionalValue.Name));

                context.DisplayMetadata.AdditionalValues.Add(additionalValue.Name, additionalValue);
            }
        }
    }

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer("Data Source=.;Initial Catalog=xxx;Integrated Security=true");
            });

            services.AddNopLocalization(options =>
            {
                options.DefaultLanguage = "fa";
                options.OtherLanguages = new[] { "en", "ar" };
            });

            services.AddMvc()
                .AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new NopMetadataProvider()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseNopLocalization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
