namespace Services.Interface
{
    using Domain.ReposneModels;

    public interface IReportService
    {
        Task<List<ReportResponce>> GetReports(string email);
    }
}
