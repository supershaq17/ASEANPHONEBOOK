namespace AseanPhonebook;

/// <summary>
/// A phone number made of country code, area code and number.
/// Area code and number are kept as strings so leading zeros are not lost.
/// </summary>
public class PhoneNumber
{
    public int CountryCode { get; private set; }
    public string AreaCode { get; private set; }
    public string Number { get; private set; }

    public PhoneNumber(int countryCode, string areaCode, string number)
    {
        ValidateCountryCode(countryCode);
        CountryCode = countryCode;
        AreaCode = RequireDigits(areaCode, "Area code");
        Number = RequireDigits(number, "Phone number");
    }

    public void ChangeCountryCode(int newCode)
    {
        ValidateCountryCode(newCode);
        CountryCode = newCode;
    }

    public void ChangeAreaCode(string newAreaCode) => AreaCode = RequireDigits(newAreaCode, "Area code");

    public void ChangeNumber(string newNumber) => Number = RequireDigits(newNumber, "Phone number");

    public override string ToString() => $"{CountryCode}-{AreaCode}-{Number}";

    private static void ValidateCountryCode(int code)
    {
        if (!CountryRegistry.IsValidCode(code))
        {
            throw new ArgumentException($"Invalid country code {code}. Valid codes: {CountryRegistry.DescribeValidCodes()}.");
        }
    }

    private static string RequireDigits(string value, string label)
    {
        string trimmed = (value ?? string.Empty).Trim();
        if (trimmed.Length == 0 || !trimmed.All(char.IsDigit))
        {
            throw new ArgumentException($"{label} must contain digits only.");
        }
        return trimmed;
    }
}
