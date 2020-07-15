using System;
using System.Collections.Generic;
using System.Text;

namespace MordorFanficWeb.Common.Helper
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "user", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }

            public static class JwtClaimIdentifiersAdmin
            {
                public const string Rol = "admin", Id = "id";
            }
        }
    }
}
