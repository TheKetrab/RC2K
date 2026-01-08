using MailKit.Security;
using MimeKit;
using RC2K.Logic.Interfaces;
using MailKit.Net.Smtp;

namespace RC2K.Logic;

public class GmailProvider : IMailProvider
{
    private const string SenderName = "RC2K Hub admin";
    private readonly string _senderEmail = "kettydun@gmail.com";
    private readonly string _sftpAppPassword;

    public GmailProvider(string senderEmail, string sftpAppPassword)
    {
        _senderEmail = senderEmail;
        _sftpAppPassword = sftpAppPassword;
    }

    public void SendMail(string recipientName, string recipientEmail, string subject, string content)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(SenderName, _senderEmail));
        message.To.Add(new MailboxAddress(recipientName, recipientEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = content
        };

        SendViaGmail(message);
    }

    private void SendViaGmail(MimeMessage message)
    {
        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        client.Authenticate(_senderEmail, _sftpAppPassword);

        client.Send(message);
        client.Disconnect(true);
    }
}