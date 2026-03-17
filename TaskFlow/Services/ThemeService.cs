 codex/generate-windows-to-do-list-application-8ppoi3
using System.Collections;
using System.Windows.Media;


 main
namespace TaskFlow.Services;

public class ThemeService
{
    public bool IsDarkTheme { get; private set; }

 codex/generate-windows-to-do-list-application-8ppoi3
    public void ToggleTheme(IDictionary resources)

    public void ToggleTheme(ResourceDictionary resources)
 main
    {
        IsDarkTheme = !IsDarkTheme;

        if (IsDarkTheme)
        {
            resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(24, 24, 27));
            resources["SurfaceBrush"] = new SolidColorBrush(Color.FromRgb(39, 39, 42));
            resources["TextBrush"] = new SolidColorBrush(Color.FromRgb(244, 244, 245));
            resources["MutedBrush"] = new SolidColorBrush(Color.FromRgb(161, 161, 170));
        }
        else
        {
            resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(246, 246, 247));
            resources["SurfaceBrush"] = new SolidColorBrush(Colors.White);
            resources["TextBrush"] = new SolidColorBrush(Color.FromRgb(24, 24, 27));
            resources["MutedBrush"] = new SolidColorBrush(Color.FromRgb(113, 113, 122));
        }
    }
}
