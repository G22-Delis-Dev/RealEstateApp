using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Infrastructure.Identity.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? IdCard { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; } = false; // se inicializa como false
}
