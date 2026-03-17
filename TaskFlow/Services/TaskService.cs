using Microsoft.EntityFrameworkCore;
using TaskFlow.Database;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class TaskService
{
    private readonly TaskFlowDbContext _dbContext;

    public TaskService(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TaskItem>> GetTasksAsync(string? search = null, int? projectId = null)
    {
        var query = _dbContext.Tasks
            .Include(t => t.Tags)
            .AsQueryable();

        if (projectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == projectId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));
        }

        return await query.OrderBy(t => t.DueDate).ThenByDescending(t => t.Priority).ToListAsync();
    }

    public async Task<TaskItem> AddTaskAsync(TaskItem taskItem)
    {
        _dbContext.Tasks.Add(taskItem);
        await _dbContext.SaveChangesAsync();
        return taskItem;
    }

    public async Task UpdateTaskAsync(TaskItem taskItem)
    {
        _dbContext.Tasks.Update(taskItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(TaskItem taskItem)
    {
        _dbContext.Tasks.Remove(taskItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ToggleCompleteAsync(TaskItem taskItem)
    {
        taskItem.Status = taskItem.Status == TaskStatus.Completed ? TaskStatus.InProgress : TaskStatus.Completed;
        await UpdateTaskAsync(taskItem);
    }
}
