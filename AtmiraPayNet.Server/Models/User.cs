using Microsoft.AspNetCore.Identity;

namespace AtmiraPayNet.Server.Models
{
    public class User : IdentityUser
    {
        // Relationships
        public required List<Payment> Payments { get; set; }

        // Properties
        public required string FullName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
    }
}
