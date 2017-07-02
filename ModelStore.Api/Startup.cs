using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelStore.Api.Security;
using ModernStore.Domain.CommandHandlers;
using ModernStore.Domain.Repositories;
using ModernStore.Domain.Services;
using ModernStore.Infra.Contexts;
using ModernStore.Infra.Repositories;
using ModernStore.Infra.Services;
using ModernStore.Infra.Transactions;
using ModernStore.Shared;

namespace ModelStore.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }
        private const string ISSUER = "ClienteSolicitante";
        private const string AUDIENCE = "AplicacaoFornecedor";
        private const string SECRET_KEY = "AplicacaoFornecedor_CHAVE_DE_GERACAO_ClienteSolicitante";

        private readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SECRET_KEY));

        public Startup(IHostingEnvironment env)
        {
              var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddCors();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("ModernStore","User"));
                options.AddPolicy("Admin", policy => policy.RequireClaim("ModernStore", "Admin"));
            });

            services.Configure<TokenOptions>(options =>
            {
                options.Issuer = ISSUER;
                options.Audience = AUDIENCE;
                options.SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            });

            services.AddScoped<ModernStoreDataContext, ModernStoreDataContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<CustomerCommandHadler, CustomerCommandHadler>();
            services.AddTransient<OrderHandler, OrderHandler>();
            services.AddTransient<IEmailService, EmailService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var tokenValidationParameters= new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer =  ISSUER,

                ValidateAudience = true,
                ValidAudience = AUDIENCE,

                ValidateIssuerSigningKey =  true,
                IssuerSigningKey = _securityKey,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });
            app.UseMvc();

            Runtime.ConnectionString = Configuration.GetConnectionString("ModernStoreConnectionString");
        }
    }
}
