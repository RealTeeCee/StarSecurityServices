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
                new IdentityRole {Name = "GeneralAdmin", NormalizedName = "GENERALADMIN"},
                new IdentityRole {Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole {Name = "Employee", NormalizedName = "EMPLOYEE"}
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
                new Branch() { Id = 1, Name = "Hồ Chí Minh", Email = "hcm@gmail.com", Phone = "0987654321", Address = "590 Đ. Cách Mạng Tháng 8, Phường 11, Quận 3, Thành phố Hồ Chí Minh",Latitude = "10.787249", Longitude = "106.666595", TimeOpen = "Mon - Fri : 8 AM to 5 PM", Facebook = "StarFb", Instagram = "StarIg", Twitter = "StarTw", Youtube = "StarYtb", Image = "security-branch1.jpg", GoogleMap = "https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d3919.3236076453527!2d106.6645791!3d10.7865081!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752ed2392c44df%3A0xd2ecb62e0d050fe9!2zRlBUIEFwdGVjaCBIQ00gLSBI4buHIFRo4buRbmcgxJDDoG8gVOG6oW8gTOG6rXAgVHLDrG5oIFZpw6puIFF14buRYyBU4bq_IChTaW5jZSAxOTk5KQ!5e0!3m2!1svi!2s!4v1672237519769!5m2!1svi!2s", CreatedAt = DateTime.Now, UpdatedAt = null }
            );
            modelBuilder.Entity<UserBranch>().HasData(
                new UserBranch() { Id = 1, UserId = users[0].Id, BranchId = 1 }                
            );

            modelBuilder.Entity<Category>().HasData(
               new Category() { Id = 1, Name = "Security Service", Image = "default.jpg", Slug = "security-service", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Category() { Id = 2, Name = "Vacancy Service", Image = "default.jpg", Slug = "vacancy-service", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Category() { Id = 3, Name = "Cash Service", Image = "default.jpg", Slug = "cash-service", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Category() { Id = 4, Name = "Train Service", Image = "default.jpg", Slug = "train-service", CreatedAt = DateTime.Now, UpdatedAt = null },
               new Category() { Id = 5, Name = "Electronic Service", Image = "default.jpg", Slug = "electronic-service", CreatedAt = DateTime.Now, UpdatedAt = null }
            );
        }
    }
}
