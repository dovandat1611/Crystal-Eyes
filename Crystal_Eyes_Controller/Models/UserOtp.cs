using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class UserOtp
{
    public int OtpId { get; set; }

    public int? UserId { get; set; }

    public string OtpCode { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool? IsUse { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
