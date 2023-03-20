using BookSeller.Data.Enums;
using BookSeller.Data.Static;
using BookSeller.Models;
using Microsoft.AspNetCore.Identity;

namespace BookSeller.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();
                //Tác giả
                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(new List<Author>()
                    {
                        new Author()
                        {
                            FullName = "Nguyễn Nhật Ánh",
                            Bio = "Giới thiệu tác giả Nguyễn Nhật Ánh",
                            ProfilePictureURL = "nguyen-nhat-anh.jpg"
                        },
                        new Author()
                        {
                            FullName = "Paulo Coelho",
                            Bio = "Giới thiệu tác giả Paulo Coelho",
                            ProfilePictureURL = "Paulo-Coel.jpg"
                        }
                    });
                    context.SaveChanges();
                }
				if (!context.Categories.Any())
				{
					context.Categories.AddRange(new List<Category>()
					{
						new Category()
						{
							Name = "Tiểu thuyết",
							Description = "This is the Bio",

						},
						new Category()
						{
							Name = "Truyện Tranh",
							Description = "This is the Bio",

						},

						new Category()
						{
							Name = "Kinh Tế",
							Description = "This is the Bio",

						},

						new Category()
						{
							Name = "Giáo dục",
							Description = "This is the Bio",

						},

						new Category()
						{
							Name = "Khoa học",
							Description = "This is the Bio",

						},
						new Category()
						{
							Name = "Khác",
							Description = "This is the Bio",

						}
					});
					context.SaveChanges();
				}
				//Nhà Xuất Bản
				if (!context.Publishers.Any())
                {
                    context.Publishers.AddRange(new List<Publisher>()
                    {
                        new Publisher()
                        {
                            FullName = "NXB Trẻ",
                            Bio = "This is the Bio of the first publisher",
                            ProfilePictureURL = "nhaxb-tre.png"
                        },
                        new Publisher()
                        {
                            FullName = "NXB Giáo dục",
                            Bio = "This is the Bio of the second publisher",
                            ProfilePictureURL = "nhaxb-giaoduc.png"
                        }
                    });
                    context.SaveChanges();
                }
                //movie
                if (!context.Books.Any())
                {
                    context.Books.AddRange(new List<Book>()
                    {
                        new Book()
                        {
                            Name = "Tôi thấy hoa vàng trên cỏ xanh",
                            Description = "Tôi thấy hoa vàng trên cỏ xanh, Tái bản 2018",
                            Price = 150000,
                            ImageURL = "toi-thay-hoa-vang-tren-co-xanh.jpg",
                            PublishingYear = 2005,
                            AuthorId = 1,
                            PublisherId = 1,
                            CategoryId = 1
                        },
                        new Book()
                        {
                            Name = "Chúc một ngày tốt lành",
                            Description = "Mô tả các thứ",
                            Price = 100000,
                            ImageURL = "chuc-mot-ngay-tot-lanh.jpg",
                            PublishingYear = 2007,
                            AuthorId = 1,
                            PublisherId = 1,
                            CategoryId = 1
                        },
                        new Book()
                        {
                            Name = "Nhà Giả Kim",
                            Description = "Mô tả các thứ",
                            Price = 90000,
                            ImageURL = "image_nhagiakim.jpg",
                            PublishingYear = 2010,
                            AuthorId = 2,
                            PublisherId = 2,
                            CategoryId = 2
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRoleAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@bookseller.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail
                        
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@bookseller.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail
                        
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }

}
