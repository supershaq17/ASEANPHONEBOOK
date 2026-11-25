namespace AseanPhonebook;

/// <summary>
/// Stores all Student objects in memory (no limit) and answers questions about them.
/// Data is lost when the program exits.
/// </summary>
public class Phonebook
{
    private readonly List<Student> _entries = new();

    public int Count => _entries.Count;

    /// <summary>Adds a student. Returns false if the student number already exists.</summary>
    public bool Add(Student student)
    {
        if (Find(student.StudentNumber) != null)
        {
            return false;
        }
        _entries.Add(student);
        return true;
    }

    /// <summary>Finds the student with this student number, or null if none exists.</summary>
    public Student? Find(string studentNumber) =>
        _entries.FirstOrDefault(s => s.HasStudentNumber(studentNumber));

    /// <summary>True if no OTHER student already uses this student number.</summary>
    public bool IsStudentNumberAvailable(string studentNumber, Student current) =>
        !_entries.Any(s => !ReferenceEquals(s, current) && s.HasStudentNumber(studentNumber));

    /// <summary>Students whose CURRENT country code is in the list, sorted by surname.</summary>
    public List<Student> SearchByCountryCodes(IEnumerable<int> countryCodes)
    {
        HashSet<int> wanted = new(countryCodes);
        return Sort(_entries.Where(s => wanted.Contains(s.Phone.CountryCode)));
    }

    public List<Student> GetAllSorted() => Sort(_entries);

    private static List<Student> Sort(IEnumerable<Student> students) =>
        students
            .OrderBy(s => s.Surname, StringComparer.OrdinalIgnoreCase)
            .ThenBy(s => s.FirstName, StringComparer.OrdinalIgnoreCase)
            .ToList();
}
