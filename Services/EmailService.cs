using MimeKit;
using MailKit.Net.Smtp;
using Vesta.Interfaces;
using Vesta.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Vesta.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _senderUserEmail;
        private readonly string _senderUserPassword;
        private readonly string _senderHostAddress;
        private readonly int _senderHostPort;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly List<SentCodeRecord> SentCodesList;

        public EmailService(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _senderUserEmail = _configuration["Vesta:SenderUserEmail"];
            _senderUserPassword = _configuration["Vesta:SenderUserPassword"];
            _senderHostAddress = _configuration["Vesta:SenderHostAddress"];
            _senderHostPort = _configuration.GetValue<int>("Vesta:SenderHostPort");
            _hostEnvironment = hostEnvironment;
            SentCodesList = new List<SentCodeRecord>{};
        }

        private async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Vesta",_senderUserEmail));
                email.To.Add(new MailboxAddress("",recipientEmail));
                email.Subject = subject;
                email.Body = new TextPart("html") { Text = message };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_senderHostAddress, _senderHostPort, true);
                    await client.AuthenticateAsync(_senderUserEmail, _senderUserPassword);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
            }
            catch
            {
                throw new Exception("An Unknown error occurred while sending a verification code to your email");
            }
        }
        private async Task SendEmailVerificationCode(string recipientEmail, string subject, string code)
        {
            string relativePath = "Templates/EmailVerificationCode.html";
            string basePath = _hostEnvironment.ContentRootPath;
            string fullPath = Path.Combine(basePath, relativePath);

            string htmlString = File.ReadAllText(fullPath);
            htmlString = htmlString.Replace("{{}}", code);
            await SendEmailAsync(recipientEmail,subject,htmlString);
        }
        public async Task SendRandomEmailVerificationCode(string recipientEmail)
        {
            if(HasSentCodeOlderThanMinutes(recipientEmail,minutesPeriod:5))
                return;
                
            string generatedCode = CustomCodeGenerator.GenerateSimpleCode(Length:6);

            var sentCodeRecord = new SentCodeRecord()
            {
                Email = recipientEmail,
                Code = generatedCode,
                CreationDate = DateTime.Now
            };

            SentCodesList.Add(sentCodeRecord);

            await SendEmailVerificationCode(recipientEmail,"Confirm Your Email Address!",generatedCode);
        }
        public bool VerifyEmailCode(string recipientEmail, string code)
        {
            if(HasSentCodeOlderThanMinutes(recipientEmail,minutesPeriod:5))
                return false;
            
            var record = SentCodesList.Find(r => r.Email == recipientEmail);

            if(record == null || record.Code != code)
                return false;

            SentCodesList.Remove(record);
            return true;
        }

        private bool HasSentCodeOlderThanMinutes(string recipientEmail,int minutesPeriod = 5)
        {
            var record = SentCodesList.Find(r => r.Email == recipientEmail);

            if(record == null)
                return false;

            bool isOld = DateTime.Now.Subtract(record.CreationDate).TotalMinutes >=  minutesPeriod;

                if(isOld)
                    SentCodesList.Remove(record);

            return isOld;
        }
        private class SentCodeRecord
        {
            [Required]
            public string Email {get; set;} = null!;
            
            [Required]
            public string Code {get; set;} = null!;

            [Required]
            public DateTime CreationDate {get; set;}
        }
    }
}
