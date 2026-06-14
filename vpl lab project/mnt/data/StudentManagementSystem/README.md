# Student Management System

Desktop-based Visual Programming Language project using C# WinForms.

## Login
Username: `admin`
Password: `1234`

## Features
- Login form
- Dashboard
- Add, view, update, delete student records
- Search students by name, email, or department
- Attendance marking
- Marks entry
- Summary report
- File handling storage
- OOP: classes, inheritance, abstraction/interface, encapsulation
- Exception handling with try-catch

## Tools
- Visual Studio 2022
- C#
- .NET 8 Windows
- WinForms

## How to Run
1. Install Visual Studio 2022.
2. During installation, select `.NET desktop development` workload.
3. Open `StudentManagementSystem.csproj` in Visual Studio.
4. Click `Start` or press `F5`.
5. Login with admin / 1234.

## Data Storage
Data is stored automatically in the `Data` folder inside the application output directory.

## Project Requirement Mapping
- GUI: WinForms forms and controls
- CRUD: Add, View, Update, Delete students
- Search: Search textbox and search button
- OOP: Person, Student, AttendanceRecord, MarksRecord, IRepository
- Exception Handling: try-catch blocks
- File Handling: students.txt, attendance.txt, marks.txt
- Report: Summary report button
