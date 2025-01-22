using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public string? Content { get; set; }

    public int? Star { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
