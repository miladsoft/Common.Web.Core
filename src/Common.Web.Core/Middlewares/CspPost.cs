using System.Text.Json.Serialization;

namespace Common.Web.Core
{
    /// <summary>
    /// CSP Posted Model
    /// </summary>
    public class CspPost
    {
        /// <summary>
        /// The posted errors data
        /// </summary>
        [JsonPropertyName("csp-report")]
        public CspReport CspReport { get; set; } = new CspReport();
    }
}