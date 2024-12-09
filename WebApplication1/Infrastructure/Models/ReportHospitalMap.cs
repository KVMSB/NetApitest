using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ReportHospitalMap
{
    public int ReportId { get; set; }

    public int HospitalId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public virtual Hospital Hospital { get; set; } = null!;

    public virtual Report Report { get; set; } = null!;
}
