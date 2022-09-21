using Microsoft.AspNetCore.Identity;

namespace lochess.Areas.Identity.Data
{
    // The ApplicationUser class inherits from IdentityUser. Additional user properties are declared here
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
