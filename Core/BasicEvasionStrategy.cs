using System;

namespace OOP_Lab3.Core
{
   public class BasicEvasionStrategy : IEvasionStrategy
   {
       private readonly Random _random = new Random();
       private const double TriggerDistance = 100; // Дистанція реакції кнопки

       public (double newX, double newY) CalculateNewPosition(
           double mouseX, double mouseY,
           double buttonX, double buttonY,
           double buttonWidth, double buttonHeight,
           double areaWidth, double areaHeight)
       {
           // Знаходимо центр кнопки
           double btnCenterX = buttonX + buttonWidth / 2;
           double btnCenterY = buttonY + buttonHeight / 2;

           // Рахуємо дистанцію від миші до центру кнопки
           double distance = Math.Sqrt(Math.Pow(mouseX - btnCenterX, 2) + Math.Pow(mouseY - btnCenterY, 2));

           // Якщо миша далеко, кнопка стоїть на місці
           if (distance > TriggerDistance)
           {
               return (buttonX, buttonY);
           }

           // Якщо миша близько - генеруємо нові координати в межах вікна
           double maxX = Math.Max(0, areaWidth - buttonWidth);
           double maxY = Math.Max(0, areaHeight - buttonHeight);

           double newX = _random.NextDouble() * maxX;
           double newY = _random.NextDouble() * maxY;

           return (newX, newY);
       }
   }
}