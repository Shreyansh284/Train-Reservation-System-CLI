using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Models;
using Train_Reservation_System_CLI.Services.IOHandlers;

namespace Train_Reservation_System_CLI.Services.Parsers
{
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
                NoOfSeats = int.Parse(bookingDetails[4])
            };
        }
    }
}
