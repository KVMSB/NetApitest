namespace Infrastructure.Repositories
{
    using Infrastructure.Models;
    using Infrastructure.Repositories.Interface;
    using Microsoft.EntityFrameworkCore;

    public class ReportRepo:IReportRepo
    {
        private readonly DfpowerbiDevContext _context;

        public ReportRepo(DfpowerbiDevContext context)
        {
            _context = context;
        }

        public async Task<List<Report>> GetReportsByEmailAsync(string email)
        {
            // Retrieve reports for a user based on their email
            return await _context.Reports
                  .Include(report => report.ReportHospitalMaps) // Include ReportHospitalMaps
        .ThenInclude(reportHospitalMap => reportHospitalMap.Hospital) // Include associated Hospitals
            .Where(report => report.ReportHospitalMaps
                .Any(reportHospital => reportHospital.Hospital.UserHospitalMaps
                    .Any(userHospital => userHospital.User.Email.ToLower() == email)))
            .ToListAsync();
        }
    }
}
