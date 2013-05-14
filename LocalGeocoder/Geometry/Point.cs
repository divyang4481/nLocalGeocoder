namespace LocalGeocoder.Geometry
{
    internal class Point
    {
        private readonly decimal _x;
        private readonly decimal _y;

        public Point(decimal x, decimal y)
        {
            _x = x;
            _y = y;
        }

        public decimal X
        {
            get { return _x; }
        }

        public decimal Y
        {
            get { return _y; }
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", _x, _y);
        }
    }
}
