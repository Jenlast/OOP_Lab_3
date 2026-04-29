using OOP_Lab3.Core;
using System.Windows.Input;
using System;
using Avalonia.Threading;

namespace OOP_Lab3.ViewModels
{
    public class Task2ViewModel : ViewModelBase
    {
        private readonly IEvasionStrategy _evasionStrategy;
        private DispatcherTimer _timer;
        
        private double _startX;
        private double _startY;

        private double _buttonX;
        private double _buttonY;
        private string _questionText = "Ви поставите нам 20 балів за цю лабу?";
        
        private string _movingButtonText = "Ні";
        private string _movingButtonColor = "DarkRed";
        private bool _isEvasive = true; 
        private bool _isTimerStarted = false;

        private int _timeLeft = 10;
        private string _timerText = ""; 
        
        // Нова властивість для керування видимістю кнопок
        private bool _areButtonsVisible = true;

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

        public string MovingButtonText
        {
            get => _movingButtonText;
            set { _movingButtonText = value; OnPropertyChanged(); }
        }

        public string MovingButtonColor
        {
            get => _movingButtonColor;
            set { _movingButtonColor = value; OnPropertyChanged(); }
        }

        public string TimerText
        {
            get => _timerText;
            set { _timerText = value; OnPropertyChanged(); }
        }

        // Властивість для Binding в XAML
        public bool AreButtonsVisible
        {
            get => _areButtonsVisible;
            set { _areButtonsVisible = value; OnPropertyChanged(); }
        }

        public ICommand SayYesCommand { get; }
        public ICommand MovingButtonCommand { get; }
        public ICommand ResetCommand { get; } 

        public Task2ViewModel(IEvasionStrategy evasionStrategy)
        {
            _evasionStrategy = evasionStrategy;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += OnTimerTick;

            SayYesCommand = new RelayCommand(ExecuteYesAction);
            ResetCommand = new RelayCommand(ResetState);

            MovingButtonCommand = new RelayCommand(() => 
            {
                if (!_isEvasive)
                {
                    ExecuteYesAction();
                }
                else
                {
                    _timer.Stop();
                    _isEvasive = false;
                    TimerText = ""; 
                    QuestionText = "А може все-таки поставите?";
                    MovingButtonText = "Так";
                    MovingButtonColor = "#333333";
                    ButtonX = _startX; 
                    ButtonY = _startY;
                }
            });
        }

        private void ResetState()
        {
            _timer.Stop();
            _isTimerStarted = false;
            _isEvasive = true;
            _timeLeft = 10;

            QuestionText = "Ви поставите нам 20 балів за цю лабу?";
            MovingButtonText = "Ні";
            MovingButtonColor = "DarkRed";
            TimerText = "";
            
            // Повертаємо видимість кнопкам
            AreButtonsVisible = true;

            ButtonX = _startX;
            ButtonY = _startY;
        }

        public void SetStartPosition(double x, double y)
        {
            _startX = x;
            _startY = y;

            if (!_isTimerStarted || !_isEvasive)
            {
                ButtonX = x;
                ButtonY = y;
            }
        }

        private void ExecuteYesAction()
        {
            QuestionText = "Дякуємо за 20 балів! Ви молодець!";
            _timer.Stop(); 
            TimerText = ""; 
            
            // Приховуємо кнопки після згоди
            AreButtonsVisible = false;
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _timeLeft--; 

            if (_timeLeft > 0)
            {
                TimerText = $"Кнопка здасться через: {_timeLeft} сек";
            }
            else
            {
                _timer.Stop();
                TimerText = ""; 
                _isEvasive = false; 
                
                ButtonX = _startX;
                ButtonY = _startY;
                
                MovingButtonText = "Так";
                MovingButtonColor = "#333333";
            }
        }

        public void HandlePointerMoved(double mouseX, double mouseY, double areaWidth, double areaHeight)
        {
            if (areaWidth <= 0 || areaHeight <= 0) return;
            if (!_isEvasive) return;

            var (newX, newY) = _evasionStrategy.CalculateNewPosition(
                mouseX, mouseY, ButtonX, ButtonY, 100, 40, areaWidth, areaHeight);

            if (newX != ButtonX || newY != ButtonY)
            {
                if (!_isTimerStarted)
                {
                    _isTimerStarted = true;
                    _timeLeft = 10; 
                    TimerText = $"Кнопка здасться через: {_timeLeft} сек"; 
                    _timer.Start();
                }

                ButtonX = newX;
                ButtonY = newY;
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _execute();
    }
}