using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? MainImage { get; set; }

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public string? SubDescription { get; set; }

    public string? Description { get; set; }

    public string? AddInfo { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    public virtual ICollection<Color> Colors { get; } = new List<Color>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<Wishlist> Wishlists { get; } = new List<Wishlist>();
}
