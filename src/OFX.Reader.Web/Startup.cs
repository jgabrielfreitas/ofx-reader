using System.IO;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using OFX.Reader.Application.Interfaces.Infrastructure;
using OFX.Reader.Application.Interfaces.Persistence;
using OFX.Reader.Application.OFX.Commands.Create;
using OFX.Reader.Infrastructure.FileManager;
using OFX.Reader.Persistence;
using OFX.Reader.Persistence.Configuration;

namespace OFX.Reader.Web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(typeof(CreateOFXCommandHandler).GetTypeInfo().Assembly);

            services.AddTransient<IOFXFileReader, OFXFileReader>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddSingleton(new OFXDirectorySettings());

            services.AddSingleton(new DatabaseSettings {
                Database = this.Configuration.GetSection("DbSettings")["Database"],
                Host = this.Configuration.GetSection("DbSettings")["Host"],
                Password = this.Configuration.GetSection("DbSettings")["Password"],
                Port = this.Configuration.GetSection("DbSettings")["Port"],
                User = this.Configuration.GetSection("DbSettings")["User"],
                Configurations = this.Configuration.GetSection("DbSettings")["Configurations"]
            });

            services.AddTransient<IDatabaseConnector, DatabaseConnector>();

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                );
            
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
