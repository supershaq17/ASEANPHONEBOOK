namespace AseanPhonebook;

/// <summary>
/// The user interface: shows the menus and coordinates the Phonebook, Student and ConsoleInput classes.
/// </summary>
public class PhonebookApp
{
    private readonly Phonebook _phonebook;

    public PhonebookApp(Phonebook phonebook)
    {
        _phonebook = phonebook;
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            ShowMainMenu();
            switch (ConsoleInput.ReadMenuChoice("Enter choice: "))
            {
                case 1: StoreEntries(); break;
                case 2: EditEntry(); break;
                case 3: SearchByCountry(); break;
                case 4:
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose from 1 to 4.");
                    break;
            }
        }
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine();
        Console.WriteLine("[1] Store to ASEAN phonebook");
        Console.WriteLine("[2] Edit entry in ASEAN phonebook");
        Console.WriteLine("[3] Search ASEAN phonebook by country");
        Console.WriteLine("[4] Exit");
    }

    // ---------------------------------------------------------------- STORE

    private void StoreEntries()
    {
        bool another;
        do
        {
            Console.WriteLine();
            Student student = ReadStudent();
            _phonebook.Add(student);
            Console.WriteLine($"Saved: {student.ToEditSummary()}");
            another = ConsoleInput.ReadYesNo("Do you want to enter another entry [Y/N]? ");
        }
        while (another);
    }

    private Student ReadStudent()
    {
        string studentNumber = ReadUniqueStudentNumber();
        string surname = ConsoleInput.ReadRequired("Enter surname: ");
        string firstName = ConsoleInput.ReadRequired("Enter first name: ");
        string occupation = ConsoleInput.ReadRequired("Enter occupation: ");
        char gender = ConsoleInput.ReadGender("Enter gender (M for male, F for female): ");
        int countryCode = ConsoleInput.ReadCountryCode("Enter country code: ");
        string areaCode = ConsoleInput.ReadDigits("Enter area code: ", "Area code");
        string number = ConsoleInput.ReadDigits("Enter number: ", "Phone number");

        PhoneNumber phone = new PhoneNumber(countryCode, areaCode, number);
        return new Student(studentNumber, surname, firstName, occupation, gender, phone);
    }

    private string ReadUniqueStudentNumber()
    {
        while (true)
        {
            string studentNumber = ConsoleInput.ReadRequired("Enter student number: ");
            if (_phonebook.Find(studentNumber) == null)
            {
                return studentNumber;
            }
            Console.WriteLine("That student number already exists. Please enter a different one.");
        }
    }

    // ----------------------------------------------------------------- EDIT

    private void EditEntry()
    {
        Console.WriteLine();
        string studentNumber = ConsoleInput.ReadRequired("Enter student number: ");
        Student? student = _phonebook.Find(studentNumber);

        if (student == null)
        {
            Console.WriteLine($"Error: student number {studentNumber} does not exist.");
            return;
        }

        bool editing = true;
        while (editing)
        {
            ShowEditMenu(student);
            switch (ConsoleInput.ReadMenuChoice("Enter choice: "))
            {
                case 1: EditStudentNumber(student); break;
                case 2: student.ChangeSurname(ConsoleInput.ReadRequired("Enter new surname: ")); break;
                case 3: student.ChangeGender(ConsoleInput.ReadGender("Enter new gender (M for male, F for female): ")); break;
                case 4: student.ChangeOccupation(ConsoleInput.ReadRequired("Enter new occupation: ")); break;
                case 5: student.Phone.ChangeCountryCode(ConsoleInput.ReadCountryCode("Enter new country code: ")); break;
                case 6: student.Phone.ChangeAreaCode(ConsoleInput.ReadDigits("Enter new area code: ", "Area code")); break;
                case 7: student.Phone.ChangeNumber(ConsoleInput.ReadDigits("Enter new phone number: ", "Phone number")); break;
                case 8: editing = false; break;
                default: Console.WriteLine("Invalid choice. Please choose from 1 to 8."); break;
            }
        }
    }

    private static void ShowEditMenu(Student student)
    {
        Console.WriteLine();
        Console.WriteLine($"Here is the existing information about {student.StudentNumber}:");
        Console.WriteLine(student.ToEditSummary());
        Console.WriteLine("Which of the following information do you wish to change?");
        Console.WriteLine("[1] Student number [2] Surname [3] Gender [4] Occupation");
        Console.WriteLine("[5] Country code [6] Area code [7] Phone number");
        Console.WriteLine("[8] None - Go back to main menu");
    }

    private void EditStudentNumber(Student student)
    {
        string newNumber = ConsoleInput.ReadRequired("Enter new student number: ");
        if (_phonebook.IsStudentNumberAvailable(newNumber, student))
        {
            student.ChangeStudentNumber(newNumber);
        }
        else
        {
            Console.WriteLine("Error: that student number is already used by another entry.");
        }
    }

    // --------------------------------------------------------------- SEARCH

    private void SearchByCountry()
    {
        Console.WriteLine();
        ShowCountryMenu();

        List<Country> selected = new();
        bool selectAll = false;
        bool done = false;

        while (!done && selected.Count < CountryRegistry.All.Count)
        {
            int choice = ConsoleInput.ReadMenuChoice($"Enter choice {selected.Count + 1}: ");

            if (choice == 0)
            {
                done = true;
            }
            else if (choice == 6)
            {
                selectAll = true;
                done = true;
            }
            else if (choice >= 1 && choice <= CountryRegistry.All.Count)
            {
                Country country = CountryRegistry.All[choice - 1];
                if (selected.Contains(country))
                {
                    Console.WriteLine($"{country.Name} is already selected.");
                }
                else
                {
                    selected.Add(country);
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please choose 0 to 6.");
            }
        }

        if (!selectAll && selected.Count == 0)
        {
            Console.WriteLine("No country selected.");
            return;
        }

        List<Student> results;
        string label;
        if (selectAll)
        {
            results = _phonebook.GetAllSorted();
            label = "all countries";
        }
        else
        {
            results = _phonebook.SearchByCountryCodes(selected.Select(c => c.Code));
            label = JoinNames(selected.Select(c => c.Name).ToList());
        }

        Console.WriteLine();
        Console.WriteLine($"Here are the students from {label}:");
        if (results.Count == 0)
        {
            Console.WriteLine("No students found.");
        }
        foreach (Student student in results)
        {
            Console.WriteLine(student.ToSearchSummary());
        }
    }

    private static void ShowCountryMenu()
    {
        Console.WriteLine("From which country:");
        Console.WriteLine("[1] Philippines [2] Thailand [3] Singapore [4] Indonesia");
        Console.WriteLine("[5] Malaysia [6] ALL [0] No More");
    }

    private static string JoinNames(List<string> names)
    {
        if (names.Count == 1) return names[0];
        return string.Join(", ", names.Take(names.Count - 1)) + " and " + names[^1];
    }
}
