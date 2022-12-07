using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class StarSecurityDbContextFactory : IDesignTimeDbContextFactory<StarSecurityDbContext>
    {
        public StarSecurityDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("StarDB");
            var optionsBuilder = new DbContextOptionsBuilder<StarSecurityDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
           return new StarSecurityDbContext(optionsBuilder.Options);
        }
    }
}
