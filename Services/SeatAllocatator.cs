using Train_Reservation_System_CLI.Models;
using static Train_Reservation_System_CLI.Validators.RouteValidator;

namespace Train_Reservation_System_CLI.Services;

public static class SeatAllocator
{
    public static SeatAllocationResult AllocateSeats(Train train, BookingRequest request)
    {
        var result = new SeatAllocationResult();
        var coaches = train.Coaches.Where(c => c.CoachType == request.CoachType).ToList();
        var stations = train.Route.GetStations();
        var reservedSeats = coaches.SelectMany(c => c.GetReservedSeats(request.Date)).ToList();
        int remaining = request.NoOfSeats;

            remaining = AssignFromReservedSeats(reservedSeats, stations, request, result, remaining);

        if (remaining > 0)
            remaining = AssignNewSeats(coaches, request, result, remaining);

        result.WaitingSeats = remaining;
        return result;
    }

    private static int AssignFromReservedSeats(
        List<Seat> reservedSeats, List<string> stations, BookingRequest request,
        SeatAllocationResult result, int remaining)
    {
        foreach (var seat in reservedSeats)
        {
            var reservation = seat.Reservations.Single(r => r.Date == request.Date);

            bool isUnassigned = reservation.BookedRoutes == null || reservation.BookedRoutes.Count == 0;
            bool isNonOverlapping = !IsOverlapping(request.From, request.To, stations, reservation.BookedRoutes);

            if (isUnassigned || isNonOverlapping)
            {
                reservation.AddRequestedRoute(new BookedRoute(request.From, request.To));
                result.BookedSeats.Add(seat);
                remaining--;
            }

            if (remaining == 0)
                break;
        }

        return remaining;
    }

    private static int AssignNewSeats(
        List<Coach> coaches, BookingRequest request, SeatAllocationResult result, int remaining)
    {
        foreach (var coach in coaches)
        {
            int available = coach.AvailableSeatsCount(request.Date);
            int booked = coach.GetReservedSeats(request.Date).Count;
            int toBook = Math.Min(available, remaining);

            for (int i = 1; i <= toBook; i++)
            {
                var seat = new Seat($"{coach.CoachId}-{booked + i}");
                var reservation = new Reservation(request.Date);
                reservation.AddRequestedRoute(new BookedRoute(request.From, request.To));
                seat.Reservations.Add(reservation);
                coach.Seats.Add(seat);
                result.BookedSeats.Add(seat);
            }

            remaining -= toBook;
            if (remaining == 0)
                break;
        }

        return remaining;
    }
}