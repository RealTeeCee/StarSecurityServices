using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class changecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TimeOpen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Youtube = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Permissions = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryBranches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryBranches_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuperAdmin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperAdmin_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoginDevice = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    LastActivity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBranchs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBranchs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBranchs_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBranchs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Education = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Grade = table.Column<byte>(type: "tinyint", nullable: true),
                    Client = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Award = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "Facebook", "Instagram", "Name", "Phone", "TimeOpen", "Twitter", "UpdatedAt", "Youtube" },
                values: new object[] { 1L, "590 CMT8", new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5134), "hcm@gmail.com", "StarFb", "StarIg", "Hồ Chí Minh", "0987654321", "01-01-2021", "StarTw", null, "StarYtb" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Image", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5147), "default.jpg", "Security Service", "security-service", null },
                    { 2L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5149), "default.jpg", "Vacancy Service", "vacancy-service", null },
                    { 3L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5151), "default.jpg", "Cash Service", "cash-service", null },
                    { 4L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5152), "default.jpg", "Train Service", "train-service", null }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "CreatedAt", "Name", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5072), "security", "Security Manager", null },
                    { 2L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5074), "vacancy", "Vacancy Manager", null },
                    { 3L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5075), "device", "Device Manager", null },
                    { 4L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5077), "train", "Train Manager", null },
                    { 5L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5078), "role", "Role Manager", null },
                    { 6L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5079), "branch", "Branch Manager", null },
                    { 7L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5080), "user", "User Manager", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Name", "Permissions", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(4966), "Super Administrator", "{{\"security\":[\"read\",\"add\",\"edit\",\"delete\"], \"device\":[\"read\",\"add\",\"edit\",\"delete\"], \"role\":[\"read\",\"add\",\"edit\",\"delete\",\"permission\"],\"post\":[\"read\",\"add\",\"edit\",\"delete\"],\"user\":[\"read\",\"add\",\"edit\",\"delete\"],\"branch\":[\"read\",\"add\",\"edit\",\"delete\"],\"vacancy\":[\"read\",\"add\",\"edit\",\"delete\"],\"train\":[\"read\",\"add\",\"edit\",\"delete\"]}}", null },
                    { 2L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(4968), "General Administrator", "{}", null },
                    { 3L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(4969), "Administrator", "{}", null },
                    { 4L, new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(4970), "Employee", "{}", null }
                });

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "Image", "Name", "Password", "Phone", "RoleId", "Status", "UpdatedAt" },
                values: new object[] { 1L, " 590 CMT8", new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5099), "admin@gmail.com", "default.jpg", "Super Admin", "123456", " 0123456789", 1L, (byte)1, null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "Image", "Name", "Password", "Phone", "RoleId", "Status", "UpdatedAt" },
                values: new object[] { 1L, " 590 CMT8", new DateTime(2022, 12, 14, 14, 51, 18, 372, DateTimeKind.Local).AddTicks(5115), "g_admin@gmail.com", "default.jpg", "General Admin", "123456", " 0123456789", 2L, (byte)1, null });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBranches_BranchId",
                table: "CategoryBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBranches_CategoryId",
                table: "CategoryBranches",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryId",
                table: "Services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SuperAdmin_RoleId",
                table: "SuperAdmin",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranchs_BranchId",
                table: "UserBranchs",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranchs_UserId",
                table: "UserBranchs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryBranches");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "SuperAdmin");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "UserBranchs");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
