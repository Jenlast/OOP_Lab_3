using System;

namespace OOP_Lab3.Core
{
    public class PhysicsEvasionStrategy : IEvasionStrategy
    {
        private const double TriggerRadius = 100; // Дистанція, на якій кнопка "лякається"
        private const double PushForce = 30;      // Сила відштовхування (швидкість)
        private readonly Random _random = new Random();

        public (double newX, double newY) CalculateNewPosition(
            double mouseX, double mouseY, 
            double buttonX, double buttonY, 
            double buttonWidth, double buttonHeight, 
            double areaWidth, double areaHeight)
        {
            // 1. Знаходимо центр кнопки
            double btnCenterX = buttonX + buttonWidth / 2;
            double btnCenterY = buttonY + buttonHeight / 2;

            // 2. Рахуємо вектор від кнопки до миші
            double dx = mouseX - btnCenterX;
            double dy = mouseY - btnCenterY;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Якщо миша далеко або прямо по центру (щоб уникнути ділення на 0)
            if (distance > TriggerRadius || distance == 0)
            {
                return (buttonX, buttonY); // Стоїмо на місці
            }

            // 3. Нормалізуємо вектор (робимо його довжину рівною 1) і беремо протилежний напрямок (від миші)
            double directionX = -dx / distance;
            double directionY = -dy / distance;

            // Чим ближче миша, тим сильніший поштовх
            double speedMultiplier = (TriggerRadius - distance) / TriggerRadius; // від 0 до 1
            double actualSpeed = PushForce + (PushForce * speedMultiplier);

            // 4. Обчислюємо нові координати (ковзання)
            double proposedX = buttonX + (directionX * actualSpeed);
            double proposedY = buttonY + (directionY * actualSpeed);

            // 5. Фізика стін (Clamp - не даємо виїхати за межі екрану)
            double maxX = Math.Max(0, areaWidth - buttonWidth);
            double maxY = Math.Max(0, areaHeight - buttonHeight);

            double finalX = Math.Clamp(proposedX, 0, maxX);
            double finalY = Math.Clamp(proposedY, 0, maxY);

            // 6. "Паніка" у куті: якщо миша затисла кнопку в кут екрану і вона не може рухатись
            if (Math.Abs(finalX - buttonX) < 1 && Math.Abs(finalY - buttonY) < 1)
            {
                // Кнопка робить ривок (наприклад, ковзає вбік або перелітає на інший кінець)
                finalX = _random.NextDouble() > 0.5 ? 0 : maxX;
                finalY = _random.NextDouble() > 0.5 ? 0 : maxY;
            }

            return (finalX, finalY);
        }
    }
}