using FinanceTracker.Utility;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Repository;
using FinanceTracker.BLL.Interface;
using FinanceTracker.BLL.Services;
using FinanceTracker.DAL;
using FinanceTracker.BLL;
using FinanceTracker.Utility.Interface;
using FinanceTracker.Utility.Services;
using FinanceTracker.Utility.Model;
using FinanceTracker.Hangfire.Interface;
using FinanceTracker.Hangfire.Services;
using FinanceTracker.Hangfire.Model;
using FinanceTracker.DAL.Model;


namespace FinanceTracker.Extension
{
    public static class FinanceTrackerExtension
    {
        public static void AddExtensionServices(this IServiceCollection services){
           services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                    new BadRequestObjectResult
                    (
                        Envelope.ErrorMessage(actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))
                    );
            });
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IBackgroundJobs, BackgroundJobs>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IAuthorityRegistrationServices, AuthorityRegistrationServices>();
            services.AddScoped<IRestServiceClientRepository, RestServiceClientRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddTransient<ICountryServices, CountryServices>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddTransient<ICityService, CityService>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddTransient<IStateService, StateService>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddTransient<IStatusServices, StatusServices>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddScoped<ICategoryTypeRepository, CategoryTypeRepository>();
            services.AddTransient<ICategoryTypeService, CategoryTypeService>();
            services.AddScoped<IEditUserRepository, EditUserRepository>();
            services.AddTransient<IEditUserService, EditUserService>();
            services.AddScoped<IAdminCreateUserRepository, AdminCreateUserRepository>();
            services.AddTransient<IAdminCreateUserServices, AdminCreateUserServices>();
            services.AddScoped<IUserDetailsService, UserDetailsService>();
            services.AddTransient<IUserDetailsRepository, UserDetailsRepository>();
            services.AddTransient<IChangePasswordService, ChangePasswordService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddTransient<INotificationService, NotificationService>();          
            services.AddTransient<IIncomeExpenseService,IncomeExpenseService>();
            services.AddScoped<IIncomeExpenseRepository,IncomeExpenseRepository>();
            services.AddScoped<IReminderRepository,ReminderRepository>();
            services.AddTransient<IReminderService,ReminderService>();
            services.AddScoped<IReminderAlertRepository,ReminderAlertRepository>();
            services.AddTransient<IReminderAlertService,ReminderAlertService>();
            services.AddScoped<IUserStatusCountRepository,UserStatusCountRepository>();
            services.AddTransient<IUserStatusCountService,UserStatusCountService>();
            services.AddTransient<ITransactionSummaryService,TransactionSummaryService>();
            services.AddScoped<ITransactionSummaryRepository,TransactionSummaryRepository>();
            services.AddSingleton<MailSettings>();
            services.AddSingleton<HangfireSettings>();
            services.AddTransient<ReminderService>();
            services.AddSingleton<ImagePathModel>();
        }
    }
}