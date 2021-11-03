using FoodTrucks.Common;
using FoodTrucks.Handlers;
using FoodTrucks.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FoodTrucks
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

            services.Configure<FoodTruckCSVFileOptions>(Configuration.GetSection("FoodTruckCSVFile"));
            services.AddSingleton<IFileReader, FileReader>();
            services.AddSingleton<IFoodTruckDataCSVParser, FoodTruckDataCSVParser>();
            services.AddSingleton<IFoodTruckDataSource, FoodTruckCSVFileDataSource>();
            services.AddSingleton<IFoodTruckDataService, FoodTruckDataService>();
            services.AddMediatR(typeof(GetFoodTruckByLocationIdRequestHandler));
            services.AddMediatR(typeof(GetFoodTrucksByBlockRequestHandler));
            services.AddMediatR(typeof(AddNewFoodTruckRequestHandler));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodTrucks", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodTrucks v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
