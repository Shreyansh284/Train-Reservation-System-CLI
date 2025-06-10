using System.Text;

namespace Train_Reservation_System_CLI.Models;

public class Train(int trainNumber, Route route, List<Coach> coaches)
{
    public readonly List<Coach> Coaches = coaches;
    public readonly Route Route = route;
    public readonly int TrainNumber = trainNumber;
    public readonly List<Ticket> WaitingTickets = new();

    public List<Ticket> GetWaitlistByCoachTypeAndDate(CoachType coachType, DateOnly journeyDate)
    {
        return WaitingTickets
            .Where(t => t.CoachType == coachType && t.Date == journeyDate)
            .ToList();
    }

    public bool HasRoute(string from, string to)
    {
        return Route.IsValidRoute(from, to);
    }

    public bool HasCoach(CoachType coachType)
    {
        return Coaches.Any(c => c.CoachType == coachType);
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
                builder.AppendLine($"    - {coach.CoachId} ({coach.CoachType})");

        return builder.ToString().TrimEnd();
    }
}