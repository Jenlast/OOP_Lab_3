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

        public NumberItem(int value)
        {
            Value = value;
        }
    }

    // ЗМІНА ТУТ: Наслідуємось безпосередньо від ObservableObject
    public partial class Task1ViewModel : ViewModelBase
    {
        // Властивості для полів вводу
        [ObservableProperty] private int _startValue = 1;
        [ObservableProperty] private int _endValue = 20;
        [ObservableProperty] private int _stepValue = 1;
        
        [ObservableProperty] private int _multipleValue = 2;
        
        // Властивість для виводу помилок (Захист від дурня)
        [ObservableProperty] private string _errorMessage = string.Empty;

        // Колекція, до якої буде прив'язаний інтерфейс
        public ObservableCollection<NumberItem> GeneratedNumbers { get; } = new ObservableCollection<NumberItem>();

        // Команда для кнопки "Створити кнопки"
        [RelayCommand]
        private void GenerateButtons()
        {
            ErrorMessage = string.Empty;
            GeneratedNumbers.Clear();

            // Валідація
            if (StepValue == 0)
            {
                ErrorMessage = "Помилка: Крок не може бути 0!";
                return;
            }

            if ((StartValue < EndValue && StepValue < 0) || (StartValue > EndValue && StepValue > 0))
            {
                ErrorMessage = "Помилка: Нескінченний цикл (неправильний крок).";
                return;
            }

            // Генерація чисел
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

        // Команда для кнопки "Видалити кратні"
        [RelayCommand]
        private void DeleteMultiples()
        {
            ErrorMessage = string.Empty;

            if (MultipleValue == 0)
            {
                ErrorMessage = "Помилка: Не можна ділити на 0 (кратність 0).";
                return;
            }

            // Знаходимо елементи, які треба видалити.
            var itemsToRemove = GeneratedNumbers.Where(x => x.Value % MultipleValue == 0).ToList();

            foreach (var item in itemsToRemove)
            {
                GeneratedNumbers.Remove(item);
            }
        }
    }
}