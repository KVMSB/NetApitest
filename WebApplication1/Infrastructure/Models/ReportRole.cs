using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ReportRole
{
    public int ReportId { get; set; }

    public int RoleId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public virtual Report Report { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
