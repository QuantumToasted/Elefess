namespace Elefess;

/// <summary>
/// Various Elefess utilities.
/// </summary>
public static class LfsUtil
{
    /// <summary>
    /// Various Git LFS constant values.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Git LFS transfer adapter names.
        /// </summary>
        /// <remarks>Not an exhaustive list - this includes ones mentioned by name in the API spec.</remarks>
        public static class TransferAdapters
        {
            /// <summary>
            /// The <c>basic</c> transfer adapter.
            /// </summary>
            public const string BASIC = "basic";
        }

        /// <summary>
        /// Supported Git LFS hash algorithms.
        /// </summary>
        /// <remarks>Not an exhaustive list - this includes ones mentioned by name in the API spec.</remarks>
        public static class HashAlgorithms
        {
            /// <summary>
            /// The <c>SHA-256</c> hash algorithm.
            /// </summary>
            public const string SHA256 = "sha256";
        }

        /// <summary>
        /// Supported Git LFS response action types.
        /// </summary>
        public static class Actions
        {
            /// <summary>
            /// The <c>upload</c> action type.
            /// </summary>
            public const string UPLOAD = "upload";
            
            /// <summary>
            /// The <c>download</c> action type.
            /// </summary>
            public const string DOWNLOAD = "download";
        }

        /// <summary>
        /// Git LFS header names and values.
        /// </summary>
        public static class Headers
        {
            /// <summary>
            /// Git LFS header names.
            /// </summary>
            public static class Names
            {
                /// <summary>
                /// The <c>LFS-Authenticate</c> header name.
                /// </summary>
                public const string LFS_AUTHENTICATE = "LFS-Authenticate";
            }

            /// <summary>
            /// Git LFS header values;
            /// </summary>
            public static class Values
            {
                /// <summary>
                /// An <c>LFS-Authenticate</c> header value of <c>Basic realm="Git LFS"</c>.
                /// </summary>
                public const string LFS_AUTHENTICATE_VALUE = "Basic realm=\"Git LFS\"";
                
                /// <summary>
                /// A <c>Content-Type</c> header value of <c>application/vnd.git-lfs+json</c>.
                /// </summary>
                public const string CONTENT_TYPE = "application/vnd.git-lfs+json";
                
                /// <summary>
                /// An <c>Accept</c> header value of <c>application/vnd.git-lfs+json</c>.
                /// </summary>
                public const string ACCEPT = CONTENT_TYPE;
            }
        }
    }
}