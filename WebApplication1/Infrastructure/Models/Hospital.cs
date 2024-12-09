using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Hospital
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ReportHospitalMap> ReportHospitalMaps { get; set; } = new List<ReportHospitalMap>();

    public virtual ICollection<UserHospitalMap> UserHospitalMaps { get; set; } = new List<UserHospitalMap>();
}
