using System;
using System.Windows.Forms;
using StudentManagementSystem.Forms;

namespace StudentManagementSystem;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}
