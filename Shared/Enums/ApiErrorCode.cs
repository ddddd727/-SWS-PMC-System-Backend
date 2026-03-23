namespace PMCSystem_Backend.Shared.Enums
{
    public enum ApiErrorCode
    {
        // 系统级错误 (1000-1999)
        SystemError = 1000,
        DatabaseError = 1001,
        NetworkError = 1002,

        // 业务级错误 (2000-2999)
        ValidationError = 2000,
        ResourceNotFound = 2001,
        DuplicateResource = 2002,
        BusinessRuleViolation = 2003,

        // 权限错误 (3000-3999)
        Unauthorized = 3000,
        Forbidden = 3001,
        TokenExpired = 3002,
        InvalidToken = 3003,

        // 请求错误 (4000-4999)
        BadRequest = 4000,
        InvalidParameter = 4001,
        MissingRequiredField = 4002
    }
}
