using System.Collections.Generic;
using System;

namespace Library.Models
{
  public class Checkout
  {
    public int CheckoutId { get; set; }
    public bool Active { get; set; } // 0 = not active, 1 = active, most recent
    public DateTime DateIn { get; set; }
    public DateTime DateDue { get; set; } // Now(), +2 weeks, hardcode
    public virtual Book Book { get; set; }
    public virtual ApplicationUser User { get; set; }

    /*public bool OverDue() {
      if (Now() IsSomehowAfter DateDue) {
        return true;
      }
      else
      {
        return false;
      }
    }*/
  }
}
