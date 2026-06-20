using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRSYSTEM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string rawPassword = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(rawPassword))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DatabaseHelper db = new DatabaseHelper();

            string query = @"SELECT u.id, u.employee_id, u.username, u.email, u.password_hash, u.role, u.last_login,
                                    e.first_name, e.last_name
                             FROM users u
                             LEFT JOIN employees e ON u.employee_id = e.id
                             WHERE u.email = @Email
                               AND u.status = 'Active'
                             LIMIT 1";

            MySqlParameter[] parameters = {
                new MySqlParameter("@Email", email)
            };

            DataTable? result = db.ExecuteQuery(query, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                DataRow userRow = result.Rows[0];
                string storedHash = Convert.ToString(userRow["password_hash"]) ?? string.Empty;

                if (VerifyPassword(rawPassword, storedHash))
                {
                    int userId = Convert.ToInt32(userRow["id"]);
                    string userRole = Convert.ToString(userRow["role"]) ?? string.Empty;

                    UpdateLastLogin(db, userId);

                    UserSession session = new UserSession
                    {
                        UserId = userId,
                        EmployeeId = userRow["employee_id"] == DBNull.Value ? null : Convert.ToInt32(userRow["employee_id"]),
                        Username = Convert.ToString(userRow["username"]) ?? string.Empty,
                        Email = Convert.ToString(userRow["email"]) ?? string.Empty,
                        FirstName = Convert.ToString(userRow["first_name"]) ?? string.Empty,
                        LastName = Convert.ToString(userRow["last_name"]) ?? string.Empty,
                        Role = userRole,
                        LastLogin = userRow["last_login"] == DBNull.Value ? null : Convert.ToDateTime(userRow["last_login"])
                    };

                    DashboardForm dashboard = new DashboardForm(session);
                    dashboard.FormClosed += (_, _) => Close();
                    dashboard.Show();
                    Hide();
                    return;
                }
            }

            ShowInvalidCredentialsMessage();
        }

        private static bool VerifyPassword(string rawPassword, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(rawPassword, storedHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static void UpdateLastLogin(DatabaseHelper db, int userId)
        {
            string query = "UPDATE users SET last_login = NOW() WHERE id = @UserId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@UserId", userId)
            };

            db.ExecuteNonQuery(query, parameters);
        }

        private void ShowInvalidCredentialsMessage()
        {
            MessageBox.Show("Invalid email or password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
