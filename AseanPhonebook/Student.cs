namespace AseanPhonebook;

/// <summary>
/// One entry of the phonebook. A Student owns a PhoneNumber object.
/// </summary>
public class Student
{
    public string StudentNumber { get; private set; }
    public string Surname { get; private set; }
    public string FirstName { get; private set; }
    public string Occupation { get; private set; }
    public char Gender { get; private set; }
    public PhoneNumber Phone { get; }

    public Student(string studentNumber, string surname, string firstName,
                   string occupation, char gender, PhoneNumber phone)
    {
        StudentNumber = RequireText(studentNumber, "Student number");
        Surname = RequireText(surname, "Surname");
        FirstName = RequireText(firstName, "First name");
        Occupation = RequireText(occupation, "Occupation");
        Gender = NormalizeGender(gender);
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
    }

    public void ChangeStudentNumber(string newNumber) => StudentNumber = RequireText(newNumber, "Student number");
    public void ChangeSurname(string newSurname) => Surname = RequireText(newSurname, "Surname");
    public void ChangeFirstName(string newFirstName) => FirstName = RequireText(newFirstName, "First name");
    public void ChangeOccupation(string newOccupation) => Occupation = RequireText(newOccupation, "Occupation");
    public void ChangeGender(char newGender) => Gender = NormalizeGender(newGender);

    public bool HasStudentNumber(string studentNumber) =>
        string.Equals(StudentNumber, studentNumber.Trim(), StringComparison.OrdinalIgnoreCase);

    private string Pronoun => Gender == 'M' ? "His" : "Her";

    private string OccupationWithArticle
    {
        get
        {
            bool startsWithVowel = "AEIOUaeiou".IndexOf(Occupation[0]) >= 0;
            return (startsWithVowel ? "an " : "a ") + Occupation;
        }
    }

    /// <summary>Format used by the edit menu.</summary>
    public string ToEditSummary() =>
        $"{FirstName} {Surname} is {OccupationWithArticle}. {Pronoun} number is {Phone}";

    /// <summary>Format used by the search results.</summary>
    public string ToSearchSummary() =>
        $"{Surname}, {FirstName}, with student number {StudentNumber}, is {OccupationWithArticle}. " +
        $"{Pronoun} phone number is {Phone}";

    private static string RequireText(string value, string label)
    {
        string trimmed = (value ?? string.Empty).Trim();
        if (trimmed.Length == 0)
        {
            throw new ArgumentException($"{label} cannot be empty.");
        }
        return trimmed;
    }

    private static char NormalizeGender(char gender)
    {
        char upper = char.ToUpperInvariant(gender);
        if (upper != 'M' && upper != 'F')
        {
            throw new ArgumentException("Gender must be M or F.");
        }
        return upper;
    }
}
