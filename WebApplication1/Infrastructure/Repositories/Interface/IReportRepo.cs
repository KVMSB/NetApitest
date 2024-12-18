using Infrastructure.Models;

namespace Infrastructure.Repositories.Interface
{
    public interface IReportRepo
    {
        Task<List<Report>> GetReportsByEmailAsync(string email);
        string? GetReportHiddenPages(string reportID);
    }
}
