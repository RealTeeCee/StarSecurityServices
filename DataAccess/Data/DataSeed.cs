using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public static class DataSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
               new Role() { Id = 1, Name = "Super Administrator",Permissions = "{{\"security\":[\"read\",\"add\",\"edit\",\"delete\"], \"device\":[\"read\",\"add\",\"edit\",\"delete\"], \"role\":[\"read\",\"add\",\"edit\",\"delete\",\"permission\"],\"post\":[\"read\",\"add\",\"edit\",\"delete\"],\"user\":[\"read\",\"add\",\"edit\",\"delete\"],\"branch\":[\"read\",\"add\",\"edit\",\"delete\"],\"vacancy\":[\"read\",\"add\",\"edit\",\"delete\"],\"train\":[\"read\",\"add\",\"edit\",\"delete\"]}}", CreatedAt = DateTime.Now , UpdatedAt = null },
               new Role() { Id = 2, Name = "General Administrator", Permissions = "{}", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Role() { Id = 3, Name = "Administrator", Permissions = "{}", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Role() { Id = 4, Name = "Employee", Permissions = "{}", CreatedAt = DateTime.Now, UpdatedAt = null }
               );
            modelBuilder.Entity<Module>().HasData(
                new Module() { Id = 1, Name = "security", Title = "Security Manager", CreatedAt = DateTime.Now, UpdatedAt = null },
                new Module() { Id = 2, Name = "vacancy", Title = "Vacancy Manager", CreatedAt = DateTime.Now, UpdatedAt = null },
                new Module() { Id = 3, Name = "device", Title = "Device Manager", CreatedAt = DateTime.Now, UpdatedAt = null },
                new Module() { Id = 4, Name = "train", Title = "Train Manager", CreatedAt = DateTime.Now, UpdatedAt = null },
                new Module() { Id = 5, Name = "role", Title = "Role Manager", CreatedAt = DateTime.Now, UpdatedAt = null },
                new Module() { Id = 6, Name = "branch", Title = "Branch Manager", CreatedAt = DateTime.Now, UpdatedAt = null },
                new Module() { Id = 7, Name = "user", Title = "User Manager", CreatedAt = DateTime.Now, UpdatedAt = null }
                
                );

            modelBuilder.Entity<SuperAdmin>().HasData(
                new SuperAdmin()  { Id = 1, Name = "Super Admin", Email = "admin@gmail.com",Password = "123456",Phone =" 0123456789",Address=" 590 CMT8",Image="default.jpg", RoleId=1,Status=1, CreatedAt = DateTime.Now, UpdatedAt = null }                       
                 );
            modelBuilder.Entity<User>().HasData(
               new User() { Id = 1, Name = "General Admin", Email = "g_admin@gmail.com", Password = "123456", Phone = " 0123456789", Address = " 590 CMT8", Image = "default.jpg", RoleId = 2, Status = 1, CreatedAt = DateTime.Now, UpdatedAt = null }
                );         
        }
    }
}
