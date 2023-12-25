using System.Reflection;

namespace SnakeGame
{
    public class Snake
    {
        public Pixel Head { get; private set; }
        private Queue<Pixel> _body;
        public Queue<Pixel> Body
        {
            get
            {
                Queue<Pixel> temp = new(_body);
                return temp;
            }
        }
        public ushort InitialX { get; }
        public ushort InitialY { get; }
        public ConsoleColor HeadColor { get; }
        public ConsoleColor BodyColor { get; }
        public ushort BodyLength { get; private set; }
        public const ushort PIXEL_SIZE = Program.PIXEL_SIZE;

        public Snake
        (
            ushort initialX, 
            ushort initialY, 
            ConsoleColor headColor, 
            ConsoleColor bodyColor, 
            ushort bodyLength = 3
        )
        {
            InitialX = initialX;
            InitialY = initialY;
            HeadColor = headColor;
            BodyColor = bodyColor;
            BodyLength = bodyLength;
            _body = new Queue<Pixel>();

            Head = new Pixel(initialX, InitialY, HeadColor, PIXEL_SIZE);

            for (int i = BodyLength; i > 0; i--)
            {
                _body.Enqueue(new Pixel( Head.X - i, Head.Y, BodyColor, PIXEL_SIZE));
            }

            Draw();
        }

        public void Draw()
        {
            Head.Draw();

            foreach (var pixel in _body)
            {
                pixel.Draw();
            }
        }

        public void Clear()
        {
            Head.Clear();

            foreach (var pixel in _body)
            {
                pixel.Clear();
            }
        }

        public void Move(Direction direction, bool eat = false)
        {
            if (!eat)
            {
                var tail = _body.Dequeue();
                tail.Clear();
            }

            var prevHead = new Pixel(Head.X, Head.Y, BodyColor, PIXEL_SIZE);
            _body.Enqueue(prevHead);

            Head = direction switch
            {
                Direction.Right => new Pixel(Head.X + 1, Head.Y, HeadColor, PIXEL_SIZE),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, HeadColor, PIXEL_SIZE),
                Direction.Up => new Pixel(Head.X, Head.Y - 1, HeadColor, PIXEL_SIZE),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, HeadColor, PIXEL_SIZE),
                _ => Head
            };

            prevHead.Draw();
            Head.Draw();
        }

        public bool isInBadPosition(ushort MAP_WIDTH, ushort MAP_HEIGHT)
        {
            return Head.X == 0 || 
            Head.X == MAP_WIDTH - 1 || 
            Head.Y == 0 || 
            Head.Y == MAP_HEIGHT - 1 || 
            _body.Any(pixel => pixel.X == Head.X && pixel.Y == Head.Y);
        }
    }
}