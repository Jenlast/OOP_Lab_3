using OOP_Lab3.Core; // Щоб бачило BasicEvasionStrategy

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
            CurrentView = new Task2ViewModel(new BasicEvasionStrategy());
        }
    }
}