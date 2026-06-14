namespace StudentManagementSystem.Models;

public class MarksRecord
{
    public int StudentId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public int Marks { get; set; }

    public override string ToString() => $"{StudentId}|{Subject}|{Marks}";
}
