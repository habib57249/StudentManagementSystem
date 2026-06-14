using System.Drawing;
using System.Windows.Forms;

namespace StudentManagementSystem.Forms;

public class LoginForm : Form
{
    TextBox txtUser = new TextBox();
    TextBox txtPass = new TextBox();
    Button btnLogin = new Button();

    public LoginForm()
    {
        Text = "Login - Student Management System";
        AutoScaleMode = AutoScaleMode.None;
        Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        Size = new Size(430, 280);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        Label title = new Label { Text = "Student Management System", Font = new Font("Arial", 16, FontStyle.Bold), AutoSize = true, Location = new Point(65, 25) };
        Label lblUser = new Label { Text = "Username", Location = new Point(55, 85), AutoSize = true };
        Label lblPass = new Label { Text = "Password", Location = new Point(55, 125), AutoSize = true };

        txtUser.Location = new Point(150, 82); txtUser.Width = 190;
        txtPass.Location = new Point(150, 122); txtPass.Width = 190; txtPass.PasswordChar = '*';
        btnLogin.Text = "Login"; btnLogin.Location = new Point(150, 165); btnLogin.Width = 190;
        btnLogin.Click += BtnLogin_Click;

        Controls.AddRange(new Control[] { title, lblUser, lblPass, txtUser, txtPass, btnLogin });
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        if (txtUser.Text == "admin" && txtPass.Text == "1234")
        {
            Hide();
            new DashboardForm().ShowDialog();
            Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password. Use admin / 1234", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
