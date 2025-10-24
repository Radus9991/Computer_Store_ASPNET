namespace WebAPI.DTO.User
{
    public class TokenCreateDTO
    {
        public string Message { get; set; } = null!;
        public string Token { get; set; } = null!;

        public static TokenCreateDTO of(string message, string token)
        {
            return new TokenCreateDTO
            {
                Message = message,
                Token = token
            };
        }
    }
}
