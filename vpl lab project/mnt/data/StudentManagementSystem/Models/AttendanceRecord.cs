namespace StudentManagementSystem.Models;

public class AttendanceRecord
{
    public int StudentId { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public override string ToString() => $"{StudentId}|{Date}|{Status}";
}
