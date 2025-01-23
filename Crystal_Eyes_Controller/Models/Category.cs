using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Category
{
	public int CategoryId { get; set; }

	public string Name { get; set; } = null!;

	public DateTime? CreatedAt { get; set; }

	public bool? IsDelete { get; set; }

	public virtual ICollection<Product> Products { get; } = new List<Product>();
}

