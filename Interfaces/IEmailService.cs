namespace Vesta.Interfaces
{
    public interface IEmailService
    {
        Task SendRandomEmailVerificationCode(string recipientEmail);

        bool VerifyEmailCode(string recipientEmail, string code);
    }
}