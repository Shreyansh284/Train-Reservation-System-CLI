using System.Text;

namespace Train_Reservation_System_CLI.Models;

public class Train
{
    public List<Coach> Coaches = new();
    public Route Route;
    public int TrainNumber;
    public Waitlist Waitlist = new();

    public Train(int TrainNumber, Route Route, List<Coach> Coaches)
    {
        this.TrainNumber = TrainNumber;
        this.Route = Route;
        this.Coaches = Coaches;
    }

    public bool HasRoute(string from, string to)
    {
        return Route.IsValidRoute(from, to);
    }

    public Coach HasCoach(CoachType coachType)
    {
        return Coaches.Find(c => c.CoachType == coachType);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.AppendLine($"Train Number : {TrainNumber}");

        // Display route
        builder.Append("Route        : ");

        if (Route.RouteStations.Any())
        {
            builder.Append(string.Join("->", Route.RouteStations.Select(s => s.StationName)));
            builder.Append("->");
        }


        // Display coach info
        builder.AppendLine("\nCoaches      :");
        if (Coaches.Count == 0)
            builder.AppendLine("  No coaches available.");
        else
            foreach (var coach in Coaches)
                builder.AppendLine($"    - {coach.CoachID} ({coach.CoachType})");

        return builder.ToString().TrimEnd();
    }
}