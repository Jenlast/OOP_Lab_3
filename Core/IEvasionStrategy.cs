namespace OOP_Lab3.Core
{
   public interface IEvasionStrategy
   {
       // Повертає нові координати (X, Y) для кнопки
       (double newX, double newY) CalculateNewPosition(
           double mouseX, double mouseY,
           double buttonX, double buttonY,
           double buttonWidth, double buttonHeight,
           double areaWidth, double areaHeight);
   }
}

