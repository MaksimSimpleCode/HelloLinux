using HelloLinux.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloLinux
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            Dictionary<string, string> emailPassword = new Dictionary<string, string>()
            {
                { "admin@gmail.com","qwerty"},   //_Aa123456
                { "test@mail.ru","qwerty" }
            };
           
            //string adminEmail = "admin@gmail.com";
            //string password = "_Aa123456";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("employee") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("employee"));
            }
            foreach (var el in emailPassword)
            {
                if (await userManager.FindByNameAsync(el.Key) == null)
                {
                    User user = new User { Email = el.Key, UserName = el.Key };
                    IdentityResult result = await userManager.CreateAsync(user, el.Value);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                }
            }
            
        }
    }
}
