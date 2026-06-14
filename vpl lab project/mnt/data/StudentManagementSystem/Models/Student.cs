namespace StudentManagementSystem.Models;

public class Student : Person
{
    public string Department { get; set; } = string.Empty;
    public string Semester { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Id}|{Name}|{Email}|{Phone}|{Department}|{Semester}";
    }

    public static Student FromString(string line)
    {
        string[] p = line.Split('|');
        return new Student
        {
            Id = int.Parse(p[0]),
            Name = p[1],
            Email = p[2],
            Phone = p[3],
            Department = p[4],
            Semester = p[5]
        };
    }
}
