using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRSYSTEM
{
    public class AttendanceView : UserControl
    {
        private string _connString;

        private ComboBox cmbEmployees;
        private DateTimePicker dtpClockTime;
        private DataGridView dgvAttendance;
        private Button btnCheckIn, btnCheckOut;

        public AttendanceView(string connString)
        {
            this._connString = connString;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            InitializeComponent();
            LoadActiveEmployees();
            RefreshAttendanceGrid();
        }

        private void InitializeComponent()
        {
            Font titleFont = new Font("Segoe UI Black", 14F, FontStyle.Bold);
            Font labelFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 10F);

            Label title = new Label { Text = "Attendance Management System", Font = titleFont, Location = new Point(25, 20), AutoSize = true };
            this.Controls.Add(title);

            Panel formCard = new Panel { Location = new Point(25, 75), Size = new Size(1210, 120), BackColor = Color.FromArgb(248, 250, 252), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(formCard);

            // Inputs
            Label lblEmp = new Label { Text = "Select Employee *", Location = new Point(20, 20), Font = labelFont, AutoSize = true };
            cmbEmployees = new ComboBox { Location = new Point(20, 45), Size = new Size(300, 28), Font = inputFont, DropDownStyle = ComboBoxStyle.DropDownList };
            formCard.Controls.Add(lblEmp);
            formCard.Controls.Add(cmbEmployees);

            Label lblTime = new Label { Text = "Stamp Time Override", Location = new Point(350, 20), Font = labelFont, AutoSize = true };
            dtpClockTime = new DateTimePicker { Location = new Point(350, 45), Size = new Size(180, 28), Font = inputFont, Format = DateTimePickerFormat.Time, ShowUpDown = true };
            formCard.Controls.Add(lblTime);
            formCard.Controls.Add(dtpClockTime);

            // Action Buttons
            btnCheckIn = new Button { Text = "Log Check-In (08:00 Standard)", Location = new Point(570, 40), Size = new Size(240, 38), BackColor = Color.FromArgb(34, 197, 94), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = labelFont, Cursor = Cursors.Hand };
            btnCheckIn.FlatAppearance.BorderSize = 0;
            btnCheckIn.Click += BtnCheckIn_Click;
            formCard.Controls.Add(btnCheckIn);

            btnCheckOut = new Button { Text = "Log Afternoon Check-Out", Location = new Point(825, 40), Size = new Size(220, 38), BackColor = Color.FromArgb(59, 130, 246), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = labelFont, Cursor = Cursors.Hand };
            btnCheckOut.FlatAppearance.BorderSize = 0;
            btnCheckOut.Click += BtnCheckOut_Click;
            formCard.Controls.Add(btnCheckOut);

            // Main Ledger History Grid Area View Display Component
            dgvAttendance = new DataGridView
            {
                Location = new Point(25, 215),
                Size = new Size(1210, 500),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvAttendance);
        }

        private void LoadActiveEmployees()
        {
            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT id, CONCAT(employee_code, ' - ', first_name, ' ', last_name) AS display_name FROM employees WHERE status = 'Active'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbEmployees.DataSource = dt;
                    cmbEmployees.DisplayMember = "display_name";
                    cmbEmployees.ValueMember = "id";
                }
                catch (Exception ex) { MessageBox.Show("Failed to parse directory indexing lists: " + ex.Message); }
            }
        }

        private void RefreshAttendanceGrid()
        {
            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT a.id AS `Record ID`, e.employee_code AS `ID Code`, 
                                            CONCAT(e.first_name, ' ', e.last_name) AS `Name`, 
                                            a.work_date AS `Date`, a.check_in AS `Check In`, a.check_out AS `Check Out`, 
                                            IF(a.is_late = 1, 'LATE', 'ON TIME') AS `Status`, 
                                            CONCAT(a.punctuality_score, ' pts') AS `Score Deductions`
                                     FROM attendance a
                                     INNER JOIN employees e ON a.employee_id = e.id
                                     ORDER BY a.work_date DESC, a.check_in DESC";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAttendance.DataSource = dt;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void BtnCheckIn_Click(object sender, EventArgs e)
        {
            if (cmbEmployees.SelectedValue == null) return;

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO attendance (employee_id, work_date, check_in) VALUES (@emp, @date, @time)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@emp", cmbEmployees.SelectedValue);
                        cmd.Parameters.AddWithValue("@date", DateTime.Today.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@time", dtpClockTime.Value.ToString("HH:mm:ss"));
                        cmd.ExecuteNonQuery();
                    }
                    RefreshAttendanceGrid();
                }
                catch (Exception) { MessageBox.Show("Attendance record already exists for this employee today."); }
            }
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            if (cmbEmployees.SelectedValue == null) return;

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE attendance SET check_out = @time WHERE employee_id = @emp AND work_date = @date";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@time", dtpClockTime.Value.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@emp", cmbEmployees.SelectedValue);
                        cmd.Parameters.AddWithValue("@date", DateTime.Today.ToString("yyyy-MM-dd"));

                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0) MessageBox.Show("No active check-in record found for today.");
                    }
                    RefreshAttendanceGrid();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}