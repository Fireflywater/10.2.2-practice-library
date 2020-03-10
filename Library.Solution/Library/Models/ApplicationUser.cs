using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
  public class ApplicationUser : IdentityUser
  {
    public bool IsLibrarian { get; set; }
  }
}
