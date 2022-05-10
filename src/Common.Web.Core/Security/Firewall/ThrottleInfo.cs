using System;
using static System.FormattableString;

namespace Common.Web.Core
{
    /// <summary>
    /// Client's info
    /// </summary>
    public class ThrottleInfo
    {
        /// <summary>
        /// ThrottleInfo's expiration time.
        /// </summary>
        public DateTimeOffset ExpiresAt { get; set; }

        /// <summary>
        /// Number of the client's requests.
        /// </summary>
        public int RequestsCount { get; set; }

        /// <summary>
        /// Has this action been logged?
        /// </summary>
        public bool IsLogged { set; get; }

        /// <summary>
        /// Ban Reason
        /// </summary>
        public string BanReason { set; get; } = default!;

        /// <summary>
        /// ToString()
        /// </summary>
        public override string ToString()
        {
            return Invariant($"RequestsCount: {RequestsCount}, ExpiresAt: {ExpiresAt},  Reason: {BanReason}");
        }
    }
}