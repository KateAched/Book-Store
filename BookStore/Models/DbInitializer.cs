using BookStore.Areas.Identity.Data;
using BookStore.Data;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(BookStoreContext context, IServiceProvider serviceProvider, UserManager<BookStoreUser> userManager)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] RoleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in RoleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string Email = "User@bookstoresite.com";
            string password = "User,./123";
            if (userManager.FindByEmailAsync(Email).Result == null)
            {
                BookStoreUser user = new BookStoreUser();
                user.UserName = Email;
                user.Email = Email;
                IdentityResult result = userManager.CreateAsync(user,password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
        }
    }
}
