using Hangfire;
using SchoolManagementSystem.Notification;

namespace School_Management_System.BackgroundTasks
{
    public class BackgroundTasks : IBackgroundTasks
    {
        private readonly IEmailSender _sender;

        public BackgroundTasks(IEmailSender sender)
        {
            _sender = sender;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Queue the email sending job to Hangfire
            BackgroundJob.Enqueue(() => _sender.SendEmailAsync(email, subject, message));
            return Task.CompletedTask;
        }
    }
}
