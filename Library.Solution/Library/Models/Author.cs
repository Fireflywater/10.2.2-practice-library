using System.Collections.Generic;

namespace Library.Models
{
  public class Author
  {
    public int AuthorId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<BookAuthor> Books { get;}

    public Author()
    {
      this.Books = new HashSet<BookAuthor>();
    }
  }
}
