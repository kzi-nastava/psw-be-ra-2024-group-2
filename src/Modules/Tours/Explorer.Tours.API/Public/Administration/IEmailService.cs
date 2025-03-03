using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public.Administration;

public interface IEmailService
{
    void SendEmail(string recipientEmail, string subject, string body);

    // Method to send a report email
    Task SendReportEmail(string emailAddress, string reportContent);

    // Method to send tour reminder emails
    void SendTourReminderEmails();

    public void SendTestEmail();

}
