using System;
using System.Threading;
using System.Threading.Tasks;
using Explorer.Tours.API.Public.Administration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class EmailReminderService : IHostedService
{
    private Timer _dailyTimer;
    private Timer _monthlyReportTimer;
    private Timer _testTimer;  // New timer for the test function
    private readonly IServiceScopeFactory _scopeFactory;

    public EmailReminderService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Schedule daily reminder emails
        ScheduleDailyEmail();

        // Schedule monthly report emails
        ScheduleMonthlyReport();

        //ScheduleTestFunction();

        return Task.CompletedTask;
    }

    private void ScheduleTestFunction()
    {
        // Schedule the test function to run every 20 minutes
        _testTimer = new Timer(SendMonthlyReport, null, TimeSpan.Zero, TimeSpan.FromMinutes(20));
    }

    private void ScheduleDailyEmail()
    {
        var currentTime = DateTime.Now;
        var nextRunTime = DateTime.Today.AddDays(1).AddHours(1); // 1 AM tomorrow
        var initialDelay = nextRunTime - currentTime;

        _dailyTimer = new Timer(SendReminderEmails, null, initialDelay, TimeSpan.FromDays(1));
    }

    private void ScheduleMonthlyReport()
    {
        var currentTime = DateTime.Now;
        var lastDayOfMonth = new DateTime(currentTime.Year, currentTime.Month, DateTime.DaysInMonth(currentTime.Year, currentTime.Month), 23, 0, 0);

        // If today is already the last day, schedule for next month
        if (currentTime > lastDayOfMonth)
        {
            lastDayOfMonth = lastDayOfMonth.AddMonths(1);
        }

        var initialDelay = lastDayOfMonth - currentTime;

        _monthlyReportTimer = new Timer(SendMonthlyReport, null, initialDelay, TimeSpan.FromDays(30)); // Approximate monthly interval
    }

    private void SendReminderEmails(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            emailService.SendTourReminderEmails();
        }
    }

    private void SendMonthlyReport(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var tourService = scope.ServiceProvider.GetRequiredService<ITourService>();

            var guidIDs = tourService.GetAllGuidesWithTourInLastMonth(); // Assuming a method to get all guides


            long bestGuideOfTheMonthID = -1;
            int mostSales = -1;
            foreach (var guideId in guidIDs)
            {
                var report = tourService.GetMonthlyGuideTourReport(guideId, DateTime.Now.Year, DateTime.Now.Month);

                if(report.totalSales > mostSales)
                {
                    mostSales = report.totalSales;
                    bestGuideOfTheMonthID = guideId;
                }

                var guideEmail = "sergej.vlaskalic@gmail.com";
                var reportString = report.ConvertGuideReportToHtml();
                emailService.SendReportEmail(guideEmail, reportString); // Assuming a method to send the email
            }

            Console.WriteLine("Best guideid: " + bestGuideOfTheMonthID);
            Console.WriteLine("Most sales: " + mostSales);
        }
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _dailyTimer?.Change(Timeout.Infinite, 0);
        _monthlyReportTimer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}
