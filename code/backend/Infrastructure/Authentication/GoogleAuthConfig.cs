using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Authentication
{
    public class GoogleAuthConfig
    {
        public const string SectionName = "Authentication:Google";

        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }
        [Required]
        public string CallbackPath { get; set; } = "/signin-google";
        public bool SaveTokens { get; set; } = true;
    }
}

