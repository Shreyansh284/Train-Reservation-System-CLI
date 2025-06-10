using Train_Reservation_System_CLI.Exceptions;
using Train_Reservation_System_CLI.Models;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Validators;

public static class TrainValidator
{
    const int MaxTrains = 50;
    const int MaxSeats = 72;
    const char Seperator = '-';

    public static void ValidateNumberOfTrain(int count)
    {
        if (InputValidator.IsOutOfRange(MaxTrains, 1, count))
        {
            throw new InvalidInputException($"Value must be between {1} and {MaxTrains}");
        }
    }

    public static void ValidateTrainNumberAndRouteInput(string[] trainNumberAndRoute)
    {
        int trainNumber = ParseInt(trainNumberAndRoute[0]);
        if (trainNumber < 1) throw new InvalidInputException("Train Number Cannot Be Negative");
        if (trainNumberAndRoute.Length < 2)
            throw new InvalidInputException("Input Must Contain Train Number And Source & Destination");

        for (var i = 1; i < trainNumberAndRoute.Length; i++)
        {
            var input = trainNumberAndRoute[i].Split(Seperator);
            if (input.Length != 2)
                throw new InvalidInputException($"Route Must Have Name And Distance With Seperator ({Seperator})");
        }
    }

    public static void ValidateTrainCoachesInput(int trainNumber, string[] trainCoaches)
    {
        if (trainNumber != ParseInt(trainCoaches[0]))
            throw new InvalidInputException("Train Number and Coach Number Must Be Same");

        ValidateAtLeastOneSleeperCoach(trainCoaches);

        for (var i = 1; i < trainCoaches.Length; i++)
        {
            var coachParts = trainCoaches[i].Split(Seperator);
            var seats = ParseInt(coachParts[1]);

            if (InputValidator.IsOutOfRange(MaxSeats, 1, seats))
            {
                throw new InvalidInputException($"Value must be between {1} and {MaxSeats}");
            }
        }
    }

    private static void ValidateAtLeastOneSleeperCoach(string[] coachesInput)
    {
        for (int i = 1; i < coachesInput.Length; i++)
        {
            var coachId = coachesInput[i].Split(Seperator)[0];
            var type = GetCoachType(coachId);

            if (type == CoachType.SL)
                return; // Valid, at least one SL exists
        }

        throw new InvalidInputException("At least one Sleeper class (SL) coach is required.");
    }

    private static CoachType GetCoachType(string coachId)
    {
        var type = coachId.StartsWith("S") ? CoachType.SL :
            coachId.StartsWith("B") ? CoachType.A3 :
            coachId.StartsWith("A") ? CoachType.A2 :
            coachId.StartsWith("H") ? CoachType.A1 :
            throw new InvalidInputException($"Unknown coach type for ID: {coachId}");
        return type;
    }

    public static void ValidateCoachIdRange(string coachId, CoachType coachType)
    {
        if (!int.TryParse(coachId.Substring(1), out var coachNumber))
            throw new InvalidInputException($"Coach ID must end with a number: {coachId}");

        switch (coachType)
        {
            case CoachType.SL:
                if (coachNumber < 1 || coachNumber > 18)
                    throw new InvalidInputException("Sleeper coach ID must be between S1 and S18");
                break;

            case CoachType.A2:
            case CoachType.A3:
                if (coachNumber < 1 || coachNumber > 3)
                    throw new InvalidInputException("2nd/3rd AC coach ID must be between A1–A3 or B1–B3");
                break;

            case CoachType.A1:
                if (coachNumber != 1)
                    throw new InvalidInputException("First AC coach ID must be H1");
                break;
        }
    }
}