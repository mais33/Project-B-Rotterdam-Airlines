public static class DateLogic
{
    // Reads a date that is written as dd-mm-yyyy, for example 25-12-2026
    public static bool TryReadDate(string text, out DateTime date)
    {
        date = DateTime.MinValue;

        if (text == null)
        {
            return false;
        }

        string[] parts = text.Split('-');

        if (parts.Length != 3)
        {
            return false;
        }

        if (!IsNumber(parts[0], 2) || !IsNumber(parts[1], 2) || !IsNumber(parts[2], 4))
        {
            return false;
        }

        int day = int.Parse(parts[0]);
        int month = int.Parse(parts[1]);
        int year = int.Parse(parts[2]);

        if (year < 1 || month < 1 || month > 12)
        {
            return false;
        }

        if (day < 1 || day > DateTime.DaysInMonth(year, month))
        {
            return false;
        }

        date = new DateTime(year, month, day);
        return true;
    }

    // Reads a date and time that is written as dd-mm-yyyy hh:mm, for example 25-12-2026 14:30
    public static bool TryReadDateTime(string text, out DateTime dateTime)
    {
        dateTime = DateTime.MinValue;

        if (text == null)
        {
            return false;
        }

        string[] parts = text.Split(' ');

        if (parts.Length != 2)
        {
            return false;
        }

        DateTime date;

        if (!TryReadDate(parts[0], out date))
        {
            return false;
        }

        string[] timeParts = parts[1].Split(':');

        if (timeParts.Length != 2)
        {
            return false;
        }

        if (!IsNumber(timeParts[0], 2) || !IsNumber(timeParts[1], 2))
        {
            return false;
        }

        int hour = int.Parse(timeParts[0]);
        int minute = int.Parse(timeParts[1]);

        if (hour > 23 || minute > 59)
        {
            return false;
        }

        dateTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
        return true;
    }

    // Writes a date as dd-mm-yyyy
    public static string DateToText(DateTime date)
    {
        return date.Day.ToString("00") + "-" + date.Month.ToString("00") + "-" + date.Year.ToString("0000");
    }

    // Writes a date and time as dd-mm-yyyy hh:mm
    public static string DateTimeToText(DateTime dateTime)
    {
        return DateToText(dateTime) + " " + dateTime.Hour.ToString("00") + ":" + dateTime.Minute.ToString("00");
    }

    // Checks if the text only has digits and has the right length
    private static bool IsNumber(string text, int length)
    {
        if (text.Length != length)
        {
            return false;
        }

        foreach (char c in text)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }
        }

        return true;
    }
}
