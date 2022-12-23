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
            //CREATE SUPERADMIN
            List<IdentityRole> roles = new List<IdentityRole>(){
                new IdentityRole {Name = "SuperAdmin", NormalizedName = "SUPERADMIN"},                
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);



            //CREATE USER
            List<User> users = new List<User>()
            {
                new User
                {
                    UserName = "nguyenngocnguyen.rtc@starsec.com",
                    NormalizedUserName ="NGUYENNGOCNGUYEN.RTC@STARSEC.COM",
                    Email = "nguyenngocnguyen.rtc@starsec.com",
                    NormalizedEmail ="NGUYENNGOCNGUYEN.RTC@STARSEC.COM",                    
                }
            };
            
            modelBuilder.Entity<User>().HasData(users);

            //CREATE USER PASSWORD
            var passwordHasher = new PasswordHasher<User>();
            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "SuperAdmin@123");

            //ASSIGN ROLE TO USER
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId = roles.First(sa => sa.Name == "SuperAdmin").Id
            });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);



            //modelBuilder.Entity<Role>().HasData(
            //   new Role() { Id = 1, Name = "Super Administrator",Permissions = "{{\"security\":[\"read\",\"add\",\"edit\",\"delete\"], \"device\":[\"read\",\"add\",\"edit\",\"delete\"], \"role\":[\"read\",\"add\",\"edit\",\"delete\",\"permission\"],\"post\":[\"read\",\"add\",\"edit\",\"delete\"],\"user\":[\"read\",\"add\",\"edit\",\"delete\"],\"branch\":[\"read\",\"add\",\"edit\",\"delete\"],\"vacancy\":[\"read\",\"add\",\"edit\",\"delete\"],\"train\":[\"read\",\"add\",\"edit\",\"delete\"]}}", CreatedAt = DateTime.Now , UpdatedAt = null },
            //   new Role() { Id = 2, Name = "General Administrator", Permissions = "{}", CreatedAt = DateTime.Now, UpdatedAt = null },
            //   new Role() { Id = 3, Name = "Administrator", Permissions = "{}", CreatedAt = DateTime.Now, UpdatedAt = null },
            //   new Role() { Id = 4, Name = "Employee", Permissions = "{}", CreatedAt = DateTime.Now, UpdatedAt = null }
            //   );
            //modelbuilder.entity<module>().hasdata(
            //    new module() { id = 1, name = "security", title = "security manager", createdat = datetime.now, updatedat = null },
            //    new module() { id = 2, name = "vacancy", title = "vacancy manager", createdat = datetime.now, updatedat = null },
            //    new module() { id = 3, name = "device", title = "device manager", createdat = datetime.now, updatedat = null },
            //    new module() { id = 4, name = "train", title = "train manager", createdat = datetime.now, updatedat = null },
            //    new module() { id = 5, name = "role", title = "role manager", createdat = datetime.now, updatedat = null },
            //    new module() { id = 6, name = "branch", title = "branch manager", createdat = datetime.now, updatedat = null },
            //    new module() { id = 7, name = "user", title = "user manager", createdat = datetime.now, updatedat = null }

            //    );

            //modelBuilder.Entity<SuperAdmin>().HasData(
            //    new SuperAdmin()  { Id = 1, Name = "Super Admin", Email = "admin@gmail.com",Password = "123456",Phone =" 0123456789",Address=" 590 CMT8",Image="default.jpg", RoleId=1,Status=1, CreatedAt = DateTime.Now, UpdatedAt = null }                       
            //);

            //modelBuilder.Entity<User>().HasData(
            //   new User() { Id = 1, Name = "General Admin", Email = "g_admin@gmail.com", Password = "123456", Phone = " 0123456789", Address = " 590 CMT8", Image = "default.jpg", RoleId = 2, Status = 1, CreatedAt = DateTime.Now, UpdatedAt = null }
            //);

            modelBuilder.Entity<Branch>().HasData(
                new Branch() { Id = 1, Name = "Hồ Chí Minh", Email = "hcm@gmail.com", Phone = "0987654321", Address = "590 CMT8", TimeOpen = "01-01-2021", Facebook = "StarFb", Instagram = "StarIg", Twitter = "StarTw", Youtube = "StarYtb", CreatedAt = DateTime.Now, UpdatedAt = null }
            );
            modelBuilder.Entity<UserBranch>().HasData(
                new UserBranch() { Id = 1, UserId = users[0].Id, BranchId = 1 }                
            );

            modelBuilder.Entity<Category>().HasData(
               new Category() { Id = 1, Name = "Security Service", Image = "default.jpg", Slug = "security-service", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Category() { Id = 2, Name = "Vacancy Service", Image = "default.jpg", Slug = "vacancy-service", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Category() { Id = 3, Name = "Cash Service", Image = "default.jpg", Slug = "cash-service", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Category() { Id = 4, Name = "Train Service", Image = "default.jpg", Slug = "train-service", CreatedAt = DateTime.Now, UpdatedAt = null }
            );
        }
    }
}
