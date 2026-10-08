using System;
using System.Collections.Generic;

namespace PAW.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public decimal? RoldId { get; set; }

    public decimal? UserId { get; set; }
}
