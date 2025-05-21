namespace Train_Reservation_System_CLI.Validators;

public class InputValidator
{
    public static bool IsOutOfRange(int? max, int? min, int value)
    {
        return (min.HasValue && value < min) || (max.HasValue && value > max);
    }
}