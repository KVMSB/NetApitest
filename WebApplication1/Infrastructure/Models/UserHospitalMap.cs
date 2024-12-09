using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class UserHospitalMap
{
    public int UserId { get; set; }

    public int HospitalId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public virtual Hospital Hospital { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
