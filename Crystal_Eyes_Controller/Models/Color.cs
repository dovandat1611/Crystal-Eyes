﻿using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Color
{
	public int ColorId { get; set; }

	public int? ProductId { get; set; }

	public string ColorPath { get; set; } = null!;

	public string ColorName { get; set; } = null!;

	public bool? IsDelete { get; set; }

	public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

	public virtual Product? Product { get; set; }
}