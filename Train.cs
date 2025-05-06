using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public class Train
    {
        public int TrainNumber;
        public Route Route;
        public List<Coach> Coaches = new List<Coach>();

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



    }
}
