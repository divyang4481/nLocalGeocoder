namespace LocalGeocoder.Geometry
{
    internal struct Rect
    {
        private readonly decimal _x;
        private readonly decimal _y;
        private readonly decimal _width;
        private readonly decimal _height;

        public Rect(decimal x, decimal y, decimal width, decimal height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public bool Contains(Point point)
        {
            return point.X >= _x &&
                   point.Y >= _y &&
                   point.X <= (_x + _width) &&
                   point.Y <= (_y + _height);
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}],[{2},{3}]", _x, _y, _width, _height);
        }

        public bool Equals(Rect other)
        {
            return _x == other._x && _y == other._y && _width == other._width && _height == other._height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Rect && Equals((Rect)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _x.GetHashCode();
                hashCode = (hashCode * 397) ^ _y.GetHashCode();
                hashCode = (hashCode * 397) ^ _width.GetHashCode();
                hashCode = (hashCode * 397) ^ _height.GetHashCode();
                return hashCode;
            }
        }
    }
}