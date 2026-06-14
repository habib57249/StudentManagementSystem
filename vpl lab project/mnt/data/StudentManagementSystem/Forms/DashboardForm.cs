using System.Drawing;
using System.Windows.Forms;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Forms;

public class DashboardForm : Form
{
    private readonly FileRepository repo = new FileRepository();
    private readonly DataGridView grid = new DataGridView();
    private readonly TextBox txtName = new TextBox();
    private readonly TextBox txtEmail = new TextBox();
    private readonly TextBox txtPhone = new TextBox();
    private readonly TextBox txtDept = new TextBox();
    private readonly TextBox txtSemester = new TextBox();
    private readonly TextBox txtSearch = new TextBox();
    private readonly Label lblSummary = new Label();
    private int selectedId = 0;

    public DashboardForm()
    {
        Text = "Student Management System";
        AutoScaleMode = AutoScaleMode.None;
        Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        ClientSize = new Size(1120, 680);
        MinimumSize = new Size(1000, 620);
        StartPosition = FormStartPosition.CenterScreen;

        var title = new Label
        {
            Text = "Student Management System",
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 65,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 0, 0, 0)
        };

        var main = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(20, 0, 20, 20)
        };
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 330));
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var leftPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 5, 20, 0) };
        BuildInputPanel(leftPanel);

        var rightPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 5, 0, 0) };
        BuildGridPanel(rightPanel);

        main.Controls.Add(leftPanel, 0, 0);
        main.Controls.Add(rightPanel, 1, 0);

        Controls.Add(main);
        Controls.Add(title);

        LoadStudents();
    }

    private void BuildInputPanel(Panel panel)
    {
        var input = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 2,
            RowCount = 5,
            Height = 190
        };
        input.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        input.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AddInputRow(input, "Name", txtName, 0);
        AddInputRow(input, "Email", txtEmail, 1);
        AddInputRow(input, "Phone", txtPhone, 2);
        AddInputRow(input, "Department", txtDept, 3);
        AddInputRow(input, "Semester", txtSemester, 4);

        var buttons = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 3,
            Height = 45,
            Padding = new Padding(0, 8, 0, 0)
        };
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

        Button btnAdd = MakeButton("Add");
        Button btnUpdate = MakeButton("Update");
        Button btnDelete = MakeButton("Delete");
        buttons.Controls.Add(btnAdd, 0, 0);
        buttons.Controls.Add(btnUpdate, 1, 0);
        buttons.Controls.Add(btnDelete, 2, 0);

        Button btnClear = MakeButton("Clear");
        btnClear.Dock = DockStyle.Top;
        btnClear.Height = 35;

        var twoButtons = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 2,
            Height = 45,
            Padding = new Padding(0, 8, 0, 0)
        };
        twoButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        twoButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        Button btnAttendance = MakeButton("Mark Attendance");
        Button btnMarks = MakeButton("Add Marks");
        twoButtons.Controls.Add(btnAttendance, 0, 0);
        twoButtons.Controls.Add(btnMarks, 1, 0);

        Button btnReport = MakeButton("Show Summary Report");
        btnReport.Dock = DockStyle.Top;
        btnReport.Height = 35;

        btnAdd.Click += BtnAdd_Click;
        btnUpdate.Click += BtnUpdate_Click;
        btnDelete.Click += BtnDelete_Click;
        btnClear.Click += (s, e) => ClearFields();
        btnAttendance.Click += BtnAttendance_Click;
        btnMarks.Click += BtnMarks_Click;
        btnReport.Click += BtnReport_Click;

        panel.Controls.Add(btnReport);
        panel.Controls.Add(twoButtons);
        panel.Controls.Add(btnClear);
        panel.Controls.Add(buttons);
        panel.Controls.Add(input);
    }

    private void BuildGridPanel(Panel panel)
    {
        var searchPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 4,
            Height = 45
        };
        searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
        searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));

        var searchLabel = new Label { Text = "Search", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
        Button btnSearch = MakeButton("Search");
        Button btnRefresh = MakeButton("Refresh");
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Margin = new Padding(3, 8, 8, 8);

        searchPanel.Controls.Add(searchLabel, 0, 0);
        searchPanel.Controls.Add(txtSearch, 1, 0);
        searchPanel.Controls.Add(btnSearch, 2, 0);
        searchPanel.Controls.Add(btnRefresh, 3, 0);

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersWidth = 28;
        grid.CellClick += Grid_CellClick;

        lblSummary.Dock = DockStyle.Bottom;
        lblSummary.Height = 45;
        lblSummary.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblSummary.TextAlign = ContentAlignment.MiddleLeft;

        btnSearch.Click += BtnSearch_Click;
        btnRefresh.Click += (s, e) => LoadStudents();

        panel.Controls.Add(grid);
        panel.Controls.Add(lblSummary);
        panel.Controls.Add(searchPanel);
    }

    private static Button MakeButton(string text)
    {
        return new Button { Text = text, Dock = DockStyle.Fill, Margin = new Padding(3), Height = 32 };
    }

    private static void AddInputRow(TableLayoutPanel table, string labelText, TextBox textBox, int row)
    {
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        var label = new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(3, 5, 3, 5);
        table.Controls.Add(label, 0, row);
        table.Controls.Add(textBox, 1, row);
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace(txtDept.Text) || string.IsNullOrWhiteSpace(txtSemester.Text))
        {
            MessageBox.Show("Please fill all fields.");
            return false;
        }
        if (!txtEmail.Text.Contains("@"))
        {
            MessageBox.Show("Please enter a valid email address.");
            return false;
        }
        return true;
    }

    private void LoadStudents()
    {
        try
        {
            var students = repo.GetAllStudents();
            grid.DataSource = null;
            grid.DataSource = students;
            lblSummary.Text = $"Total Students: {students.Count}";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading records: " + ex.Message);
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!ValidateInput()) return;
            var students = repo.GetAllStudents();
            students.Add(new Student { Id = repo.GetNextStudentId(), Name = txtName.Text, Email = txtEmail.Text, Phone = txtPhone.Text, Department = txtDept.Text, Semester = txtSemester.Text });
            repo.SaveStudents(students);
            LoadStudents();
            ClearFields();
        }
        catch (Exception ex) { MessageBox.Show("Add failed: " + ex.Message); }
    }

    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        try
        {
            if (selectedId == 0) { MessageBox.Show("Select a student first."); return; }
            if (!ValidateInput()) return;
            var students = repo.GetAllStudents();
            var st = students.FirstOrDefault(x => x.Id == selectedId);
            if (st != null)
            {
                st.Name = txtName.Text;
                st.Email = txtEmail.Text;
                st.Phone = txtPhone.Text;
                st.Department = txtDept.Text;
                st.Semester = txtSemester.Text;
                repo.SaveStudents(students);
                LoadStudents();
                ClearFields();
            }
        }
        catch (Exception ex) { MessageBox.Show("Update failed: " + ex.Message); }
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        try
        {
            if (selectedId == 0) { MessageBox.Show("Select a student first."); return; }
            var students = repo.GetAllStudents();
            students.RemoveAll(x => x.Id == selectedId);
            repo.SaveStudents(students);
            LoadStudents();
            ClearFields();
        }
        catch (Exception ex) { MessageBox.Show("Delete failed: " + ex.Message); }
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        string key = txtSearch.Text.ToLower();
        var result = repo.GetAllStudents()
            .Where(s => s.Name.ToLower().Contains(key) || s.Department.ToLower().Contains(key) || s.Email.ToLower().Contains(key))
            .ToList();
        grid.DataSource = null;
        grid.DataSource = result;
        lblSummary.Text = $"Search Results: {result.Count}";
    }

    private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            selectedId = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["Id"].Value);
            txtName.Text = grid.Rows[e.RowIndex].Cells["Name"].Value?.ToString();
            txtEmail.Text = grid.Rows[e.RowIndex].Cells["Email"].Value?.ToString();
            txtPhone.Text = grid.Rows[e.RowIndex].Cells["Phone"].Value?.ToString();
            txtDept.Text = grid.Rows[e.RowIndex].Cells["Department"].Value?.ToString();
            txtSemester.Text = grid.Rows[e.RowIndex].Cells["Semester"].Value?.ToString();
        }
    }

    private void BtnAttendance_Click(object? sender, EventArgs e)
    {
        if (selectedId == 0) { MessageBox.Show("Select a student first."); return; }
        repo.AddAttendance(new AttendanceRecord { StudentId = selectedId, Date = DateTime.Now.ToShortDateString(), Status = "Present" });
        MessageBox.Show("Attendance marked as Present.");
    }

    private void BtnMarks_Click(object? sender, EventArgs e)
    {
        if (selectedId == 0) { MessageBox.Show("Select a student first."); return; }
        string subject = Microsoft.VisualBasic.Interaction.InputBox("Enter Subject", "Marks");
        string marksText = Microsoft.VisualBasic.Interaction.InputBox("Enter Marks", "Marks");
        if (int.TryParse(marksText, out int marks))
        {
            repo.AddMarks(new MarksRecord { StudentId = selectedId, Subject = subject, Marks = marks });
            MessageBox.Show("Marks saved.");
        }
        else
        {
            MessageBox.Show("Invalid marks.");
        }
    }

    private void BtnReport_Click(object? sender, EventArgs e)
    {
        int totalStudents = repo.GetAllStudents().Count;
        int totalAttendance = repo.GetAttendance().Count;
        int totalMarks = repo.GetMarks().Count;
        MessageBox.Show($"Summary Report\n\nTotal Students: {totalStudents}\nAttendance Records: {totalAttendance}\nMarks Records: {totalMarks}", "Report");
    }

    private void ClearFields()
    {
        selectedId = 0;
        txtName.Clear();
        txtEmail.Clear();
        txtPhone.Clear();
        txtDept.Clear();
        txtSemester.Clear();
        txtSearch.Clear();
    }
}
