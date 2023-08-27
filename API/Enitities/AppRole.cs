using Microsoft.AspNetCore.Identity;

namespace API.Enitities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}