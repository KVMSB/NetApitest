namespace Services
{
    using AutoMapper;
    using Domain.ReposneModels;
    using Infrastructure.Repositories.Interface;
    using Services.Interface;

    public class ReportService: IReportService
    {
        private readonly IReportRepo _reportRepo;
        private readonly IMapper _mapper;

        public ReportService(IReportRepo reportRepo,IMapper mapper)
        {
            this._reportRepo = reportRepo;
            this._mapper = mapper;
        }

        public async Task<List<ReportResponce>> GetReports(string email)
        {
            var res = await _reportRepo.GetReportsByEmailAsync(email);
            var reports = _mapper.Map<List<ReportResponce>>(res);

            return reports;
        }
    }
}
