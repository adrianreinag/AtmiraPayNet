using Microsoft.AspNetCore.Identity;

namespace AtmiraPayNet.Server.Models
{
    public class User : IdentityUser
    {
        // Relationships
        required public List<Payment> Payments { get; set; }

        // Properties
        required public string Fullname { get; set; }
        required public DateOnly DateOfBirth { get; set; }
    }
}
