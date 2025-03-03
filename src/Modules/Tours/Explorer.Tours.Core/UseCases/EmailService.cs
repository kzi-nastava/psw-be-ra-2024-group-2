using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;
    private readonly ITourRepository _tourRepository;

    public EmailService(IConfiguration configuration, ITourRepository tourRepository)
    {
        _smtpServer = configuration["EmailSettings:SmtpServer"];
        _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
        _smtpUser = configuration["EmailSettings:SmtpUser"];
        _smtpPassword = configuration["EmailSettings:SmtpPassword"];
        _senderEmail = configuration["EmailSettings:SenderEmail"];
        _tourRepository = tourRepository; // Inject the tour repository
    }

    // Method to send emails
    public void SendEmail(string recipientEmail, string subject, string body)
    {
        try
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(recipientEmail);

                client.Send(mailMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    // Send reminders for upcoming tours
    public void SendTourReminderEmails()
    {
        var reminderTours = _tourRepository.GetReminderTours(); // Fetch tours happening in the next 48 hours

        foreach (var tour in reminderTours)
        {
            var purchaseEmails = _tourRepository.GetTourReminderEmails(tour.Id); // Get emails for this tour

            foreach (var email in purchaseEmails)
            {
                var emailBody = GenerateTourReminderEmailBody(tour); // Generate email content
                SendEmail(email, "Reminder: Upcoming Tour", emailBody); // Use the SendEmail method
            }
        }
    }

    // Generate the body of the reminder email
    private string GenerateTourReminderEmailBody(Tour tour)
    {
        return $@"
        <h2>{tour.Name} is happening soon!</h2>
        <p>This is a friendly reminder that your tour is coming up on {tour.Date.ToString("f")}. We look forward to seeing you!</p>
        <p>Best regards,</p>
        <p>The Tour Team</p>
        ";
    }

    public void SendTestEmail()
    {
        var tour = _tourRepository.GetTourByID(1); // Fetch tours happening in the next 48 hours
        var email = "sergej.vlaskalic@gmail.com";
        var emailBody = GenerateTourReminderEmailBody(tour); // Generate email content
        this.SendEmail(email, "Reminder: Upcoming Tour", emailBody);
    }

    


    // Send monthly report email
    public async Task SendReportEmail(string emailAddress, string reportContent)
    {
        var subject = "Monthly Tour Report";

        var body = $@"
        <h1>Tour Report for {DateTime.Now.ToString("MMMM yyyy")}</h1>
        <p>{reportContent}</p>
        ";

        try
        {
            // Call the SendEmail method to send the email
            SendEmail(emailAddress, subject, body);
            Console.WriteLine($"Report email sent to {emailAddress}");
        }
        catch (Exception ex)
        {
            // Log any exception that occurs while sending the email
            Console.WriteLine($"Error sending email to {emailAddress}: {ex.Message}");
        }
    }
}
