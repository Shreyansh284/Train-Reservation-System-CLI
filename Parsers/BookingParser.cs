using Train_Reservation_System_CLI.Models;
using static Train_Reservation_System_CLI.Utils.InputUtils;

namespace Train_Reservation_System_CLI.Parsers;

public class BookingParser
{
    public static BookingRequest ParseBookingDetails(string[] bookingDetails)
    {
        return new BookingRequest
        {
            From = bookingDetails[0],
            To = bookingDetails[1],
            Date = DateOnly.Parse(bookingDetails[2]),
            CoachType = Enum.Parse<CoachType>(bookingDetails[3]),
            NoOfSeats = ParseInt(bookingDetails[4])
        };
    }
}