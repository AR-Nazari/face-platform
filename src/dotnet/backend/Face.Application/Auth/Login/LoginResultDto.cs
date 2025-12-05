namespace Face.Application.Auth.Login
{
    public class LoginResultDto
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Preferred language of the authenticated user (e.g. "fa-IR", "en-US").
        /// </summary>
        public string PreferredLanguage { get; set; } = string.Empty;
    }
}
