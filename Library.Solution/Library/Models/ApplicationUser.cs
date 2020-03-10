using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string Type { get; set; } // Librarian OR Patron
  }
}
