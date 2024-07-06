using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Resources;
using FinanceTracker.DAL.Interface.TokenRepository;
using FinanceTracker.DAL.Repository.TokenRepository;
using FinanceTracker.Model.AuthoritySetting;
using Hangfire;
using FinanceTracker.Utility.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FinanceTracker.Extension;
using QuestPDF.Infrastructure;
using FinanceTracker.BLL.Services;
using FinanceTracker.Hangfire.Model;
using FinanceTracker.DAL.Model;


namespace FinanceTracker
{
    public class Startup
    {
        public FinanceTrackerConfiguration Configuration { get; }
        private static bool _jobsScheduled = false;
        private static readonly object _lock = new object();

        [System.Obsolete("This Constructor is Deprecated")]
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = new FinanceTrackerConfiguration(builder.Build());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var option = Configuration.Authority;
                options.Authority = option.Authority;
                options.Audience = option.ApiName;
            });

            services.AddAuthorizationCore();
            services.AddSwaggerGen();

            services.AddScoped<HttpClient>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers();

            var connectionString = Configuration.GetConnectionString("DbConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<HangfireSettings>(Configuration.GetSection("HangfireSettings"));
            services.Configure<ImagePathModel>(Configuration.GetSection("ImagePath"));


            services.AddSingleton<FinanceTrackerConfiguration>();

            services.AddCors(x => x.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddSingleton(new AuthorityModel
            {
                TokenUrl = Configuration.GetSection(ValidationResources.TokenURL).Value,
                ClientId = Configuration.GetSection(ValidationResources.ClientID).Value,
                Secret = Configuration.GetSection(ValidationResources.AuthoritySecret).Value,
                BaseUri = Configuration.GetSection(ValidationResources.BaseUri).Value,
                BasePut = Configuration.GetSection(ValidationResources.BasePut).Value
            });

            services.AddExtensionServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHangfireDashboard();
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });
            string jobId = Resources.JobId;
        //     if (!_jobsScheduled)
        //     {
        //         lock (_lock)
        //         {
        //             if (!_jobsScheduled)
        //             {
        //                 RecurringJobMethod(recurringJobs, jobId);
        //                 _jobsScheduled = true;
        //             }
        //         }
        //     }
        }
        // [DisableConcurrentExecution(180)]
        // public void RecurringJobMethod(IRecurringJobManager recurringJobs, string jobId)
        // {
        //     recurringJobs.AddOrUpdate<ReminderService>(jobId, x => x.CheckReminderEmailToSend(),
        //         Configuration.GetSection(Resources.CronExpression).Value);
        // }
    }
}
