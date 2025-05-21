namespace Train_Reservation_System_CLI.Utils;

public class InputUtils
{
    public static int ParseInt(string input)
    {
        return int.Parse(input);
    }

    public static string[] SplitInput(string input)
    {
        return input.Split(' ');
    }
}