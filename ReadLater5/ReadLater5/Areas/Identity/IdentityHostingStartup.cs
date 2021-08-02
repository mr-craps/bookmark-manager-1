using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadLater5.Areas.Identity.Data;
using Data;

[assembly: HostingStartup(typeof(ReadLater5.Areas.Identity.IdentityHostingStartup))]
namespace ReadLater5.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDbContext<ReadLater5IdentityDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ReadLater5IdentityDbContextConnection")));

                //services.AddDbContext<ReadLaterDataContext>(options =>
                //    options.UseSqlServer(
                //        context.Configuration.GetConnectionString("DefaultConnection")));

                //services
                //.AddIdentity<IdentityUser, IdentityRole>()
                //.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //.AddEntityFrameworkStores<ReadLater5IdentityDbContext>();

            });
        }
    }
}