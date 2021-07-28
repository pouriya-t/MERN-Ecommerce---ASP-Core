namespace Infrastructure.EmailService
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int PortResetPassword { get; set; }
        public int PortConfirmationEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
