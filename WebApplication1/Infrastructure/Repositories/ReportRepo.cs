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
            .Where(report => report.ReportHospitalMaps
                .Any(reportRole => reportRole.Hospital.UserHospitalMaps
                    .Any(userRole => userRole.User.Email.ToLower() == email)))
            .ToListAsync();
        }
    }
}
