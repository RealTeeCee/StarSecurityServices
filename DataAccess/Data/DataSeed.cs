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
                    UserName = "TeeCee",
                    NormalizedUserName ="TEECEE",
                    Email = "nguyenngocnguyen.rtc@starsec.com",
                    NormalizedEmail ="NGUYENNGOCNGUYEN.RTC@STARSEC.COM",
                    Name = "Nguyễn Ngọc Nguyên"
                }
            };
            
            modelBuilder.Entity<User>().HasData(users);

            //CREATE USER PASSWORD
            var passwordHasher = new PasswordHasher<User>();
            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "123456@");

            //ASSIGN ROLE TO USER
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId = roles.First(sa => sa.Name == "SuperAdmin").Id
            });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);



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
