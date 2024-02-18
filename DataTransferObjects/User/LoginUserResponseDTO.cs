namespace Vesta.DataTransferObjects.User
{
    public class LoginUserResponseDTO
    {
        public string Email { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
    }
}