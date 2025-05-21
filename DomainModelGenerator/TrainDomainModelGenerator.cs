using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Parsers.Model;

namespace Train_Reservation_System_CLI.DomainModelGenerator;

public class TrainDomainModelGenerator
{
    public static Train GenerateTrainDominModel(ParsedTrain parsedTrain)
    {
        var train = new Train(parsedTrain.TrainNumber, parsedTrain.Route, parsedTrain.Coaches);
        return train;
    }
}