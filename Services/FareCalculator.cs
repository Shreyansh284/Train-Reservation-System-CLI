using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.Services;

public static class FareCalculator
{
    public static double CalculateFare(int distance, CoachType coachType, int noOfSeats)
    {
        return distance * (int)coachType * noOfSeats;
    }
}