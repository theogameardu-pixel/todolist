using Microsoft.EntityFrameworkCore;
using TaskFlow.Database;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class ProjectService
{
    private readonly TaskFlowDbContext _dbContext;

    public ProjectService(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        return await _dbContext.Projects.Include(p => p.Tasks).OrderBy(p => p.Name).ToListAsync();
    }

    public async Task<Project> AddProjectAsync(string name)
    {
        var project = new Project { Name = name };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }
}
