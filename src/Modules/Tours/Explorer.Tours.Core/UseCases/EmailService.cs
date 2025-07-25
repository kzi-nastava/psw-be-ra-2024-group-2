using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;
    private readonly ITourRepository _tourRepository;
    private readonly HttpClient _httpClient;


    public EmailService(IConfiguration configuration, ITourRepository tourRepository, HttpClient httpClient)
    {
        _smtpServer = configuration["EmailSettings:SmtpServer"];
        _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
        _smtpUser = configuration["EmailSettings:SmtpUser"];
        _smtpPassword = configuration["EmailSettings:SmtpPassword"];
        _senderEmail = configuration["EmailSettings:SenderEmail"];
        _tourRepository = tourRepository; // Inject the tour repository
        _httpClient = httpClient;
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

    private string GenerateTourRecommendationEmailBody(TourCreateDto tourDto, string guideUsername, string touristFullName)
    {
        return $@"
    <h2>Dear {touristFullName},</h2>
    <h2>Recommended Tour: {tourDto.Name}</h2>
    <p>We have a tour that we think you might like!</p>
    <p><strong>Tour Name:</strong> {tourDto.Name}</p>
    <p><strong>Date:</strong> {tourDto.Date.ToString("f")}</p>
    <p><strong>Category:</strong> {string.Join(", ", tourDto.Category)}</p>
    <p><strong>Guide:</strong> {guideUsername}</p>
    <p>Don't miss out on this experience!</p>
    <p>Best regards,</p>
    <p>The Tour Team</p>
    ";
    }


    // tourist data is type "email*name*surname"
    public void SendTourRecommendationEmails(List<string> touristsData, string guideUsername, TourCreateDto tourDto)
    {

        foreach (var touristData in touristsData)
        {
            var dataParts = touristData.Split('*');

            // Extract email, name, and surname
            var email = dataParts[0];
            var name = dataParts[1];
            var surname = dataParts[2];

            // Construct full name
            var touristFullName = $"{name} {surname}";
            var emailBody = GenerateTourRecommendationEmailBody(tourDto, guideUsername, touristFullName);

            this.SendEmail(email, "Recommended Tour for You!", emailBody);
        }
    }

    public async Task<List<string>> GetUserEmailsByInterestsAsync(string interests)
    {
        if (string.IsNullOrEmpty(interests))
        {
            throw new ArgumentException("Interests cannot be null or empty", nameof(interests));
        }

        // Define the URL of your API endpoint, including the interests as a query parameter
        string apiUrl = $"http://localhost:5000/api/users/getUsersWithInterests?interests={Uri.EscapeDataString(interests)}";

        // Make the async GET request
        var response = await _httpClient.GetAsync(apiUrl);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content asynchronously and parse it into a list of strings
            var result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
            return result; // Return the list of emails
        }
        else
        {
            // Handle the error case (could be logging or throwing an exception)
            throw new Exception("Failed to fetch user emails.");
        }
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
