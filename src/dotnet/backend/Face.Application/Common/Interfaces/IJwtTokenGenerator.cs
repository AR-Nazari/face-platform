namespace Face.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generate a JWT token for the specified user.
        /// </summary>
        /// <param name="userId">Numeric identifier of the user.</param>
        /// <param name="userName">User name (login).</param>
        /// <param name="role">Application role (e.g. Admin).</param>
        /// <param name="preferredLanguage">Preferred UI language (e.g. "fa-IR", "en-US").</param>
        /// <returns>Signed JWT token string.</returns>
        string GenerateToken(int userId, string userName, string role, string preferredLanguage);
    }
}
