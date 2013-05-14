using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LocalGeocoder.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LocalGeocoder
{
    internal class DataSource
    {
        private readonly IEnumerable<Entity> _countries;
        private readonly Dictionary<string, IEnumerable<Entity>> _administrativeAreasLevel1;
        private readonly Dictionary<string, Dictionary<string, IEnumerable<Entity>>> _administrativeAreasLevel2;

        public DataSource(string dir)
        {
            var countriesDir = Path.Combine(dir, "countries");
            const string file = "features.geo.json";
            _countries = LoadData(countriesDir, file);

            _administrativeAreasLevel1 = GetChildEntities(_countries, countriesDir, id => Path.Combine(id, file));
            _administrativeAreasLevel2 = _administrativeAreasLevel1
                .Where(kvp => kvp.Value.Any())
                .Select(kvp =>
                        Tuple.Create(kvp.Key,
                                     GetChildEntities(kvp.Value, countriesDir, id => Path.Combine(kvp.Key, id, file))))
                .Where(t => t.Item2.Any())
                .ToDictionary(t => t.Item1, t => t.Item2);

        }

        public IEnumerable<Entity> Countries
        {
            get { return _countries; }
        }

        private static Dictionary<string, IEnumerable<Entity>> GetChildEntities(IEnumerable<Entity> entities, string dir, Func<string, string> getFileParts)
        {
            return entities.Select(e => Tuple.Create(e.Id, LoadData(dir, getFileParts(e.Id))))
                           .Where(t => t.Item2.Any())
                           .ToDictionary(t => t.Item1, t => t.Item2);
        }

        internal IEnumerable<Entity> AdministrativeAreasLevel1(string countryId)
        {
            IEnumerable<Entity> entities;
            if (!String.IsNullOrEmpty(countryId) &&
                _administrativeAreasLevel1.TryGetValue(countryId, out entities))
                return entities;
            return Enumerable.Empty<Entity>();
        }

        internal IEnumerable<Entity> AdministrativeAreasLevel2(string countryId, string aa1Id)
        {
            Dictionary<string, IEnumerable<Entity>> dictionary;
            IEnumerable<Entity> entities;
            if (!String.IsNullOrEmpty(countryId) &&
                !String.IsNullOrEmpty(aa1Id) &&
                _administrativeAreasLevel2.TryGetValue(countryId, out dictionary) &&
                dictionary.TryGetValue(aa1Id, out entities))
                return entities;
            return Enumerable.Empty<Entity>();
        }

        private static IEnumerable<Entity> LoadData(string dir, string file)
        {
            var path = Path.Combine(dir, file);
            return File.Exists(path) ? LoadData(path).ToArray() : Enumerable.Empty<Entity>();
        }

        private static IEnumerable<Entity> LoadData(string path)
        {
            var streamReader = new StreamReader(path);
            var jsonTextReader = new JsonTextReader(streamReader);
            var jsonSerializer = new JsonSerializer();
            var features = jsonSerializer.Deserialize<JObject>(jsonTextReader)["features"];
            foreach (var feature in features)
            {
                IEnumerable<Polygon> geometries;
                var geometryType = (String)feature["geometry"]["type"];
                var coordinates = feature["geometry"]["coordinates"];
                switch (geometryType)
                {
                    case "MultiPolygon":
                        geometries = coordinates
                            .Select(c => Polygon.FromPointArray(c.First()));
                        break;
                    case "Polygon":
                        geometries = coordinates
                            .Select(Polygon.FromPointArray);
                        break;
                    default:
                        throw new InvalidDataException(string.Format("Cannot process geometry type {0}", geometryType));
                } 
                var id = ((String)feature["id"] ?? String.Empty).Split('-').Last();
                var name = (String)feature["properties"]["name"];
                yield return new Entity(id, name, geometries.ToArray());
            }
        }
    }
}