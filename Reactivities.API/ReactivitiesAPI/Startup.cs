using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reactivities.API.Extensions;
using Reactivities.API.Middleware;
using Reactivities.API.SignalR;
using Reactivities.Application.Activities;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace ReactivitiesAPI
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Create>();
            });
            services.AddApplicationServices(_config);
            services.AddIdentityServices(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReactivitiesDbContext context, UserManager<AppUser> userManager)
        {
            //custom exception handling middleware
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseXContentTypeOptions();
            //app.UseReferrerPolicy(opt => opt.NoReferrer());
            //app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
            //app.UseXfo(opt => opt.Deny());
            //app.UseCspReportOnly(opt => opt
            //    .BlockAllMixedContent()
            //    .StyleSources(s => s.Self())
            //    .FontSources(s => s.Self())
            //    .FormActions(s => s.Self())
            //    .FrameAncestors(s => s.Self())
            //    .ImageSources(s => s.Self())
            //    .ScriptSources(s => s.Self())
            //);

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReactivitiesAPI v1"));
            }
            //else
            //{
            //    app.Use(async (context, next) =>
            //    {
            //        context.Response.Headers.Add("Strict-Transport-Security", "max-age-31536000");
            //        await next.Invoke();
            //    });
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            DataSeeding.SeedData(context, userManager).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
