using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WPF_AppList._3_GDI_Canvas
{
    public delegate void BrickEventHandler(BrickEventArgs e);

    public static class Movement
    {
        public static event EventHandler BallOufOfBoundsEvent;
        public static event BrickEventHandler BallCollisionEvent;
        public static event BrickEventHandler BonusBrickCollisionEvent;
        public static event BrickEventHandler BonusBrickOutOfBoundsEvent;
        // Ball out of bounds game over

        /// <summary>
        /// Logic for ball movement on canvas.
        /// </summary>
        public static void BallMove(Canvas canvas, Ellipse ball, Rectangle ship, List<Rectangle> bricksList)
        {
            var ballData = (ItemData)ball.Tag;
            double ballX = Canvas.GetLeft(ball) + ballData.VelocityX;
            double ballY = Canvas.GetTop(ball) + ballData.VelocityY;


            // Left border collision
            if (ballX <= 0) 
            {
                ballData.VelocityX = -ballData.VelocityX;
                ballX = 0;
            }

            // Right border collision
            if (ballX >= canvas.ActualWidth - ball.ActualWidth) 
            {
                ballData.VelocityX = -ballData.VelocityX;
                ballX = canvas.ActualWidth - ball.ActualWidth;
            }

            // Top border collision
            if (ballY <= 0) 
            {
                ballData.VelocityY = -ballData.VelocityY;
                ballY = 0;
            }

            // Bottom collision - game over
            if (ballY >= canvas.ActualHeight - ball.ActualHeight / 2)
            {
                BallOufOfBoundsEvent?.Invoke(null, null);
                return;
            }

            // Bottom - ship collision
            if (ballY >= Canvas.GetTop(ship) - ball.ActualHeight)  
            {
                double shipX = Canvas.GetLeft(ship);

                if (ballX + ball.ActualWidth / 2 >= shipX
                 && ballX + ball.ActualWidth / 2 <= shipX + ship.ActualWidth)
                {
                    ballData.VelocityY = -ballData.VelocityY;
                    ballY = Canvas.GetTop(ship) - ball.ActualHeight;
                }
            }

            // Brick collision
            Rectangle? removed = null;
            foreach (var brick in bricksList)
            {
                double brickX = Canvas.GetLeft(brick);
                double brickY = Canvas.GetTop(brick);

                if (ballX + ball.ActualWidth / 2 >= brickX
                 && ballX + ball.ActualWidth / 2 <= brickX + brick.ActualWidth)
                {
                    if (ballY + ball.ActualHeight >= brickY
                     && ballY + ball.ActualHeight <= brickY + 2 * Math.Abs(ballData.VelocityY))
                    {
                        // Cверху
                        ballData.VelocityY = -ballData.VelocityY;
                        removed = brick;
                    }
                    if (ballY <= brickY + brick.ActualHeight
                     && ballY >= brickY + brick.ActualHeight - 2 * Math.Abs(ballData.VelocityY))
                    {
                        // Снизу
                        ballData.VelocityY = -ballData.VelocityY;
                        removed = brick;
                    }
                }
                else if (ballY + ball.ActualHeight / 2 >= brickY
                      && ballY + ball.ActualHeight / 2 <= brickY + brick.ActualHeight)
                {
                    if (ballX + ball.ActualWidth >= brickX
                     && ballX + ball.ActualWidth <= brickX + 2 * Math.Abs(ballData.VelocityX))
                    {
                        // Слева
                        ballData.VelocityX = -ballData.VelocityX;
                        removed = brick;
                    }
                    if (ballX <= brickX + brick.ActualWidth
                     && ballX >= brickX + brick.ActualWidth - 2 * Math.Abs(ballData.VelocityX))
                    {
                        // Справа
                        ballData.VelocityX = -ballData.VelocityX;
                        removed = brick;
                    }
                }
            }
            if (removed != null)
            {
                BallCollisionEvent?.Invoke(new BrickEventArgs(removed));
            }


            // Setting new ball data
            ball.Tag = ballData;
            Canvas.SetLeft(ball, ballX);
            Canvas.SetTop(ball, ballY);
        }

        /// <summary>
        /// Logic for ship movement on canvas.
        /// </summary>
        public static void ShipMove(Canvas canvas, Rectangle ship, bool leftKeyHold, bool rightKeyHold)
        {
            var shipData = (ItemData)ship.Tag;
            double x = Canvas.GetLeft(ship);

            if (leftKeyHold)
            {
                if (x > shipData.VelocityX) 
                    x -= shipData.VelocityX;
                else 
                    x = 0;

                Canvas.SetLeft(ship, x);
            }
            if (rightKeyHold)
            {
                if (x < canvas.ActualWidth - ship.ActualWidth - shipData.VelocityX)
                    x += shipData.VelocityX;
                else 
                    x = canvas.ActualWidth - ship.ActualWidth;

                Canvas.SetLeft(ship, x);
            }
        }

        /// <summary>
        /// Logic for bonus brick movement on canvas.
        /// </summary>
        public static void BonusBrickMove(Canvas canvas, Rectangle bonusBrick, Rectangle ship)
        {
            var bonusBrickData = (ItemData)bonusBrick.Tag;

            // Out of bounds
            if (Canvas.GetTop(bonusBrick) >= canvas.ActualHeight)
                BonusBrickOutOfBoundsEvent?.Invoke(new BrickEventArgs(bonusBrick));

            // Ship collision
            Rect _brick = new(Canvas.GetLeft(bonusBrick), Canvas.GetTop(bonusBrick),
                              bonusBrick.ActualWidth, bonusBrick.ActualHeight);
            Rect _ship = new(Canvas.GetLeft(ship), Canvas.GetTop(ship),
                             ship.ActualWidth, ship.ActualHeight);
            if (_brick.IntersectsWith(_ship))
                BonusBrickCollisionEvent?.Invoke(new BrickEventArgs(bonusBrick));

            // Move brick
            double bonusBrickY = Canvas.GetTop(bonusBrick) + bonusBrickData.VelocityY;
            Canvas.SetTop(bonusBrick, bonusBrickY);
        }
    }
}
