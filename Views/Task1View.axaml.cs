using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace OOP_Lab3.Views
{
    public partial class Task1View : UserControl
    {
        public Task1View()
        {
            InitializeComponent();
            
            this.AddHandler(InputElement.TextInputEvent, OnPreviewTextInput, RoutingStrategies.Tunnel);
        }

        private void OnPreviewTextInput(object? sender, TextInputEventArgs e)
        {
            if (e.Text != null && (e.Text.Contains('.') || e.Text.Contains(',')))
            {
                e.Handled = true;
            }
        }
    }
}