using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ReportRole> ReportRoles { get; set; } = new List<ReportRole>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
