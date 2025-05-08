using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public class Ticket
    {
        public int PNR;
        public int TrainNumber;
        public string From;
        public string To;
        public DateOnly Date;
        public CoachType CoachType;
        public List<string> SeatNumbers;
        public double Fare;

        public Ticket(int pnr, int trainNumber, string from, string to, DateOnly date, CoachType coachType, List<string> seatNumbers, double fare)
        {
            PNR = pnr;
            TrainNumber = trainNumber;
            From = from;
            To = to;
            Date = date;
            CoachType = coachType;
            SeatNumbers = seatNumbers;
            Fare = fare;
        }
        public override string ToString()
        {
            return
                $"========== TICKET DETAILS ==========\n" +
                $"PNR          : {PNR}\n" +
                $"From         : {From}\n" +
                $"To           : {To}\n" +
                $"Date         : {Date}\n" +
                $"Coach Type   : {CoachType}\n" +
                $"Seat Numbers : {string.Join(", ", SeatNumbers)}\n" +
                $"Fare         : INR {Fare:F2}\n" +
                $"====================================";

        }
    }
}
