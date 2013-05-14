using System.Linq;
using LocalGeocoder.Geometry;

namespace LocalGeocoder
{
    public class Geocoder
    {
        private readonly string _dataDir;
        private DataSource _dataSource;

        public Geocoder(string dataDir = "Data")
        {
            _dataDir = dataDir;
        }

        public Result ReverseGeocode(decimal lng, decimal lat)
        {
            return FindResult(lng, lat);
        }

        private DataSource DataSource()
        {
            return _dataSource ?? (_dataSource = new DataSource(_dataDir));
        }

        private Result FindResult(decimal lng, decimal lat)
        {
            var point = new Point(lng, lat);
            var country = FindCountry(point);
            var administrativeArea1 = FindAdministrativeArea1(country.Id, point);
            var administrativeArea2 = FindAdministrativeArea2(country.Id, administrativeArea1.Id, point);
            return new Result(country, administrativeArea1, administrativeArea2);
        }

        private Entity FindAdministrativeArea2(string countryId, string aa1Id, Point point)
        {
            return DataSource().AdministrativeAreasLevel2(countryId, aa1Id)
                               .FirstOrDefault(e => e.Contains(point));
        }

        private Entity FindAdministrativeArea1(string countryId, Point point)
        {
            return DataSource().AdministrativeAreasLevel1(countryId)
                               .FirstOrDefault(e => e.Contains(point));
        }

        private Entity FindCountry(Point point)
        {
            return DataSource().Countries
                               .FirstOrDefault(e => e.Contains(point));
        }
    }
}