namespace Train_Reservation_System_CLI.Utils;

public static class InputUtils
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