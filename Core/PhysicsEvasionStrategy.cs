using System;

namespace OOP_Lab3.Core
{
    public class PhysicsEvasionStrategy : IEvasionStrategy
    {
        private const double TriggerRadius = 100; // На якій дистанції кнопка лякається миші
        private const double PushForce = 40;      // Базова швидкість втечі
        private const double WallMargin = 100;    // На якій дистанції ВІД СТІНИ вона починає відштовхуватись від неї

        public (double newX, double newY) CalculateNewPosition(
            double mouseX, double mouseY, 
            double buttonX, double buttonY, 
            double buttonWidth, double buttonHeight, 
            double areaWidth, double areaHeight)
        {
            double btnCenterX = buttonX + buttonWidth / 2;
            double btnCenterY = buttonY + buttonHeight / 2;

            // 1. Вектор ВІД миші
            double dx = mouseX - btnCenterX;
            double dy = mouseY - btnCenterY;
            double distanceToMouse = Math.Sqrt(dx * dx + dy * dy);

            // Якщо миша далеко - стоїмо на місці
            if (distanceToMouse > TriggerRadius || distanceToMouse == 0)
            {
                return (buttonX, buttonY);
            }

            // Нормалізуємо напрямок і розраховуємо силу поштовху
            double mouseDirX = -dx / distanceToMouse;
            double mouseDirY = -dy / distanceToMouse;
            
            double speedMultiplier = (TriggerRadius - distanceToMouse) / TriggerRadius;
            double moveX = mouseDirX * (PushForce + PushForce * speedMultiplier);
            double moveY = mouseDirY * (PushForce + PushForce * speedMultiplier);

            // 2. МАГНІТНІ СТІНИ (Wall Repulsion) - ось це фіксить проблему з кутами!
            double maxX = Math.Max(0, areaWidth - buttonWidth);
            double maxY = Math.Max(0, areaHeight - buttonHeight);

            double wallForceMultiplier = 1.5; // Стіни відштовхують сильніше за мишу (щоб вирватись з кута)

            // Ліва стіна відштовхує вправо
            if (buttonX < WallMargin) 
                moveX += ((WallMargin - buttonX) / WallMargin) * (PushForce * wallForceMultiplier);
            
            // Права стіна відштовхує вліво
            if (buttonX > maxX - WallMargin) 
                moveX -= ((buttonX - (maxX - WallMargin)) / WallMargin) * (PushForce * wallForceMultiplier);
            
            // Верхня стіна відштовхує вниз
            if (buttonY < WallMargin) 
                moveY += ((WallMargin - buttonY) / WallMargin) * (PushForce * wallForceMultiplier);
            
            // Нижня стіна відштовхує вгору
            if (buttonY > maxY - WallMargin) 
                moveY -= ((buttonY - (maxY - WallMargin)) / WallMargin) * (PushForce * wallForceMultiplier);

            // 3. Застосовуємо розрахований рух (Миша + Стіни)
            double finalX = buttonX + moveX;
            double finalY = buttonY + moveY;

            // 4. Останній запобіжник (щоб кнопка технічно не могла вилізти за межі вікна)
            finalX = Math.Clamp(finalX, 0, maxX);
            finalY = Math.Clamp(finalY, 0, maxY);

            return (finalX, finalY);
        }
    }
}