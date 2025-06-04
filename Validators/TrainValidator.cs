using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Services;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Validators;

public static class TrainValidator
{
    const int MaxTrains = 50;

    // const int Ma
    public static void ValidateNumberOfTrain(int count)
    {
        if (InputValidator.IsOutOfRange(MaxTrains, 0, count))
        {
            throw new InvalidInputExecption($"Value must be between {0} and {MaxTrains}");
        }
    }

    public static void ValidateTrainNumberAndRoute(string[] trainNumberAndRoute)
    {
        try
        {
            int trainNumber = ParseInt(trainNumberAndRoute[0]);
            if (trainNumber < 1) throw new InvalidInputExecption("Train Number Cannot Be Negative");
            if (trainNumberAndRoute.Length < 2)
                throw new InvalidInputExecption("Input Must Contain Train Number And Atleast Source & Destination");
        }
        catch (Exception e)
        {
            OutputHandler.PrintError(e.Message);
            TrainManager.GetTrainDetails();
        }
    }

    public static void ValidateCoachIdRange(string coachId, CoachType coachType)
    {
        if (!int.TryParse(coachId.Substring(1), out var coachNumber))
            throw new InvalidInputExecption($"Coach ID must end with a number: {coachId}");

        switch (coachType)
        {
            case CoachType.SL:
                if (coachNumber < 1 || coachNumber > 18)
                    throw new InvalidInputExecption("Sleeper coach ID must be between S1 and S18");
                break;

            case CoachType.A2:
            case CoachType.A3:
                if (coachNumber < 1 || coachNumber > 3)
                    throw new InvalidInputExecption("2nd/3rd AC coach ID must be between A1–A3 or B1–B3");
                break;

            case CoachType.A1:
                if (coachNumber != 1)
                    throw new InvalidInputExecption("First AC coach ID must be H1");
                break;
        }
    }
}