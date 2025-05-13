using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI.Models
{
    public class Route
    {
        public string Source;
        public string Destination;
        public Dictionary<string, int> RouteAndDistanceMap = new Dictionary<string, int>();

        public Route(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        public bool IsValidRoute(string from, string to)
        {
            var keyRouteList = RouteAndDistanceMap.Keys.ToList();
            return keyRouteList.Contains(from) && keyRouteList.Contains(to) && keyRouteList.IndexOf(from) < keyRouteList.IndexOf(to);
        }
        public int GetDistance(string from, string to)
        {
            return RouteAndDistanceMap[to] - RouteAndDistanceMap[from];
        }

    }
}
