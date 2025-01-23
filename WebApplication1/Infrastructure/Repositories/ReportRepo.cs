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

        public string? GetReportHiddenPages(string reportID)
        {
            return _context.Reports.FirstOrDefault(x => x.ReportId == reportID).HiddenPages;
        }


        public async Task<string> UpdateLastLoginTimeByEmail(string email, DateTime lastLoginTime)
        {
            try
            {
                using (var context = _context)
                {
                    // Find the user by Email
                    var user = context.Users.SingleOrDefault(u => u.Email == email);

                    if (user != null)
                    {
                        // Update the LastLoginTime
                        user.LastLoginTime = lastLoginTime;

                        // Save changes to the database
                        await context.SaveChangesAsync();

                    }
                    else
                    {
                        return "User not found with the provided email.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw ex;
            }

            return "Last login time updated successfully.";

        }
    }
}
