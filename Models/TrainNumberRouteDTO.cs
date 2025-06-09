namespace Train_Reservation_System_CLI.Models;

public class TrainNumberRouteDTO
{
    public int TrainNumber { get; set; }
    public Route Route { get; set; }

    public TrainNumberRouteDTO(int trainNumber, Route route)
    {
        TrainNumber = trainNumber;
        Route = route;
    }
}