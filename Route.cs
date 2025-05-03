using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public class Route
    {
        public string Source;
        public string Destination;
        public Dictionary<string, int> RouteAndDistanceMap;

        public Route(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

    }
}
