namespace DfPowerBI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Identity.Web;
    using Services;
    using Services.Interface;

    [ApiController]
    [Route("api/[controller]")]
    public class PowerBIController : ControllerBase
    {
        private readonly IPowerBIEmbedService _embedService;

        public PowerBIController(IPowerBIEmbedService embedService)
        {
            _embedService = embedService;
        }

        [HttpGet("embed-url/{groupId}/{reportId}")]
        public async Task<IActionResult> GetEmbedUrl(string groupId, string reportId)
        {
            try
            {
                var embedUrl = await _embedService.GetReportEmbedUrlAsync(groupId, reportId);
                return Ok(new { EmbedUrl = embedUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }

}
