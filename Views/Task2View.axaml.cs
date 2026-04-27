using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using OOP_Lab3.ViewModels;

namespace OOP_Lab3.Views
{
    public partial class Task2View : UserControl
    {
        public Task2View()
        {
            InitializeComponent();
            
            // Відслідковуємо подію зміни розміру нашого головного поля (в т.ч. при запуску програми)
            GameArea.SizeChanged += GameArea_SizeChanged;
        }

        private void GameArea_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            if (DataContext is Task2ViewModel vm)
            {
                // Отримуємо координати нашої пустої DummyPanel відносно загального GameArea
                var point = DummyPanel.TranslatePoint(new Point(0, 0), GameArea);
                
                if (point.HasValue)
                {
                    // Передаємо ці ідеальні координати у ViewModel
                    vm.SetStartPosition(point.Value.X, point.Value.Y);
                }
            }
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (DataContext is not Task2ViewModel vm) return;

            var point = e.GetPosition(GameArea);
            double areaWidth = GameArea.Bounds.Width;
            double areaHeight = GameArea.Bounds.Height;

            vm.HandlePointerMoved(point.X, point.Y, areaWidth, areaHeight);
        }
    }
}