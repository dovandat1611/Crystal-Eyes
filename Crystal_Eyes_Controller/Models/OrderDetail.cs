﻿using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class OrderDetail
{
	public int OrderDetailId { get; set; }

	public int? OrderId { get; set; }

	public int? ProductId { get; set; }

	public int? ColorId { get; set; }

	public decimal Price { get; set; }

	public int Quantity { get; set; }

	public decimal? TotalPrice { get; set; }

	public virtual Color? Color { get; set; }

	public virtual Order? Order { get; set; }

	public virtual Product? Product { get; set; }
}
