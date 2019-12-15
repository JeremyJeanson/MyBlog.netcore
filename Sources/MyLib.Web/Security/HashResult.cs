using System;

namespace MyLib.Web.Security
{
    /// <summary>
    /// Hash and Salt used
    /// </summary>
    public sealed class HashResult
    {
        /// <summary>
        /// Hash
        /// </summary>
        public String Hash { get; set; }

        /// <summary>
        /// Salt used to create the hash
        /// </summary>
        public String Salt { get; set; }
    }
}
