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
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (DataContext is not Task2ViewModel vm) return;

            // Отримуємо координати миші відносно всієї ігрової зони (Grid)
            var point = e.GetPosition(GameArea);
            
            double areaWidth = GameArea.Bounds.Width;
            double areaHeight = GameArea.Bounds.Height;

            vm.HandlePointerMoved(point.X, point.Y, areaWidth, areaHeight);
        }
    }
}