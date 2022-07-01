using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Data;

namespace Shop
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
            services.AddControllers();
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString")));
            services.AddScoped<DataContext, DataContext>();

            // Singleton: é criada uma única instância para todas requisições. Em outras palavras, é criada uma instância a primeira vez que é solicitada e todas as vezes seguintes a mesma instância é usada (design patter singleton);
            // Scoped: é criada uma única instância por requição. Ou seja, usando o exemplo de uma aplicação Web, quando recebe uma nova requisição, por exemplo, um click num botão do outro lado do navegador, é criada uma instância, onde o escopo é essa requisição. Então se for necessária a dependência multiplas vezes na mesma requisição a mesma instância será usada. Seria como um "Singleton para uma requisição";
            // Transient: sempre é criada uma nova instância cada vez que for necessário, independentede da requisição, basicamente new XXX cada vez que for necessário.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
