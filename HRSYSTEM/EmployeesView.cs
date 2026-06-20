using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRSYSTEM
{
    public class EmployeesView : UserControl
    {
        private string _connString;

        private DataGridView dataGridView1;
        private Panel panel1;

        private TextBox txtFirstName, txtLastName, txtEmail, txtPhone;
        private DateTimePicker dtpHireDate;
        private ComboBox cmbDepartment, cmbStatus;
        private Button btnCreate, btnUpdate, btnDelete, btnClear;
        private int selectedEmployeeId = -1;

        public EmployeesView(string connectionString)
        {
            this._connString = connectionString;
            InitializeComponent();
            SetupResponsiveFormLayout();
            LoadDepartments();
            LoadStatuses();
            RefreshDataGrid();
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1 (Inputs Card View Wrapper)
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.FromArgb(248, 250, 252);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1260, 240);
            // 
            // dataGridView1 (Data Listing Panel)
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 260);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1260, 485);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += DataGridView1_CellClick;
            // 
            // EmployeesView
            // 
            this.BackColor = Color.White;
            Controls.Add(panel1);
            Controls.Add(dataGridView1);
            Name = "EmployeesView";
            Size = new Size(1260, 745);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SetupResponsiveFormLayout()
        {
            Font lblFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 10F);

            // Left Column (X = 30)
            CreateLabel("First Name *", new Point(30, 20), lblFont);
            txtFirstName = CreateTextBox(new Point(30, 45), new Size(280, 28), inputFont);

            CreateLabel("Email Address *", new Point(30, 90), lblFont);
            txtEmail = CreateTextBox(new Point(30, 115), new Size(280, 28), inputFont);

            CreateLabel("Department", new Point(30, 160), lblFont);
            cmbDepartment = new ComboBox { Location = new Point(30, 185), Size = new Size(280, 28), Font = inputFont, DropDownStyle = ComboBoxStyle.DropDownList };
            panel1.Controls.Add(cmbDepartment);

            // Center Column (X = 360)
            CreateLabel("Last Name", new Point(360, 20), lblFont);
            txtLastName = CreateTextBox(new Point(360, 45), new Size(280, 28), inputFont);

            CreateLabel("Phone Number", new Point(360, 90), lblFont);
            txtPhone = CreateTextBox(new Point(360, 115), new Size(280, 28), inputFont);

            CreateLabel("Employment Status", new Point(360, 160), lblFont);
            cmbStatus = new ComboBox { Location = new Point(360, 185), Size = new Size(280, 28), Font = inputFont, DropDownStyle = ComboBoxStyle.DropDownList };
            panel1.Controls.Add(cmbStatus);

            // Right Column (X = 690)
            CreateLabel("Official Hire Date", new Point(690, 20), lblFont);
            dtpHireDate = new DateTimePicker { Location = new Point(690, 45), Size = new Size(250, 28), Font = inputFont, Format = DateTimePickerFormat.Short };
            panel1.Controls.Add(dtpHireDate);

            // Action Button Matrix (Aligned to the Right Edge of the top card container)
            btnCreate = CreateButton("Save Record", new Point(1020, 25), Color.FromArgb(34, 197, 94), btnCreate_Click);
            btnUpdate = CreateButton("Modify Selected", new Point(1020, 75), Color.FromArgb(59, 130, 246), btnUpdate_Click);
            btnDelete = CreateButton("Purge Employee", new Point(1020, 125), Color.FromArgb(239, 68, 68), btnDelete_Click);
            btnClear = CreateButton("Clear View", new Point(1020, 175), Color.FromArgb(148, 163, 184), btnClear_Click);
        }

        #region Functional Generators & Utility Blocks
        private void CreateLabel(string text, Point location, Font font) =>
            panel1.Controls.Add(new Label { Text = text, Location = location, Font = font, AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) });

        private TextBox CreateTextBox(Point location, Size size, Font font)
        {
            TextBox t = new TextBox { Location = location, Size = size, Font = font };
            panel1.Controls.Add(t);
            return t;
        }

        private Button CreateButton(string text, Point location, Color bg, EventHandler evt)
        {
            Button b = new Button { Text = text, Location = location, Size = new Size(200, 38), BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 10F), Cursor = Cursors.Hand, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            b.FlatAppearance.BorderSize = 0; b.Click += evt;
            panel1.Controls.Add(b);
            return b;
        }

        private void LoadDepartments()
        {
            using (MySqlConnection c = new MySqlConnection(_connString))
            {
                try
                {
                    c.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter("SELECT id, name FROM departments ORDER BY name ASC", c);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    cmbDepartment.DataSource = dt; cmbDepartment.DisplayMember = "name"; cmbDepartment.ValueMember = "id";
                }
                catch { /* Silent handle errors */ }
            }
        }

        private void LoadStatuses() { cmbStatus.Items.AddRange(new string[] { "Active", "Terminated", "On Leave" }); cmbStatus.SelectedIndex = 0; }

        private void RefreshDataGrid()
        {
            using (MySqlConnection c = new MySqlConnection(_connString))
            {
                try
                {
                    c.Open();
                    string query = @"SELECT e.id AS `ID`, e.first_name AS `First Name`, e.last_name AS `Last Name`, 
                                            e.email AS `Email`, e.phone AS `Phone`, e.hire_date AS `Hire Date`, 
                                            d.name AS `Department`, e.status AS `Status`, e.department_id
                                     FROM employees e LEFT JOIN departments d ON e.department_id = d.id ORDER BY e.id DESC";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    if (dataGridView1.Columns["department_id"] != null) dataGridView1.Columns["department_id"].Visible = false;
                }
                catch (Exception ex) { MessageBox.Show("Data execution error: " + ex.Message); }
            }
        }
        #endregion

        #region CRUD Action Events
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill out all fields marked with an asterisk (*)."); return;
            }
            using (MySqlConnection c = new MySqlConnection(_connString))
            {
                try
                {
                    c.Open();
                    string sql = "INSERT INTO employees (first_name, last_name, email, phone, hire_date, department_id, status) VALUES (@f, @l, @e, @p, @h, @d, @s)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, c))
                    {
                        cmd.Parameters.AddWithValue("@f", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@l", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@p", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@h", dtpHireDate.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@d", cmbDepartment.SelectedValue);
                        cmd.Parameters.AddWithValue("@s", cmbStatus.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                    }
                    ClearFields(); RefreshDataGrid();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedEmployeeId == -1) return;
            using (MySqlConnection c = new MySqlConnection(_connString))
            {
                try
                {
                    c.Open();
                    string sql = "UPDATE employees SET first_name=@f, last_name=@l, email=@e, phone=@p, hire_date=@h, department_id=@d, status=@s WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, c))
                    {
                        cmd.Parameters.AddWithValue("@f", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@l", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@p", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@h", dtpHireDate.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@d", cmbDepartment.SelectedValue);
                        cmd.Parameters.AddWithValue("@s", cmbStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@id", selectedEmployeeId);
                        cmd.ExecuteNonQuery();
                    }
                    ClearFields(); RefreshDataGrid();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedEmployeeId == -1) return;
            var res = MessageBox.Show("Are you sure you want to delete this employee record?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.No) return;

            using (MySqlConnection c = new MySqlConnection(_connString))
            {
                try
                {
                    c.Open();
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM employees WHERE id=@id", c))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedEmployeeId);
                        cmd.ExecuteNonQuery();
                    }
                    ClearFields(); RefreshDataGrid();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearFields();

        private void ClearFields()
        {
            txtFirstName.Clear(); txtLastName.Clear(); txtEmail.Clear(); txtPhone.Clear();
            dtpHireDate.Value = DateTime.Now; if (cmbDepartment.Items.Count > 0) cmbDepartment.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0; selectedEmployeeId = -1; btnCreate.Enabled = true;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow r = dataGridView1.Rows[e.RowIndex];
                selectedEmployeeId = Convert.ToInt32(r.Cells["ID"].Value);
                txtFirstName.Text = r.Cells["First Name"].Value.ToString();
                txtLastName.Text = r.Cells["Last Name"].Value.ToString();
                txtEmail.Text = r.Cells["Email"].Value.ToString();
                txtPhone.Text = r.Cells["Phone"].Value != DBNull.Value ? r.Cells["Phone"].Value.ToString() : "";
                dtpHireDate.Value = Convert.ToDateTime(r.Cells["Hire Date"].Value);
                cmbStatus.SelectedItem = r.Cells["Status"].Value.ToString();
                if (r.Cells["department_id"].Value != DBNull.Value) cmbDepartment.SelectedValue = r.Cells["department_id"].Value;
                btnCreate.Enabled = false;
            }
        }
        #endregion
    }
}