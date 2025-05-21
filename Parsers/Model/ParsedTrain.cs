using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.Parsers.Model;

public class ParsedTrain
{
    public List<Coach> Coaches = new();
    public Route Route;
    public int TrainNumber;

    public ParsedTrain(int trainNumber, Route route, List<Coach> coaches)
    {
        TrainNumber = trainNumber;
        Route = route;
        Coaches = coaches;
    }
}