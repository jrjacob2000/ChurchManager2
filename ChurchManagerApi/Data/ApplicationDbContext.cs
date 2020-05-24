using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChurchManagerApi.Models;

namespace ChurchManagerApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = "232EBE68-8783-4734-BAAE-6A1B1C7A4458",
                Name = "admin",
                NormalizedName = "ADMIN"
            },
            new ApplicationRole
            {
                Id = "28924E18-2381-4184-989C-71CFCFFF65B0",
                Name = "accountant",
                NormalizedName = "ACCOUNTANT"
            },
             new ApplicationRole
             {
                 Id = "1804AE3F-A188-43A4-927E-1B6F756476BE",
                 Name = "encoder",
                 NormalizedName = "ENCODER"
             });
           
        }
        public DbSet<ChurchManagerApi.Models.AccountChart> AccountCharts { get; set; }
        public DbSet<ChurchManagerApi.Models.Transaction> Transactions { get; set; }
        public DbSet<ChurchManagerApi.Models.TransactionLine> TransactionLines { get; set; }
        public DbSet<ChurchManagerApi.Models.Family> Family { get; set; }
    }
}
