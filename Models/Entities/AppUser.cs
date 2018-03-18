using Microsoft.AspNetCore.Identity;

namespace CountriesListApi.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}