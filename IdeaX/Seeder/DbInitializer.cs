using BCrypt.Net;
using IdeaX.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdeaX.Seeder
{
    public class DbInitializer
    {
        public static void Seed(IdeaXDbContext context)
        {
            context.Database.Migrate();
            var defaultUserId = Guid.NewGuid();
            var defaultUserId1 = Guid.NewGuid();


            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role
                    {
                        RoleName = "Admin",
                        Description = "Quản trị viên",
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = defaultUserId,
                        UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = defaultUserId,
                        IsDeleted = false
                    },
                    new Role
                    {
                        RoleName = "Founder",
                        Description = "Người dùng thông thường",
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = defaultUserId,
                        UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = defaultUserId,
                        IsDeleted = false
                    }, new Role
                    {
                        RoleName = "Investor",
                        Description = "Nhà đầu tư",
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = defaultUserId,
                        UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = defaultUserId,
                        IsDeleted = false
                    }

                );
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                new User
                {
                    Id = defaultUserId,
                    FullName = "Nguyễn Văn A",
                    Username = "vanductin",
                    Email = "vanductin1910@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123456"), 
                    CCCD = "123456789",
                    CCCDBack = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    CCCDFront = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    RoleId = context.Roles.First().Id,
                    Birthday = DateTime.UtcNow,
                    Phone = "0909123456",
                    Avatar = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    Address = "Hà Nội",
                    PreferredIndustries = "Thiết kế",
                    PreferredStages = "MVP",
                    PreferredRegions = "Việt Nam",
                    FundingRangeMin = 100000000,
                    FundingRangeMax = 1000000000,
                    CreatedOn = DateTime.UtcNow,  
                    CreatedBy = defaultUserId,
                    UpdatedOn = DateTime.UtcNow,  
                    UpdatedBy = defaultUserId,
                    IsDeleted = false
                },
                new User
                {
                    Id = defaultUserId1,
                    FullName = "Nguyễn Văn A",
                    Username = "admin",
                    Email = "admin123@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123456"),
                    CCCD = "123456789",
                    CCCDBack = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    CCCDFront = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    RoleId = context.Roles.First().Id,
                    Birthday = DateTime.UtcNow,
                    Phone = "0909123456",
                    Avatar = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    Address = "Đà nẵng",
                    PreferredIndustries = "AI, ABC",
                    PreferredStages = "MVP",
                    PreferredRegions = "Việt Nam",
                    FundingRangeMin = 100000000,
                    FundingRangeMax = 1000000000,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = defaultUserId,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = defaultUserId,
                    IsDeleted = false
                }
                );
                context.SaveChanges();
            }
   
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Kỹ thuật",
                        Description = "Ý tưởng công nghệ kỹ thuật",
                        CreatedBy = defaultUserId
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Thiết kế",
                        Description = "Ý tưởng về thiết kế sáng tạo",
                        CreatedBy = defaultUserId
                    }
                );
                context.SaveChanges();
            }

            if (!context.Ideas.Any())
            {
                context.Ideas.Add(new Idea
                {
                    Title = "Ứng dụng giao thông thông minh",
                    IdeaCode = "IDEA-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    CommunityId = Guid.NewGuid(),
                    Description = "Ý tưởng xây dựng hệ thống giám sát giao thông sử dụng AI.",
                    InitiatorId = defaultUserId,
                    CollaborationType = "Mở",
                    ImageUrls = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    CategoryId = context.Categories.First().Id,
                    Status = "Pending",
                    CopyrightStatus = true,
                    CopyrightCertificate = "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
                    Price = 200000000,
                    IsForSale = true,
                    Stage = "MVP",
                    Region = "Việt Nam",
                    CreatedBy = defaultUserId,
                    isPublic = true
                })
                ;
                context.SaveChanges();
            }


        }
    }
}
