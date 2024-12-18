
namespace Services
{
    using Microsoft.Identity.Client;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Services.Interface;
    using Microsoft.Extensions.Configuration;
    using System.Net.Http.Json;
    using Newtonsoft.Json.Linq;
    using Infrastructure.Models;
    using System.Security.Principal;
    using Microsoft.PowerBI.Api;
    using Microsoft.Rest;
    using Microsoft.PowerBI.Api.Models;
    using Microsoft.Identity.Web;
    using Newtonsoft.Json;
    using System.Runtime.InteropServices;
    using Domain.Infrastructure;
    using Infrastructure.Repositories.Interface;

    public class PowerBIEmbedService : IPowerBIEmbedService
    {
        private readonly string tenantId = "YOUR_TENANT_ID";
        private readonly string clientId = "YOUR_CLIENT_ID";
        private readonly string clientSecret = "YOUR_CLIENT_SECRET";
        private readonly string authority;
        private readonly string[] scopes = new[] { "https://analysis.windows.net/powerbi/api/.default" };
        private readonly IReportRepo _reportRepo;

        private string urlPowerBiServiceApiRoot { get; }
        public const string powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";
        public static readonly string[] RequiredScopes = new string[] {
         "https://analysis.windows.net/powerbi/api/Report.Read.All"
     };
        public PowerBIEmbedService(IConfiguration configuration, IReportRepo reportRepo)
        {
            var azureAdSection = configuration.GetSection("AzureAd");
            tenantId = azureAdSection["TenantId"];
            clientId = azureAdSection["ClientId"];
            clientSecret = azureAdSection["ClientSecret"];

            authority = $"https://login.microsoftonline.com/{tenantId}";
            this.urlPowerBiServiceApiRoot = configuration["PowerBi:ServiceRootUrl"];
            this._reportRepo = reportRepo;
        }

        public string GetAccessTokenAsync()
        {
            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(authority)
                .Build();

            var authResult = confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync().Result;
            return authResult.AccessToken;
        }

        public PowerBIClient GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(GetAccessTokenAsync(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }

        public EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            PowerBIClient pbiClient = this.GetPowerBiClient();

            var reoirt = new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) };
            var datset = datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList();
            var workspace = targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null;
            // Create a request for getting Embed token 
            // This method works only with new Power BI V2 workspace experience
            var tokenRequest = new GenerateTokenRequestV2(

                reports: reoirt,

                datasets: datset,

                targetWorkspaces: workspace
            );

            // Generate Embed token
            var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

            return embedToken;
        }


        public async Task<EmbeddedReportModel> GetReportEmbedUrlAsync(string groupId, string reportId)
        {
            Guid WorkspaceId = new Guid(groupId);
            Guid ReportId = new Guid(reportId);
            PowerBIClient pbiClient = GetPowerBiClient();

            var hiddenPages = _reportRepo.GetReportHiddenPages(reportId);

            // Call the Power BI service API to get the embedding data.
            var report = await pbiClient.Reports.GetReportInGroupAsync(WorkspaceId, ReportId);
            var pages = await pbiClient.Reports.GetPagesAsync(WorkspaceId, ReportId);
            // Generate a read-only embed token for the report.
            var datasetId = new Guid(report.DatasetId);
            var embedToken = GetEmbedToken(ReportId, new List<Guid>() { datasetId }, WorkspaceId);
            // Return the report embedded data to caller.
            return new EmbeddedReportModel
            {
                Id = report.Id.ToString(),
                EmbedUrl = report.EmbedUrl,
                Name = report.Name,
                Token = embedToken.Token,
                Pages = pages.Value.Where(x=>!hiddenPages.ToLower().Contains(x.DisplayName.ToLower())).ToList()
            };

            //string accessToken = await GetAccessTokenAsync();
            //using HttpClient client = new();

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //client.BaseAddress = new Uri("https://api.powerbi.com/v1.0/myorg/");

            //var response = await client.GetAsync($"groups/{groupId}/reports/{reportId}");
            //response.EnsureSuccessStatusCode();

            //var jsonResponse = await response.Content.ReadAsStringAsync();
            //dynamic json = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            //string embedUrl = json.embedUrl;
            //string name = json.name;
            //string datasetId = json.datasetId;
            //var embedRequestBody = new
            //{
            //    reports = new[] { new { id = reportId } },
            //    datasets = new[] { new { id = datasetId } }
            //};

            //var embedRequest = new StringContent(JsonConvert.SerializeObject(embedRequestBody));
            //embedRequest.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //var tokenResponse = await client.PostAsync($"https://api.powerbi.com/v1.0/myorg/groups/{groupId}/reports/{reportId}/GenerateToken", embedRequest);
            //tokenResponse.EnsureSuccessStatusCode();

            //var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            //var tokenDetails = JObject.Parse(tokenJson);
            //string embedToken = tokenDetails["token"]?.ToString() ?? throw new Exception("Embed token not found.");


            //return new EmbeddedReportModel
            //{
            //    Id = reportId,
            //    EmbedUrl = embedUrl,
            //    Token = embedToken,
            //    Name = name,
            //};
        }

    }

}
