namespace Train_Reservation_System_CLI.Execptions;

internal class InvalidInputExecption : Exception
{
    public InvalidInputExecption(string message) : base(message)
    {
    }
}