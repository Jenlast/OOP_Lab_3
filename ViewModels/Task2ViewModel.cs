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
        
        // Початкові координати тепер не жорсткі, а задаються динамічно з UI
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
        private string _timerText = ""; // Порожній текст, поки таймер не запущено

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

        // Властивість, яку бачить XAML
        public string TimerText
        {
            get => _timerText;
            set { _timerText = value; OnPropertyChanged(); }
        }

        public ICommand SayYesCommand { get; }
        public ICommand MovingButtonCommand { get; }

        public Task2ViewModel(IEvasionStrategy evasionStrategy)
        {
            _evasionStrategy = evasionStrategy;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += OnTimerTick;

            SayYesCommand = new RelayCommand(ExecuteYesAction);

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
                    TimerText = ""; // Приховуємо таймер
                    QuestionText = "А може все-таки поставите?";
                    MovingButtonText = "Так";
                    MovingButtonColor = "#333333";
                    ButtonX = _startX; // Повертаємо на початкову позицію
                    ButtonY = _startY;
                }
            });
        }

        // Цей метод викликається з UI, щоб повідомити ідеально рівні координати
        public void SetStartPosition(double x, double y)
        {
            _startX = x;
            _startY = y;

            // Якщо ми ще не почали гратися або вже закінчили - оновлюємо позицію кнопки
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
            TimerText = ""; // Приховуємо таймер, якщо відповіли "Так"
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _timeLeft--; // Віднімаємо 1 секунду

            if (_timeLeft > 0)
            {
                // Оновлюємо текст на екрані
                TimerText = $"Кнопка здасться через: {_timeLeft} сек";
            }
            else
            {
                // Час вийшов!
                _timer.Stop();
                TimerText = ""; // Ховаємо таймер
                
                _isEvasive = false; 
                
                // Повертаємо рівно на місце "DummyPanel"
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
                    _timeLeft = 10; // Скидаємо час на 10
                    TimerText = $"Кнопка здасться через: {_timeLeft} сек"; // Показуємо відразу 10
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