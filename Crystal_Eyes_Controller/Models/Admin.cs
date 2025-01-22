using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class Admin
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? Dob { get; set; }

    public string? Image { get; set; }

    public virtual User User { get; set; } = null!;
}
