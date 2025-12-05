using Face.Domain.Common;

namespace Face.Domain.Users
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";

        /// <summary>
        /// Preferred UI language of the user (e.g. "fa-IR", "en-US").
        /// Default is "fa-IR".
        /// </summary>
        public string PreferredLanguage { get; set; } = "fa-IR";
    }
}
