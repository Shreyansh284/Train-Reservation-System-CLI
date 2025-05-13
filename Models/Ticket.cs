using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI.Models
{
    public class Ticket
    {
        public int PNR;
        public int TrainNumber;
        public string From;
        public string To;
        public DateOnly Date;
        public CoachType CoachType;
        public int WaitingSeats;
        public List<Seat> BookedSeats;
        public int TotalNoOfSeats;
        public double Fare;

        public Ticket(int pnr, int trainNumber, string from, string to, DateOnly date, CoachType coachType, int seatsInWaiting, List<Seat> bookedSeats, int totalSeats, double fare)
        {
            PNR = pnr;
            TrainNumber = trainNumber;
            From = from;
            To = to;
            Date = date;
            CoachType = coachType;
            WaitingSeats = seatsInWaiting;
            BookedSeats = bookedSeats;
            TotalNoOfSeats = totalSeats;
            Fare = fare;
        }
        public override string ToString()
        {
            return
                $"========== TICKET DETAILS ==========\n" +
                $"PNR              : {PNR}\n" +
                $"From             : {From}\n" +
                $"To               : {To}\n" +
                $"Coach Type       : {CoachType}\n" +
                $"Date             : {Date}\n" +
                $"Seats In Waiting : {WaitingSeats}\n" +
                $"Booked Seats     : {string.Join(", ", BookedSeats.Select(s => s.SeatNumber))}\n" +
                $"Total Seats      : {TotalNoOfSeats}\n" +
                $"Fare             : INR {Fare:F2}\n" +
                $"====================================";

        }
    }
}
