using Entity.Models;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Service.Helpers.EmailNotification
{
    public class SendEmailNotificationService
    {
        private readonly TransactionalEmailsApi _apiInstance;
        private readonly string _senderName;
        private readonly string _senderEmail;
        private readonly string _api_key;

        public SendEmailNotificationService(IConfiguration configuration)
        {
            var brevo = configuration.GetSection("Brevo");
            var brevoSettings = brevo.Get<Brevo>();

            Configuration.Default.ApiKey.Clear();
            _api_key = brevoSettings?.ApiKey?.Trim() ?? "";
            Configuration.Default.ApiKey.Add("api-key", _api_key);
            _apiInstance = new TransactionalEmailsApi();

            // Fix: use proper sender name
            _senderName = brevoSettings?.SenderName ?? "FitTracker";
            _senderEmail = "shubhamjangid236@gmail.com";
        }

        public bool SendEmail(string toEmail, string toName, string subject, string htmlContent)
        {
            // Fix: check that all required values are present
            if (string.IsNullOrWhiteSpace(_senderEmail) || string.IsNullOrWhiteSpace(_senderName) || string.IsNullOrWhiteSpace(_api_key))
            {
                return false;
            }

            try
            {
                var sender = new SendSmtpEmailSender(_senderName, _senderEmail);
                var smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);
                var to = new List<SendSmtpEmailTo> { smtpEmailTo };

                var sendSmtpEmail = new SendSmtpEmail(
                    sender,
                    to,
                    null,
                    null,
                    htmlContent,
                    null,
                    subject
                );



                var send = _apiInstance.SendTransacEmail(sendSmtpEmail);
                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
