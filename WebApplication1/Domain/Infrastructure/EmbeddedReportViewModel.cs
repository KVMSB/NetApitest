using Microsoft.PowerBI.Api.Models;

namespace Domain.Infrastructure
{
    public class EmbeddedReportModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmbedUrl { get; set; }
        public string Token { get; set; }
        public List<Page> Pages { get; set; }
    }
}
