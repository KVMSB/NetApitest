using Domain.Infrastructure;
using Infrastructure.Models;

namespace Services.Interface
{
    public interface IPowerBIEmbedService
    {
        Task<EmbeddedReportModel> GetReportEmbedUrlAsync(string groupId, string reportId);
    }
}
