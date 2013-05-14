using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace LocalGeocoder.Geometry
{
    internal class Polygon
    {
        private readonly Point[] _points;

        public Polygon(IEnumerable<Point> points) : this(points.ToArray())
        {
        }

        public Polygon(params Point[] points)
        {
            _points = points;
        }

        public static Polygon FromPointArray(JToken points)
        {
            return new Polygon(points.Select(p => new Point((decimal)p[0], (decimal)p[1])));
        }

        public Point this[int index]
        {
            get { return _points[index]; }
        }

        public int NumberOfPoints { get { return _points.Length; } }

        public Rect BoundingBox()
        {
            var minX = _points.Min(p => p.X);
            var maxX = _points.Max(p => p.X);
            var minY = _points.Min(p => p.Y);
            var maxY = _points.Max(p => p.Y);
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        public bool WithinYBounds(Point point, Point p1, Point p2)
        {
            return (p1.Y <= point.Y && point.Y < p2.Y) ||
                   (p2.Y <= point.Y && point.Y < p1.Y);
        }

        public bool IntersectsLineSegment(Point point, Point p1, Point p2)
        {
            return point.X < ((p2.X - p1.X) * (point.Y - p1.Y) / (p2.Y - p1.Y) + p1.X);

        }

        public bool Contains(Point point)
        {
            var boundingBox = BoundingBox();
            if(!boundingBox.Contains(point))
                return false;

            var containsPoint = false;
            var i = -1;
            var j = NumberOfPoints - 1;
            while ((i += 1) < NumberOfPoints)
            {
                var p1 = this[i];
                var p2 = this[j];
                if (WithinYBounds(point, p1, p2) &&
                    IntersectsLineSegment(point, p1, p2))
                {
                    containsPoint = !containsPoint;
                }
                j = i;
            }
            return containsPoint;
        }

        public override string ToString()
        {
            return string.Join(",", _points.Select(p => p.ToString()));
        }
    }
}