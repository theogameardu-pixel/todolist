using TaskFlow.Database;

namespace TaskFlow.Services;

public class DatabaseService
{
    private readonly TaskFlowDbContext _dbContext;

    public DatabaseService(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
    }
}
