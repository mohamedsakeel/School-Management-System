
namespace School_Management_System.BackgroundTasks
{
    public interface IBackgroundTasks
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}