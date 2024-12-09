using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public virtual ICollection<UserHospitalMap> UserHospitalMaps { get; set; } = new List<UserHospitalMap>();
}
