using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OOP_Lab3.ViewModels
{
    // Модель для нашої згенерованої кнопки
    public partial class NumberItem : ObservableObject
    {
        [ObservableProperty]
        private int _value;

        // НОВЕ: Лічильник натискань для конкретної кнопки
        [ObservableProperty]
        private int _clickCount = 0;

        public NumberItem(int value)
        {
            Value = value;
        }
    }

    public partial class Task1ViewModel : ViewModelBase
    {
        [ObservableProperty] private int _startValue = 1;
        [ObservableProperty] private int _endValue = 20;
        [ObservableProperty] private int _stepValue = 1;
        [ObservableProperty] private int _multipleValue = 2;
        [ObservableProperty] private string _errorMessage = string.Empty;

        [ObservableProperty] 
        private string _numberInfo = "Натисніть на будь-яке число, щоб побачити інформацію.";

        public ObservableCollection<NumberItem> GeneratedNumbers { get; } = new ObservableCollection<NumberItem>();

        [RelayCommand]
        private void GenerateButtons()
        {
            ErrorMessage = string.Empty;
            
            // ПРИБРАНО: GeneratedNumbers.Clear(); - Тепер нові кнопки дописуються до старих!
            
            if (StepValue == 0)
            {
                ErrorMessage = "Помилка: Крок не може бути 0!";
                return;
            }

            if ((StartValue < EndValue && StepValue < 0) || (StartValue > EndValue && StepValue > 0))
            {
                ErrorMessage = "Помилка: Нескінченний цикл (неправильний крок для цього діапазону).";
                return;
            }

            if (StartValue <= EndValue)
            {
                for (int i = StartValue; i <= EndValue; i += StepValue)
                    GeneratedNumbers.Add(new NumberItem(i));
            }
            else
            {
                for (int i = StartValue; i >= EndValue; i += StepValue)
                    GeneratedNumbers.Add(new NumberItem(i));
            }
        }

        [RelayCommand]
        private void DeleteMultiples()
        {
            ErrorMessage = string.Empty;

            if (MultipleValue == 0)
            {
                ErrorMessage = "Помилка: Не можна ділити на 0 (кратність 0).";
                return;
            }

            var itemsToRemove = GeneratedNumbers.Where(x => x.Value % MultipleValue == 0).ToList();
            foreach (var item in itemsToRemove)
            {
                GeneratedNumbers.Remove(item);
            }
        }

        // НОВЕ: Тепер ми приймаємо весь об'єкт NumberItem, а не просто число
        [RelayCommand]
        private void ShowNumberInfo(NumberItem item)
        {
            if (item == null) return;

            // Збільшуємо лічильник кліків для цієї конкретної кнопки
            item.ClickCount++;

            int number = item.Value;

            // Математика: визначаємо знак
            string sign = number > 0 ? "Додатнє" : (number < 0 ? "Від'ємне" : "Нуль");
            
            // Математика: визначаємо простоту
            string primeType = string.Empty;
            if (number <= 1) primeType = "Не є ні простим, ні складним"; 
            else
            {
                bool isPrime = true;
                for (int i = 2; i <= Math.Sqrt(number); i++)
                {
                    if (number % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                primeType = isPrime ? "Просте" : "Складне";
            }

            // Формуємо базову відповідь
            string mathAnswer = $"Число: {number}\nЗнак: {sign}\nТип: {primeType}";

            // ПЕРЕВІРКА НА "ПАСХАЛКУ" (якщо натиснули 4 або більше разів)
            if (item.ClickCount >= 4)
            {
                NumberInfo = $"😠 ДОСИТЬ ВЖЕ НАТИСКАТИ!\nТи натиснув на цю кнопку {item.ClickCount} разів!\nОсь твоя відповідь, тільки відчепися:\n\n{mathAnswer}";
            }
            else
            {
                NumberInfo = mathAnswer;
            }
        }
        
        // БОНУС: Кнопка, щоб все ж таки мати можливість очистити екран
        [RelayCommand]
        private void ClearAll()
        {
            GeneratedNumbers.Clear();
            NumberInfo = "Екран очищено.";
            ErrorMessage = string.Empty;
        }
    }
}