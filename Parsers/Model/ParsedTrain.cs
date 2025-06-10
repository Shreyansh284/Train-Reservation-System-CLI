using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.Parsers.Model;

public class ParsedTrain(int trainNumber, Route route, List<Coach> coaches)
{
    public readonly List<Coach> Coaches = coaches;
    public readonly Route Route = route;
    public readonly int TrainNumber = trainNumber;
}