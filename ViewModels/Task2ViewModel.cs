using OOP_Lab3.Core;
using System.Windows.Input; // Потрібно для команд
using System;

namespace OOP_Lab3.ViewModels
{
    public class Task2ViewModel : ViewModelBase
    {
        private readonly IEvasionStrategy _evasionStrategy;
        
        private double _buttonX = 450; // Початкова позиція "Ні" (ближче до центру)
        private double _buttonY = 280; 
        private string _questionText = "Ви поставите нам 20 балів за цю лабу?";

        public double ButtonX
        {
            get => _buttonX;
            set { _buttonX = value; OnPropertyChanged(); }
        }

        public double ButtonY
        {
            get => _buttonY;
            set { _buttonY = value; OnPropertyChanged(); }
        }

        public string QuestionText
        {
            get => _questionText;
            set { _questionText = value; OnPropertyChanged(); }
        }

        // Команда для кнопки "Так"
        public ICommand SayYesCommand { get; }

        public Task2ViewModel(IEvasionStrategy evasionStrategy)
        {
            _evasionStrategy = evasionStrategy;
            
            // Створюємо найпростішу команду для кнопки "Так"
            SayYesCommand = new RelayCommand(() => 
            {
                QuestionText = "Дякуємо за 20 балів! Ви найкращі!";
            });
        }

        public void HandlePointerMoved(double mouseX, double mouseY, double areaWidth, double areaHeight)
        {
            // Якщо вікно ще не завантажилось (розміри = 0), нічого не робимо
            if (areaWidth <= 0 || areaHeight <= 0) return;

            var (newX, newY) = _evasionStrategy.CalculateNewPosition(
                mouseX, mouseY, 
                ButtonX, ButtonY, 
                100, 40, // Ширина і висота кнопки "Ні"
                areaWidth, areaHeight);

            ButtonX = newX;
            ButtonY = newY;
        }
    }

    // Проста реалізація ICommand для ViewModel
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _execute();
    }
}