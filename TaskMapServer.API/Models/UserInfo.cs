using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace dotnetserver.Models
{
    public class UserMainInfo
    {
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? avatar { get; set; }
    }
    public class SignUpUser : UserMainInfo
    {
        public string password { get; set; }
    }
    public class UserData : UserMainInfo
    {
        public uint userId { get; set; }
        public uint lastBoardId { get; set; }

    }
    public class LoginResult : UserData
    {

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }

    public class OAuthRequest
    {
        [JsonPropertyName("idToken")]
        [Required]
        public string IdToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenResponse : RefreshTokenRequest
    {

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }

}
