
namespace DfPowerBI.Controllers
{
    using Domain.ReposneModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interface;
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserReportController : ControllerBase
    {
        private IReportService reportService;

        public UserReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<List<ReportResponce>> Get() 
        {
            string email = User.Claims.FirstOrDefault(x => x.Type == "preferred_username").Value;
            return await reportService.GetReports(email);
        }
    }
}
