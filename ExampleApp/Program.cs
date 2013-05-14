using System;
using LocalGeocoder;

namespace ExampleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var geocoder = new Geocoder();
            var start = DateTime.UtcNow;

            var i = 0;
            while (i < 10000)
            {
                var result = geocoder.ReverseGeocode(-122.4194155M, 37.7749295M);
                i++;
            }
            var end = DateTime.UtcNow;
            Console.WriteLine(start.Subtract(end).TotalSeconds);

        }
    }
}
