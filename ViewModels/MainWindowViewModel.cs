using OOP_Lab3.Core;

namespace OOP_Lab3.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            _currentView = new Task1ViewModel();
        }

        public void OpenTask1() => CurrentView = new Task1ViewModel();
        
        public void OpenTask2() 
        {
            // ОСЬ ТУТ: Просто передаємо нову фізичну стратегію!
            CurrentView = new Task2ViewModel(new PhysicsEvasionStrategy());
        }
    }
}