using System.Collections.Generic;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services;

public interface IRepository
{
    List<Student> GetAllStudents();
    void SaveStudents(List<Student> students);
    int GetNextStudentId();
}
