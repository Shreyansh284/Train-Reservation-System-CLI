using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI.Models
{
    public class Train
    {
        public int TrainNumber;
        public Route Route;
        public List<Coach> Coaches = new List<Coach>();
        public Waitlist Waitlist = new Waitlist();

        public Train(int TrainNumber, Route Route, List<Coach> Coaches)
        {
            this.TrainNumber = TrainNumber;
            this.Route = Route;
            this.Coaches = Coaches;
        }

        public bool HasRoute(string from, string to)
        {
            return Route.IsValidRoute(from, to);
        }

        public Coach HasCoach(CoachType coachType)
        {
            return Coaches.Find(c => c.CoachType == coachType);
        }
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Train Number : {TrainNumber}");

            // Display route
            builder.Append("Route        : ");
          
            if (Route.RouteAndDistanceMap.Any())
            {
                builder.Append(string.Join("->", Route.RouteAndDistanceMap.Keys));
                builder.Append("->");
            }
       

            // Display coach info
            builder.AppendLine("\nCoaches      :");
            if (Coaches.Count == 0)
            {
                builder.AppendLine("  No coaches available.");
            }
            else
            {
                foreach (var coach in Coaches)
                {
                    builder.AppendLine($"    - {coach.CoachID} ({coach.CoachType})");
                }
            }

            return builder.ToString().TrimEnd();
        }






    }
}
