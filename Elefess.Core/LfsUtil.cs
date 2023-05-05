using System.Text;
using System.Text.RegularExpressions;

namespace Elefess.Core;

public static partial class LfsUtil
{
    public static class Constants
    {
        public static class TransferAdapters
        {
            public const string BASIC = "basic";
        }

        public static class HashAlgorithms
        {
            public const string SHA256 = "sha256";
        }

        public static class Actions
        {
            public const string UPLOAD = "upload";
            public const string DOWNLOAD = "download";
        }

        public static class Headers
        {
            public static class Names
            {
                public const string LFS_AUTHENTICATE = "LFS-Authenticate";
            }

            public static class Values
            {
                public const string LFS_AUTHENTICATE_VALUE = "Basic realm=\"Git LFS\"";
                public const string CONTENT_TYPE = "application/vnd.git-lfs+json";
                public const string ACCEPT = CONTENT_TYPE;
                
            }
        }
    }
}