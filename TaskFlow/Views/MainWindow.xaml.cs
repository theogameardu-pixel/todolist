using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow.Views;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
    }

    private void TasksList_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        if (sender is ListBox listBox && listBox.SelectedItem is TaskItem task)
        {
            DragDrop.DoDragDrop(listBox, task, DragDropEffects.Move);
        }
    }

    private void TasksList_Drop(object sender, DragEventArgs e)
    {
        if (DataContext is not MainViewModel vm || e.Data.GetData(typeof(TaskItem)) is not TaskItem droppedTask)
        {
            return;
        }

        // Réorganisation locale de la liste pour simuler le drag & drop de priorisation.
        var collection = vm.Tasks;
        var target = ((FrameworkElement)e.OriginalSource).DataContext as TaskItem;
        if (target is null || droppedTask == target)
        {
            return;
        }

        var oldIndex = collection.IndexOf(droppedTask);
        var newIndex = collection.IndexOf(target);
        if (oldIndex >= 0 && newIndex >= 0)
        {
            collection.Move(oldIndex, newIndex);
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not MainViewModel vm)
        {
            return;
        }

        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.N)
        {
            vm.AddTaskCommand.Execute(null);
        }
        else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
        {
            vm.ToggleThemeCommand.Execute(null);
        }
        else if (e.Key == Key.Delete)
        {
            vm.DeleteTaskCommand.Execute(vm.SelectedTask);
        }
    }
}
