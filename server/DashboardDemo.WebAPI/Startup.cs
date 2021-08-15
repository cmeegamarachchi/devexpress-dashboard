using DashboardDemo.WebAPI.Repository;
using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DashboardDemo.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDevExpressControls()
                .AddControllers()
                .AddDefaultDashboardController(configurator => {
                    configurator.DataLoading += (s, e) => {
                        if(e.DataSourceName == "Fruit Data Source") {
                            e.Data = DataRepository.GetFruits();
                        }
                        
                        if(e.DataSourceName == "Animal Data Source") {
                            e.Data = DataRepository.GetAnimals();
                        }
                    };
                    
                    configurator.SetDataSourceStorage(CreateDataSourceStorage());
                });
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DashboardDemo.WebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DashboardDemo.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseDevExpressControls();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard");
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });
        }

        public DataSourceInMemoryStorage CreateDataSourceStorage()
        {
            var dataSourceStorage = new DataSourceInMemoryStorage();

            var fruitDataSource = new DashboardObjectDataSource("Fruit Data Source");
            var animalDataSource = new DashboardObjectDataSource("Animal Data Source");
            
            dataSourceStorage.RegisterDataSource("fruitDataSource", fruitDataSource.SaveToXml());             
            dataSourceStorage.RegisterDataSource("animalDataSource", animalDataSource.SaveToXml());             
            
            return dataSourceStorage;
        }
    }
}
