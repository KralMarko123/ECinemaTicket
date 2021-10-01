using ECinemaTicket.Domain;
using ECinemaTicket.Domain.Identity;
using ECinemaTicket.Repository;
using ECinemaTicket.Repository.Implementation;
using ECinemaTicket.Repository.Interface;
using ECinemaTicket.Services;
using ECinemaTicket.Services.Implementation;
using ECinemaTicket.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System.Threading.Tasks;

namespace DomasnaIntegrirani
{
    public class Startup
    {
        private EmailSettings emailService;
        public Startup(IConfiguration configuration)
        {

            emailService = new EmailSettings();
            Configuration = configuration;
            Configuration.GetSection("EmailSettings").Bind(emailService);
          
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ECinemaApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            //services.AddIdentity<ECinemaApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));


            //services.AddScoped(es => emailService);
            //services.AddScoped<IEmailService, EmailService>(email => new EmailService(emailService));
            //services.AddScoped<IBackgroundEmailSender, BackgroundEmailSender>();
            //services.AddHostedService<ConsumeScopedHostedService>();

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));


            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IOrderService, ECinemaTicket.Services.Implementation.OrderService>();







            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddRazorPages();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Account/Login";
                options.LogoutPath = $"/Account/Logout";
                options.AccessDeniedPath = $"/Account/accessDenied";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        //private async Task CreateRolesandUsers()
        //{
        //    bool x = await roleManager.RoleExistsAsync("Admin");
        //    if (!x)
        //    {
        //        // first we create Admin role    
        //        var role = new IdentityRole();
        //        role.Name = "Admin";
        //        await roleManager.CreateAsync(role);

        //        //Here we create a Admin super user who will maintain the website                   

        //        var user = new ECinemaApplicationUser();
        //        user.UserName = "admin";
        //        user.Email = "admin@admin.com";

        //        string userPWD = "@admin123";

        //        IdentityResult chkUser = await userManager.CreateAsync(user, userPWD);

        //        //Add default User to Role Admin    
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = await userManager.AddToRoleAsync(user, "Admin");
        //        }
        //    }

        //    bool y = await roleManager.RoleExistsAsync("User");
        //    if (!x)
        //    {
        //        // first we create Admin role    
        //        var role = new IdentityRole();
        //        role.Name = "User";
        //        await roleManager.CreateAsync(role);

        //        //Here we create a Admin super user who will maintain the website                   

        //        var user = new ECinemaApplicationUser();
        //        user.UserName = "user";
        //        user.Email = "user@user.com";

        //        string userPWD = "@user123";

        //        IdentityResult chkUser = await userManager.CreateAsync(user, userPWD);

        //        //Add default User to Role Admin    
        //        if (chkUser.Succeeded)
        //        {
        //            var result2 = await userManager.AddToRoleAsync(user, "User");
        //        }
        //    }
        //}
    }
}
