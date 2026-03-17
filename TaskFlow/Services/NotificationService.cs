using Microsoft.Toolkit.Uwp.Notifications;
using TaskFlow.Models;

namespace TaskFlow.Services
{

    public class NotificationService
    {
        public void NotifyDue(TaskItem task)
        {
            if (task.DueDate is null)
            {
                return;
            }

            // Notification Windows native via toast.
            new ToastContentBuilder()
                .AddText("TaskFlow - Échéance")
                .AddText($"La tâche '{task.Title}' arrive à échéance le {task.DueDate:dd/MM/yyyy HH:mm}")
                .Show();
        }
namespace TaskFlow.Services;

public class NotificationService
{
    public void NotifyDue(TaskItem task)
    {
        if (task.DueDate is null)
        {
            return;
        }

        // Notification Windows native via toast.
        new ToastContentBuilder()
            .AddText("TaskFlow - Échéance")
            .AddText($"La tâche '{task.Title}' arrive à échéance le {task.DueDate:dd/MM/yyyy HH:mm}")
            .Show();
    }
}
