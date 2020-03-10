using System.Collections.Generic;

namespace Library.Models
{
  public class Book
  {
    public int BookId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Checkout> CheckoutHistory { get;}
    public virtual ICollection<BookAuthor> Authors { get;}

    public Book()
    {
      this.CheckoutHistory = new HashSet<Checkout>();
      this.Authors = new HashSet<BookAuthor>();
    }
  }
}
