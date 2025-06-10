namespace Train_Reservation_System_CLI.Models;

public class TrainNumberRouteDTO(int trainNumber, Route route)
{
    public int TrainNumber { get; set; } = trainNumber;
    public Route Route { get; set; } = route;
}