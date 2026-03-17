using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow.ViewModels
{

    public enum TaskFilter
    {
        All,
        Today,
        Upcoming,
        Completed
    }

    public partial class MainViewModel : ObservableObject
    {
        private readonly TaskService _taskService;
        private readonly ProjectService _projectService;
        private readonly NotificationService _notificationService;
        private readonly ThemeService _themeService;

        [ObservableProperty] private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        [ObservableProperty] private ObservableCollection<TaskItem> _tasks = new ObservableCollection<TaskItem>();
        [ObservableProperty] private Project? _selectedProject;
        [ObservableProperty] private TaskItem? _selectedTask;
        [ObservableProperty] private string _newTaskTitle = string.Empty;
        [ObservableProperty] private string _searchText = string.Empty;
        [ObservableProperty] private TaskFilter _selectedFilter;

        public Array PriorityValues => Enum.GetValues(typeof(TaskPriority));
        public Array FilterValues => Enum.GetValues(typeof(TaskFilter));

        public double CompletionRate => Tasks.Count == 0
            ? 0
            : Tasks.Count(t => t.Status == TaskStatus.Completed) * 100d / Tasks.Count;

        public MainViewModel(TaskService taskService, ProjectService projectService, NotificationService notificationService, ThemeService themeService)
        {
            _taskService = taskService;
            _projectService = projectService;
            _notificationService = notificationService;
            _themeService = themeService;

            SelectedFilter = TaskFilter.All;
            _ = LoadAsync();
        }

        partial void OnSelectedProjectChanged(Project? value) => _ = LoadTasksAsync();
        partial void OnSearchTextChanged(string value) => _ = LoadTasksAsync();
        partial void OnSelectedFilterChanged(TaskFilter value) => _ = LoadTasksAsync();

        [RelayCommand]
        public async Task LoadAsync()
        {
            Projects = new ObservableCollection<Project>(await _projectService.GetProjectsAsync());

            if (Projects.Count > 0)
            {
                SelectedProject = Projects[0];
            }

            await LoadTasksAsync();
        }

        [RelayCommand]
        public async Task AddProjectAsync()
        {
            var name = $"Projet {Projects.Count + 1}";
            var project = await _projectService.AddProjectAsync(name);
            Projects.Add(project);
            SelectedProject = project;
        }

        [RelayCommand]
        public async Task AddTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle) || SelectedProject is null)
            {
                return;
            }

            var task = new TaskItem
            {
                Title = NewTaskTitle.Trim(),
                ProjectId = SelectedProject.Id,
                DueDate = DateTime.Now.AddDays(1),
                Priority = TaskPriority.Medium,
                Status = TaskStatus.InProgress
            };

            await _taskService.AddTaskAsync(task);
            NewTaskTitle = string.Empty;
            await LoadTasksAsync();
        }

        [RelayCommand]
        public async Task DeleteTaskAsync(TaskItem? task)
        {
            if (task is null)
            {
                return;
            }

            await _taskService.DeleteTaskAsync(task);
            await LoadTasksAsync();
        }

        [RelayCommand]
        public async Task ToggleTaskStatusAsync(TaskItem? task)
        {
            if (task is null)
            {
                return;
            }

            await _taskService.ToggleCompleteAsync(task);
            if (task.DueDate.HasValue && task.DueDate.Value.Date <= DateTime.Today.AddDays(1))
            {
                _notificationService.NotifyDue(task);
            }

            await LoadTasksAsync();
        }

        [RelayCommand]
        public async Task SaveTaskAsync(TaskItem? task)
        {
            if (task is null)
            {
                return;
            }

            await _taskService.UpdateTaskAsync(task);
            await LoadTasksAsync();
        }

        [RelayCommand]
        public void ToggleTheme()
        {
            _themeService.ToggleTheme(Application.Current.Resources);
        }

        [RelayCommand]
        public async Task MoveTaskToProjectAsync(Project destination)
        {
            if (SelectedTask is null)
            {
                return;
            }

            SelectedTask.ProjectId = destination.Id;
            await _taskService.UpdateTaskAsync(SelectedTask);
            await LoadTasksAsync();
        }

        private async Task LoadTasksAsync()
        {
            var fetchedTasks = await _taskService.GetTasksAsync(SearchText, SelectedProject?.Id);
            var filtered = ApplyFilter(fetchedTasks);

            Tasks = new ObservableCollection<TaskItem>(filtered);
            OnPropertyChanged(nameof(CompletionRate));
        }

        private IEnumerable<TaskItem> ApplyFilter(IEnumerable<TaskItem> tasks)
        {
            return SelectedFilter switch
            {
                TaskFilter.Today => tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == DateTime.Today),
                TaskFilter.Upcoming => tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date > DateTime.Today),
                TaskFilter.Completed => tasks.Where(t => t.Status == TaskStatus.Completed),
                _ => tasks
            };
        }
namespace TaskFlow.ViewModels;

public enum TaskFilter
{
    All,
    Today,
    Upcoming,
    Completed
}

public partial class MainViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly ProjectService _projectService;
    private readonly NotificationService _notificationService;
    private readonly ThemeService _themeService;

    [ObservableProperty] private ObservableCollection<Project> _projects = [];
    [ObservableProperty] private ObservableCollection<TaskItem> _tasks = [];
    [ObservableProperty] private Project? _selectedProject;
    [ObservableProperty] private TaskItem? _selectedTask;
    [ObservableProperty] private string _newTaskTitle = string.Empty;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private TaskFilter _selectedFilter;

    public Array PriorityValues => Enum.GetValues(typeof(TaskPriority));
    public Array FilterValues => Enum.GetValues(typeof(TaskFilter));

    public double CompletionRate => Tasks.Count == 0
        ? 0
        : Tasks.Count(t => t.Status == TaskStatus.Completed) * 100d / Tasks.Count;

    public MainViewModel(TaskService taskService, ProjectService projectService, NotificationService notificationService, ThemeService themeService)
    {
        _taskService = taskService;
        _projectService = projectService;
        _notificationService = notificationService;
        _themeService = themeService;

        SelectedFilter = TaskFilter.All;
        _ = LoadAsync();
    }

    partial void OnSelectedProjectChanged(Project? value) => _ = LoadTasksAsync();
    partial void OnSearchTextChanged(string value) => _ = LoadTasksAsync();
    partial void OnSelectedFilterChanged(TaskFilter value) => _ = LoadTasksAsync();

    [RelayCommand]
    public async Task LoadAsync()
    {
        Projects = new ObservableCollection<Project>(await _projectService.GetProjectsAsync());

        if (Projects.Count > 0)
        {
            SelectedProject = Projects[0];
        }

        await LoadTasksAsync();
    }

    [RelayCommand]
    public async Task AddProjectAsync()
    {
        var name = $"Projet {Projects.Count + 1}";
        var project = await _projectService.AddProjectAsync(name);
        Projects.Add(project);
        SelectedProject = project;
    }

    [RelayCommand]
    public async Task AddTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle) || SelectedProject is null)
        {
            return;
        }

        var task = new TaskItem
        {
            Title = NewTaskTitle.Trim(),
            ProjectId = SelectedProject.Id,
            DueDate = DateTime.Now.AddDays(1),
            Priority = TaskPriority.Medium,
            Status = TaskStatus.InProgress
        };

        await _taskService.AddTaskAsync(task);
        NewTaskTitle = string.Empty;
        await LoadTasksAsync();
    }

    [RelayCommand]
    public async Task DeleteTaskAsync(TaskItem? task)
    {
        if (task is null)
        {
            return;
        }

        await _taskService.DeleteTaskAsync(task);
        await LoadTasksAsync();
    }

    [RelayCommand]
    public async Task ToggleTaskStatusAsync(TaskItem? task)
    {
        if (task is null)
        {
            return;
        }

        await _taskService.ToggleCompleteAsync(task);
        if (task.DueDate.HasValue && task.DueDate.Value.Date <= DateTime.Today.AddDays(1))
        {
            _notificationService.NotifyDue(task);
        }

        await LoadTasksAsync();
    }

    [RelayCommand]
    public async Task SaveTaskAsync(TaskItem? task)
    {
        if (task is null)
        {
            return;
        }

        await _taskService.UpdateTaskAsync(task);
        await LoadTasksAsync();
    }

    [RelayCommand]
    public void ToggleTheme()
    {
        _themeService.ToggleTheme(Application.Current.Resources);
    }

    [RelayCommand]
    public async Task MoveTaskToProjectAsync(Project destination)
    {
        if (SelectedTask is null)
        {
            return;
        }

        SelectedTask.ProjectId = destination.Id;
        await _taskService.UpdateTaskAsync(SelectedTask);
        await LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        var fetchedTasks = await _taskService.GetTasksAsync(SearchText, SelectedProject?.Id);
        var filtered = ApplyFilter(fetchedTasks);

        Tasks = new ObservableCollection<TaskItem>(filtered);
        OnPropertyChanged(nameof(CompletionRate));
    }

    private IEnumerable<TaskItem> ApplyFilter(IEnumerable<TaskItem> tasks)
    {
        return SelectedFilter switch
        {
            TaskFilter.Today => tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == DateTime.Today),
            TaskFilter.Upcoming => tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date > DateTime.Today),
            TaskFilter.Completed => tasks.Where(t => t.Status == TaskStatus.Completed),
            _ => tasks
        };
    }
}
