namespace AseanPhonebook;

/// <summary>
/// Reads and validates what the user types, so the rest of the program receives clean data.
/// </summary>
public static class ConsoleInput
{
    public static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Trim();
            if (input.Length > 0)
            {
                return input;
            }
            Console.WriteLine("Input cannot be empty. Please try again.");
        }
    }

    public static char ReadGender(string prompt)
    {
        while (true)
        {
            string input = ReadRequired(prompt).ToUpperInvariant();
            if (input == "M" || input == "F")
            {
                return input[0];
            }
            Console.WriteLine("Please enter M for male or F for female.");
        }
    }

    public static int ReadCountryCode(string prompt)
    {
        while (true)
        {
            string input = ReadRequired(prompt);
            if (int.TryParse(input, out int code) && CountryRegistry.IsValidCode(code))
            {
                return code;
            }
            Console.WriteLine($"Invalid country code. Valid codes: {CountryRegistry.DescribeValidCodes()}.");
        }
    }

    public static string ReadDigits(string prompt, string label)
    {
        while (true)
        {
            string input = ReadRequired(prompt);
            if (input.All(char.IsDigit))
            {
                return input;
            }
            Console.WriteLine($"{label} must contain digits only.");
        }
    }

    public static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            string input = ReadRequired(prompt).ToUpperInvariant();
            if (input == "Y") return true;
            if (input == "N") return false;
            Console.WriteLine("Please enter Y or N.");
        }
    }

    /// <summary>Reads a menu number. Returns -1 when the input is not a whole number.</summary>
    public static int ReadMenuChoice(string prompt)
    {
        Console.Write(prompt);
        string input = (Console.ReadLine() ?? string.Empty).Trim();
        return int.TryParse(input, out int choice) ? choice : -1;
    }
}
