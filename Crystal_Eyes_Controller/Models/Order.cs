using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Order
{
	public int OrderId { get; set; }

	public int? UserId { get; set; }

	public string NameReceiver { get; set; } = null!;

	public string PhoneReceiver { get; set; } = null!;

	public string AddressReceiver { get; set; } = null!;

	public string? ContentReservation { get; set; }

	public string OrderStatus { get; set; } = null!;

	public decimal TotalAmount { get; set; }

	public DateTime? PendingAt { get; set; }

	public DateTime? AcceptAt { get; set; }

	public DateTime? ShippingAt { get; set; }

	public DateTime? SuccessAt { get; set; }

	public DateTime? CancelAt { get; set; }

	public string? CancelReason { get; set; }

	public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

	public virtual User? User { get; set; }
}

