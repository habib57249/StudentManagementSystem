using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services;

public class FileRepository : IRepository
{
    private readonly string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
    private readonly string studentFile;
    private readonly string attendanceFile;
    private readonly string marksFile;

    public FileRepository()
    {
        Directory.CreateDirectory(folderPath);
        studentFile = Path.Combine(folderPath, "students.txt");
        attendanceFile = Path.Combine(folderPath, "attendance.txt");
        marksFile = Path.Combine(folderPath, "marks.txt");
        if (!File.Exists(studentFile)) File.Create(studentFile).Close();
        if (!File.Exists(attendanceFile)) File.Create(attendanceFile).Close();
        if (!File.Exists(marksFile)) File.Create(marksFile).Close();
    }

    public List<Student> GetAllStudents()
    {
        try
        {
            return File.ReadAllLines(studentFile)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(Student.FromString)
                .ToList();
        }
        catch
        {
            return new List<Student>();
        }
    }

    public void SaveStudents(List<Student> students)
    {
        File.WriteAllLines(studentFile, students.Select(s => s.ToString()));
    }

    public int GetNextStudentId()
    {
        List<Student> students = GetAllStudents();
        return students.Count == 0 ? 1 : students.Max(s => s.Id) + 1;
    }

    public void AddAttendance(AttendanceRecord record)
    {
        File.AppendAllLines(attendanceFile, new[] { record.ToString() });
    }

    public List<AttendanceRecord> GetAttendance()
    {
        return File.ReadAllLines(attendanceFile)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line =>
            {
                string[] p = line.Split('|');
                return new AttendanceRecord { StudentId = int.Parse(p[0]), Date = p[1], Status = p[2] };
            }).ToList();
    }

    public void AddMarks(MarksRecord record)
    {
        File.AppendAllLines(marksFile, new[] { record.ToString() });
    }

    public List<MarksRecord> GetMarks()
    {
        return File.ReadAllLines(marksFile)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line =>
            {
                string[] p = line.Split('|');
                return new MarksRecord { StudentId = int.Parse(p[0]), Subject = p[1], Marks = int.Parse(p[2]) };
            }).ToList();
    }
}
