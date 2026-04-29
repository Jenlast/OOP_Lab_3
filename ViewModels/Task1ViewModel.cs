using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OOP_Lab3.ViewModels
{
    public partial class NumberItem : ObservableObject
    {
        [ObservableProperty]
        private int _value;

        [ObservableProperty]
        private int _clickCount = 0;

        public NumberItem(int value)
        {
            Value = value;
        }
    }

    public partial class Task1ViewModel : ViewModelBase
    {
        // НОВЕ: Константа ліміту кнопок
        private const int MaxButtonsLimit = 20000;

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
            
            // Перевірка: чи не досягнуто ліміт ще ДО початку генерації
            if (GeneratedNumbers.Count >= MaxButtonsLimit)
            {
                ErrorMessage = $"Помилка: Досягнуто максимальний ліміт кнопок ({MaxButtonsLimit}). Очистіть екран.";
                return;
            }

            if (StepValue == 0)
            {
                ErrorMessage = "Помилка: Крок не може бути 0!";
                return;
            }

            if (StartValue == EndValue)
            {
                ErrorMessage = "Помилка: Початкове і кінцеве значення не можуть бути однаковими.";
                return;
            }

            if ((StartValue < EndValue && StepValue < 0) || (StartValue > EndValue && StepValue > 0))
            {
                ErrorMessage = "Помилка: Нескінченний цикл (неправильний крок для цього діапазону).";
                return;
            }

            bool limitReached = false;
            int currentCount = GeneratedNumbers.Count; // Кешуємо поточну кількість для швидкодії

            if (StartValue <= EndValue)
            {
                for (int i = StartValue; i <= EndValue; i += StepValue)
                {
                    // НОВЕ: Перевірка ліміту під час додавання
                    if (currentCount >= MaxButtonsLimit)
                    {
                        limitReached = true;
                        break; // Зупиняємо цикл
                    }
                    GeneratedNumbers.Add(new NumberItem(i));
                    currentCount++;
                }
            }
            else
            {
                for (int i = StartValue; i >= EndValue; i += StepValue)
                {
                    // НОВЕ: Перевірка ліміту під час додавання
                    if (currentCount >= MaxButtonsLimit)
                    {
                        limitReached = true;
                        break; // Зупиняємо цикл
                    }
                    GeneratedNumbers.Add(new NumberItem(i));
                    currentCount++;
                }
            }

            // Якщо ми зупинилися через ліміт, повідомляємо про це
            if (limitReached)
            {
                ErrorMessage = $"Увага: Згенеровано лише частину чисел. Досягнуто ліміт у {MaxButtonsLimit} кнопок!";
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

        [RelayCommand]
        private void ShowNumberInfo(NumberItem item)
        {
            if (item == null) return;

            item.ClickCount++;
            int number = item.Value;

            string sign = number > 0 ? "Додатнє" : (number < 0 ? "Від'ємне" : "Нуль");
            
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

            string mathAnswer = $"Число: {number}\nЗнак: {sign}\nТип: {primeType}";

            if (item.ClickCount >= 4)
            {
                NumberInfo = $"😠 ДОСИТЬ ВЖЕ НАТИСКАТИ!\nТи натиснув на цю кнопку {item.ClickCount} разів!\nОсь твоя відповідь, тільки відчепися:\n\n{mathAnswer}";
            }
            else
            {
                NumberInfo = mathAnswer;
            }
        }
        
        [RelayCommand]
        private void ClearAll()
        {
            GeneratedNumbers.Clear();
            NumberInfo = "Екран очищено.";
            ErrorMessage = string.Empty;
        }
    }
}