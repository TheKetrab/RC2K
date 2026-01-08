
namespace RC2K.Logic.Interfaces;

public interface IMailProvider
{
    void SendMail(string recipientName, string recipientEmail, string subject, string content);
}
