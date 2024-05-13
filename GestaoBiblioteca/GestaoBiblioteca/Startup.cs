using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GestaoBiblioteca.Context;
using GestaoBiblioteca.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using GestaoBiblioteca.Services;

namespace GestaoBiblioteca
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            /*
             DB_HOST=localhost
                DB_PASS=123456
             */
            var host = configuration["DB_HOST"];
            var pass = configuration["DB_PASS"];
            var string_conexao = configuration["CONECT_STRING_SQLSERVER"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GestaoBibliotecaContext>(
                context => context.UseSqlServer(
                    Configuration.GetConnectionString("SqlServerConnection")
                )
            );

            services.AddScoped<BibliotecaService>();

            //services.AddScoped<CourseService>();
            services.AddControllers(f =>
            {
                //f.Filters.Add<MyExceptionFilter>();
            });

            services.AddControllers()
                .AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddScoped<IRepository, Repository>();
            //services.AddSingleton<IRepository, Repository>();
            //services.AddTransient<IRepository, Repository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gestão de Biblioteca", Version = "v1" });
                
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                if(xmlCommentsFullPath != null)
                    c.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddCors();
            //services.addAu;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICourse v1"));
            }

            // Verifica e aplica migrações ao iniciar o aplicativo
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GestaoBibliotecaContext>();
                dbContext.Database.MigrateAsync();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIGestaoBiblioteca v1"));


            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
