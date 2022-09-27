using Microsoft.AspNetCore.Identity;

namespace lochess.Models
{
    public class AspNetUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? GroupName { get; set; }
    }
}
