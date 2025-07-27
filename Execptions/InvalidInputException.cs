namespace Train_Reservation_System_CLI.Exceptions;

internal class InvalidInputException : Exception
{
    public InvalidInputException(string message) : base(message)
    {
    }
}