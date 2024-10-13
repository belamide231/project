using MailKit.Net.Smtp;
using MimeKit;


public class MailKitHelper {


    private static MailboxAddress? _mailbox;

    public MailKitHelper() {
        _mailbox = new MailboxAddress("PORTAL", DotEnvHelper.GmailUsername);
    }


    public static async Task F_MailVerificationCode(string recepient, string verificationCode) {

        var email = new MimeMessage();
        
        email.From.Add(_mailbox);
        email.To.Add(new MailboxAddress("", recepient));
        email.Subject = "Your Verification Code";
        email.Body = new TextPart("plain") {
            Text = $"Your verification code is: {verificationCode}\nThis will expire after {DateTime.UtcNow.AddHours(-4).AddMinutes(5).Add(TimeSpan.FromHours(12))}"
        };

        using (var smtp = new SmtpClient()) {

            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(DotEnvHelper.GmailUsername, DotEnvHelper.GmailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
