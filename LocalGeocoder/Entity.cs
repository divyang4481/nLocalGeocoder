using System.Collections.Generic;
using System.Linq;
using LocalGeocoder.Geometry;

namespace LocalGeocoder
{
    internal struct Entity
    {
        private readonly string _id;
        private readonly string _name;
        private readonly IEnumerable<Polygon> _geometries;

        public Entity(string id, string name, IEnumerable<Polygon> geometries)
        {
            _id = id;
            _name = name;
            _geometries = geometries;
        }

        public string Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool Contains(Point point)
        {
            return _geometries.Any(g => g.Contains(point));
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(_id) ? _name : string.Format("{0} ({1})", _name, _id);
        }
    }
}