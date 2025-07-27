using Train_Reservation_System_CLI.Exceptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Models;

namespace Train_Reservation_System_CLI.Validators;

public static class BookingCancellationValidator
{
    private const int MaxMinInputLength = 2;
    public static void ValidateNumbersOfSeatsToCancel(int seatsToCancel, Ticket ticket)
    {
        if (seatsToCancel > ticket.TotalNoOfSeats)
            throw new InvalidInputException(OutputHandler.ErrorInvalidSeatCount(seatsToCancel));
    }
    public static void ValidateCancellationInput(string[] input)
    {
        if(input.Length<MaxMinInputLength||input.Length>MaxMinInputLength)
        {
            throw new InvalidInputException("Please Enter  PNR & Seats As Given Above");
        }
    }

}