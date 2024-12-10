
namespace DfPowerBI.Controllers
{
    using Domain.ReposneModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Identity.Web;
    using Services.Interface;

    [AuthorizeForScopes]
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
            try
            {
                var email = User.Claims.FirstOrDefault(static x => x.Type == "preferred_username").Value;
                return await reportService.GetReports(email);
            }
            catch (Exception ex)
            {
                throw new Exception("invalid user");
            }
        }
    }
}
