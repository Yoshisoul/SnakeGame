
namespace SnakeGame
{
    public struct Pixel
    {
        private const char PIXEL_CHAR = '█';
        private Func<int> next1;
        private Func<int> next2;
        private ConsoleColor red;

        public int X { get; }
        public int Y { get; }
        public ConsoleColor Color { get; }
        public int PixelSize { get; }

        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }

        public void Draw()
        {
            Console.ForegroundColor = Color;

            for (int i = 0; i < PixelSize; i++)
            {
                for (int j = 0; j < PixelSize; j++)
                {
                    Console.SetCursorPosition(X * PixelSize + i, Y * PixelSize + j);
                    Console.Write(PIXEL_CHAR);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < PixelSize; i++)
            {
                for (int j = 0; j < PixelSize; j++)
                {
                    Console.SetCursorPosition(X * PixelSize + i, Y * PixelSize + j);
                    Console.Write(' ');
                }
            }
        }
    }
}