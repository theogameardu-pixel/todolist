namespace TaskFlow.Models
{

    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.InProgress;

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int? ParentTaskId { get; set; }
        public TaskItem? ParentTask { get; set; }
        public ICollection<TaskItem> SubTasks { get; set; } = new List<TaskItem>();

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public bool IsCompleted => Status == TaskStatus.Completed;
    }
}
