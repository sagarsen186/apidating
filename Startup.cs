using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Register.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace Register
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
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "AllowOrigin", builder =>
            //    {
            //        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
            //    });
            //});
            services.AddCors();
            services.AddDbContext<UserContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            x.TokenValidationParameters=new TokenValidationParameters
            {
                ValidateIssuer= true,
                ValidateAudience= true, 
                ValidateLifetime= true,
                ValidateIssuerSigningKey= true,
                ValidIssuer="localhost",
                ValidAudience= "localhost",
                IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwtConfig:Key"])),
                ClockSkew=TimeSpan.Zero
            }

            );
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //var builder = WebApplication.CreateBuilder(args);
            //app.UseExceptionHandler(
            // options =>
            // {
            //     options.Run(
            //         async context =>
            //         {
            //             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //             var ex = context.Features.Get<IExceptionHandlerFeature>();
            //             if (ex != null)
            //             {
            //                 await context.Response.WriteAsync(ex.Error.Message);
            //             }
            //         });

            // });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
