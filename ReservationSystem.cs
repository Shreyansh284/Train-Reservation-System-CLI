using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public class ReservationSystem
    {
        private TrainManager trainManager;

        public ReservationSystem(TrainManager trainManager)
        {
            this.trainManager = trainManager;
        }
        public List<Train> Trains => trainManager.Trains;

        public int PNR = 10000000;

        public string BookTicket(string from, string to, DateOnly date, CoachType coachType, int noOfSeats)
        {
            if (noOfSeats <= 0 || noOfSeats > 24)
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidSeatCount(noOfSeats));

            if (!(date >= DateOnly.FromDateTime(DateTime.Today)))
                throw new InvalidInputExecption(OutputHandler.ErrorInvalidDate(date));

            foreach (var train in Trains)
            {
                if (!train.HasRoute(from, to))
                    continue;

                if (train.HasCoach(coachType) == null)
                    throw new InvalidInputExecption(OutputHandler.ErrorCoachUnavailable(coachType));

                var coaches = train.Coaches.Where(c => c.CoachType == coachType).ToList();
                int totalAvailableSeats = coaches.Sum(c => c.GetAvailableSeats(date));

                if (totalAvailableSeats < noOfSeats)
                    throw new InvalidInputExecption(OutputHandler.ErrorInsufficientSeats(coachType, totalAvailableSeats));

                BookSeatsAcrossCoaches(coaches, date, noOfSeats);

                double fare = CalculateFare(train.Route.GetDistance(from, to), coachType, noOfSeats);
                return OutputHandler.SuccessBooking(++PNR, fare);
            }

            throw new InvalidInputExecption(OutputHandler.ErrorNoTrainAvailable());
        }


        private void BookSeatsAcrossCoaches(List<Coach> coaches, DateOnly date, int seatsToBook)
        {
            foreach (var coach in coaches)
            {
                int available = coach.TotalSeats - coach.SeatsByDate[date];
                int toBook = Math.Min(seatsToBook, available);

                coach.SeatsByDate[date] += toBook;
                seatsToBook -= toBook;

                if (seatsToBook == 0)
                    break;
            }
        }
        private static double CalculateFare(int distance, CoachType coachType, int noOfSeats)
        {
            return distance * (int)coachType * noOfSeats;
        }

    }
}