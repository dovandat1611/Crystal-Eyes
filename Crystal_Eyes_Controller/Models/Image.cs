﻿using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Image
{
	public int ImageId { get; set; }

	public int? ProductId { get; set; }

	public string ImageUrl { get; set; } = null!;

	public bool? IsDelete { get; set; }

	public virtual Product? Product { get; set; }
}