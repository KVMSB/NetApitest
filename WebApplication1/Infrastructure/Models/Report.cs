using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Report
{
    public int Id { get; set; }

    public string ReportId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string WorkspaceId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ReportHospitalMap> ReportHospitalMaps { get; set; } = new List<ReportHospitalMap>();
}
