using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OOP_Lab3.ViewModels; // Обов'язково
using OOP_Lab3.Views;      // Обов'язково

namespace OOP_Lab3 // Без підкреслення, щоб збігалося з іншими!
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(), // Ось це запускає логіку!
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}