using System;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.DataFactory;
using FundraisingandEngagement.Services;
using FundraisingandEngagement.Utils.ConfigModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
	public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EncryptionUtilsConfig>(Configuration.GetSection("EncryptionUtilsConfig"));
            services.Configure<SaltStringConfig>(Configuration.GetSection("APIKeys"));

            var connectionStringBuilder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("PaymentContext"));

			if (!String.IsNullOrEmpty(Configuration["ConnectionSecrets:PaymentContextUserID"]))
			{
				connectionStringBuilder.UserID = Configuration["ConnectionSecrets:PaymentContextUserID"];
				connectionStringBuilder.Password = Configuration["ConnectionSecrets:PaymentContextPassword"];
			}

			var paymentConnectionString = connectionStringBuilder.ConnectionString;

            services.AddDbContext<PaymentContext>(options => options.UseSqlServer(paymentConnectionString, o => o.MigrationsAssembly("Data.Migrations")));
            services.AddScoped<IPaymentContext>(provider => provider.GetRequiredService<PaymentContext>());
            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();

            services.AddTransient<SaltString>();
            services.AddScoped<DataFactory>();

            services
                .AddAuthentication("Padlock")
                .AddScheme<AuthenticationSchemeOptions, PadlockAuthenticationHandler>("Padlock", _ => { });

            // This policy is only used on Images controller:
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            services
                .AddControllers(o => o.Filters.Add(new AuthorizeFilter()))
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapControllers();
            });
        }
    }
}
