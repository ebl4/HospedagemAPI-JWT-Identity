using Business;
using Business.Helpers;
using Business.Security;
using Domain.Model;
using HospedagemAPI.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Persistence.Context;

namespace HospedagemAPI
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
            // Configurando o acesso a dados de Acomoda��es
            services.AddDbContext<CatalogoDbContext>(options => 
                options.UseInMemoryDatabase("InMemoryDatabase"));
            services.AddScoped<AcomodacaoService>();

            // Configurando o uso da classe de contexto para
            // acesso �s tabelas do ASP.NET Identity Core
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDatabase"));

            // Ativando a utiliza��o do ASP.NET Identity, a fim de
            // permitir a recupera��o de seus objetos via inje��o de
            // depend�ncias
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configurando a depend�ncia para classes de valida��o
            // de credenciais e gera��o de tokens
            services.AddScoped<AccessManager>();

            var signinConfiguration = new SigningConfigurations();
            services.AddSingleton(signinConfiguration);

            var tokenConfiguration = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfiguration);
            services.AddSingleton(tokenConfiguration);

            // Aciona a extens�o que ir� configurar o uso de
            // autentica��o e autoriza��o via tokens
            services.AddJwtSecurity(signinConfiguration, tokenConfiguration);

            services.AddCors();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Cria��o de estruturas, usu�rios e permiss�es
            // na base do ASP.NET Identity Core (caso ainda n�o
            // existam)
            new IdentityInitializer(context, userManager, roleManager).Initialize();

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
