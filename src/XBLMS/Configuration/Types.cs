namespace XBLMS.Configuration
{
    public static class Types
    {
        public static class Claims
        {
            public const string UserId = System.Security.Claims.ClaimTypes.NameIdentifier;
            public const string UserName = System.Security.Claims.ClaimTypes.Name;
            public const string Role = System.Security.Claims.ClaimTypes.Role;
            public const string IsPersistent = System.Security.Claims.ClaimTypes.IsPersistent;
        }

        public static class Roles
        {
            public const string Administrator = nameof(Administrator);
            public const string User = nameof(User);
        }

    }
}
