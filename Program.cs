using System.Diagnostics;
using static System.Console;

namespace SnakeGame
{
    class Program
    {
        private const ushort PIXEL_SIZE = 3;
        private const ushort MAP_WIDTH = 60;
        private const ushort MAP_HEIGHT = 20;
        private const ushort FRAME_MS = 150;

        private const ushort SCREEN_WIDTH = MAP_WIDTH * PIXEL_SIZE;
        private const ushort SCREEN_HEIGHT = MAP_HEIGHT * PIXEL_SIZE;
        private const ConsoleColor BORDER_COLOR = ConsoleColor.DarkBlue;
        private const ConsoleColor HEAD_COLOR = ConsoleColor.Magenta;
        private const ConsoleColor BODY_COLOR = ConsoleColor.Blue;
        private const ConsoleColor FOOD_COLOR = ConsoleColor.Green;
        private static readonly Random random = new();
        private static uint _score = 0;
        static void Main()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetWindowSize(SCREEN_WIDTH, SCREEN_HEIGHT);
                try
                {
                    SetBufferSize(SCREEN_WIDTH, SCREEN_HEIGHT);
                }
                catch (Exception)
                {
                    WriteLine("Failed to set buffer size. Restart Console and try again.");
                }   
            }

            CursorVisible = false;
            Clear();
            DrawBorder();
            SpawnSnakeAndPlay();
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;

            ConsoleKey key = ReadKey(true).Key;
            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };

            return currentDirection;
        }

        static void DrawBorder()
        {
            for (int i = 0; i < MAP_WIDTH; i++)
            {
                new Pixel(i, 0, BORDER_COLOR).Draw();
                new Pixel(i, MAP_HEIGHT - 1, BORDER_COLOR).Draw();
            }

            for (int i = 0; i < MAP_HEIGHT - 1; i++)
            {
                new Pixel(0, i, BORDER_COLOR).Draw();
                new Pixel(MAP_WIDTH - 1, i, BORDER_COLOR).Draw();
            }
        }

        static void SpawnSnakeAndPlay(){
            Snake snake = new(15, 5, HEAD_COLOR, BODY_COLOR, 5);
            Pixel food = GenerateFood(snake);
            food.Draw();
            Direction userDirection = Direction.Right;
            Stopwatch stopwatch = new Stopwatch();
            int lagMs = 0;

            while(true)
            {
                stopwatch.Restart();

                Direction oldDirection = userDirection;

                while (stopwatch.ElapsedMilliseconds <= FRAME_MS - lagMs)
                {
                    if (userDirection == oldDirection)
                    {
                        userDirection = ReadMovement(userDirection);
                    }
                }

                stopwatch.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(userDirection, eat: true);
                    food = GenerateFood(snake);
                    food.Draw();
                    _score++;
                    Task.Run(() => Beep(600, 200));
                }
                else
                {
                    snake.Move(userDirection);
                }

                if (snake.isInBadPosition(MAP_WIDTH, MAP_HEIGHT))
                    break;

                lagMs = (int) stopwatch.ElapsedMilliseconds;
            }

            snake.Clear();
            food.Clear();
            SetCursorPosition(SCREEN_WIDTH / 2 - 17, SCREEN_HEIGHT / 2);
            WriteLine($"Game over, score {_score}");
            Task.Run(() => Beep(200, 600));
            Thread.Sleep(200);
            ReadKey();
        }

        static Pixel GenerateFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(random.Next(1, MAP_WIDTH - 2), random.Next(1, MAP_HEIGHT - 2), FOOD_COLOR);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                snake.Body.Any(pixel => pixel.X == food.X && pixel.Y == food.Y));

            return food;
        }
    }
}