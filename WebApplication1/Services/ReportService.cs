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
            var reports = new List<ReportResponce>();
            var workspace = res?.FirstOrDefault();
            foreach (var item in workspace?.ReportHospitalMaps)
            {
                reports.Add(new ReportResponce
                {
                    HospitalId = item.HospitalId,
                    HospitalName = item.Hospital.Name,
                    ReportID = workspace.ReportId,
                    Name = workspace.Name,
                    WorkspaceID = workspace.WorkspaceId,
                    Id = workspace.Id
                });
            }
            

            return reports;
        }
    }
}
