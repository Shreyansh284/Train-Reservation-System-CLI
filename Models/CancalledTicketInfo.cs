using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI.Models
{
    public class CancalledTicketInfo
    {
        public int TrainNumber;
        public string From;
        public string To;
        public CoachType CoachType;
        public DateOnly Date;
        public List<Seat> ConfirmedCancelledSeats = new();
        public int WaitingCancelledSeats;

        public CancalledTicketInfo(int trainNumber, string from, string to, CoachType coachType, DateOnly date, List<Seat> confirmedCancelledSeats, int waitingCancelledSeats)
        {
            TrainNumber = trainNumber;
            From = from;
            To = to;
            CoachType = coachType;
            Date = date;
            ConfirmedCancelledSeats = confirmedCancelledSeats;
            WaitingCancelledSeats = waitingCancelledSeats;
        }
    }
}
