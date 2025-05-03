using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public class Coach
    {
        public string CoachID;
        public CoachType CoachType;
        public int TotalSeats;
        public Dictionary<DateOnly, int> SeatsByDate;

        public Coach(string coachID,CoachType coachType,int totalSeats) {
        
            CoachID = coachID;
            CoachType = coachType;
            TotalSeats = totalSeats;
        }

        
    }
}
