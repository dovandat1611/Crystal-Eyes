using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    public virtual User? User { get; set; }
}
