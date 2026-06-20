using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRSYSTEM
{
    public class PayrollView : UserControl
    {
        private readonly string _connString;

        private ComboBox cmbEmployees;
        private DateTimePicker dtpMonth;
        private NumericUpDown numBaseSalary, numBonuses, numDeductions;
        private ComboBox cmbStatus;
        private DataGridView dgvPayroll;
        private Button btnProcess, btnUpdateStatus, btnClear;
        private int selectedPayrollId = -1;

        public PayrollView(string connString)
        {
            this._connString = connString;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            InitializeComponent();
            SetupLayoutForm();
            LoadActiveEmployees();
            RefreshPayrollGrid();
        }

        private void InitializeComponent()
        {
            dgvPayroll = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvPayroll).BeginInit();
            SuspendLayout();

            // Data Grid View Configuration
            dgvPayroll.AllowUserToAddRows = false;
            dgvPayroll.AllowUserToDeleteRows = false;
            dgvPayroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPayroll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayroll.BackgroundColor = Color.White;
            dgvPayroll.BorderStyle = BorderStyle.None;
            dgvPayroll.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPayroll.Location = new Point(0, 260);
            dgvPayroll.MultiSelect = false;
            dgvPayroll.Name = "dgvPayroll";
            dgvPayroll.ReadOnly = true;
            dgvPayroll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayroll.Size = new Size(1260, 485);
            dgvPayroll.CellClick += DgvPayroll_CellClick;

            Controls.Add(dgvPayroll);
            Size = new Size(1260, 745);
            ((System.ComponentModel.ISupportInitialize)dgvPayroll).EndInit();
            ResumeLayout(false);
        }

        private void SetupLayoutForm()
        {
            Panel cardPanel = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 0),
                Size = new Size(1260, 240)
            };
            Controls.Add(cardPanel);

            Font lblFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 10F);

            // Column 1
            CreateLabel(cardPanel, "Select Employee *", new Point(30, 20), lblFont);
            cmbEmployees = new ComboBox { Location = new Point(30, 45), Size = new Size(280, 28), Font = inputFont, DropDownStyle = ComboBoxStyle.DropDownList };
            cardPanel.Controls.Add(cmbEmployees);

            CreateLabel(cardPanel, "Salary Month *", new Point(30, 90), lblFont);
            dtpMonth = new DateTimePicker { Location = new Point(30, 115), Size = new Size(280, 28), Font = inputFont, CustomFormat = "MMMM yyyy", Format = DateTimePickerFormat.Custom };
            cardPanel.Controls.Add(dtpMonth);

            CreateLabel(cardPanel, "Payment Status", new Point(30, 160), lblFont);
            cmbStatus = new ComboBox { Location = new Point(30, 185), Size = new Size(280, 28), Font = inputFont, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new string[] { "Pending", "Processed", "Failed" });
            cmbStatus.SelectedIndex = 0;
            cardPanel.Controls.Add(cmbStatus);

            // Column 2
            CreateLabel(cardPanel, "Base Salary ($) *", new Point(360, 20), lblFont);
            numBaseSalary = CreateNumericUpDown(cardPanel, new Point(360, 45), new Size(280, 28), inputFont, 0, 100000);

            CreateLabel(cardPanel, "Bonuses ($)", new Point(360, 90), lblFont);
            numBonuses = CreateNumericUpDown(cardPanel, new Point(360, 115), new Size(280, 28), inputFont, 0, 50000);

            CreateLabel(cardPanel, "Deductions ($)", new Point(360, 160), lblFont);
            numDeductions = CreateNumericUpDown(cardPanel, new Point(360, 185), new Size(280, 28), inputFont, 0, 50000);

            // Column 3 Actions
            btnProcess = CreateButton(cardPanel, "Process New Payroll", new Point(1020, 30), Color.FromArgb(34, 197, 94), BtnProcess_Click);
            btnUpdateStatus = CreateButton(cardPanel, "Update Status", new Point(1020, 90), Color.FromArgb(59, 130, 246), BtnUpdateStatus_Click);
            btnClear = CreateButton(cardPanel, "Reset Fields", new Point(1020, 150), Color.FromArgb(148, 163, 184), BtnClear_Click);
        }

        #region Helpers & UI Generators
        private void CreateLabel(Panel p, string text, Point loc, Font f) =>
            p.Controls.Add(new Label { Text = text, Location = loc, Font = f, AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) });

        private NumericUpDown CreateNumericUpDown(Panel p, Point loc, Size s, Font f, decimal min, decimal max)
        {
            NumericUpDown n = new NumericUpDown { Location = loc, Size = s, Font = f, Minimum = min, Maximum = max, DecimalPlaces = 2, ThousandsSeparator = true };
            p.Controls.Add(n);
            return n;
        }

        private Button CreateButton(Panel p, string text, Point loc, Color bg, EventHandler evt)
        {
            Button b = new Button { Text = text, Location = loc, Size = new Size(200, 40), BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 10F), Cursor = Cursors.Hand, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            b.FlatAppearance.BorderSize = 0; b.Click += evt;
            p.Controls.Add(b);
            return b;
        }

        private void LoadActiveEmployees()
        {
            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter("SELECT id, CONCAT(employee_code, ' - ', first_name, ' ', last_name) AS full_name FROM employees WHERE status = 'Active'", conn);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    cmbEmployees.DataSource = dt; cmbEmployees.DisplayMember = "full_name"; cmbEmployees.ValueMember = "id";
                }
                catch { }
            }
        }

        private void RefreshPayrollGrid()
        {
            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT p.id AS `ID`, e.employee_code AS `Emp Code`, CONCAT(e.first_name, ' ', e.last_name) AS `Employee`, 
                                   DATE_FORMAT(p.salary_month, '%Y-%m') AS `Month`, p.base_salary AS `Base ($)`, 
                                   p.bonuses AS `Bonuses ($)`, p.deductions AS `Deductions ($)`, p.net_pay AS `Net Pay ($)`, 
                                   p.payment_status AS `Status`, p.payment_date AS `Paid Date`
                                   FROM payroll p INNER JOIN employees e ON p.employee_id = e.id ORDER BY p.salary_month DESC";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    dgvPayroll.DataSource = dt;
                }
                catch (Exception ex) { MessageBox.Show("Error loading payroll dashboard: " + ex.Message); }
            }
        }
        #endregion

        #region Operations Logic
        private void BtnProcess_Click(object sender, EventArgs e)
        {
            if (cmbEmployees.SelectedValue == null) return;

            // Format to first day of the target month as mandated by database unique constraint architecture
            DateTime targetMonth = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string sql = @"INSERT INTO payroll (employee_id, salary_month, base_salary, bonuses, deductions, payment_status, payment_date) 
                                   VALUES (@emp, @month, @base, @bonus, @deduct, @status, @pdate)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@emp", cmbEmployees.SelectedValue);
                        cmd.Parameters.AddWithValue("@month", targetMonth.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@base", numBaseSalary.Value);
                        cmd.Parameters.AddWithValue("@bonus", numBonuses.Value);
                        cmd.Parameters.AddWithValue("@deduct", numDeductions.Value);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@pdate", cmbStatus.SelectedItem.ToString() == "Processed" ? (object)DateTime.Now : DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    BtnClear_Click(null, null); RefreshPayrollGrid();
                }
                catch (Exception) { MessageBox.Show("Payroll ledger record already exists for this employee in the selected target month context."); }
            }
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (selectedPayrollId == -1) { MessageBox.Show("Please select a ledger item from the data grid table down below first."); return; }

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE payroll SET payment_status = @status, payment_date = @pdate WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        string status = cmbStatus.SelectedItem.ToString();
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@pdate", status == "Processed" ? (object)DateTime.Now : DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", selectedPayrollId);
                        cmd.ExecuteNonQuery();
                    }
                    BtnClear_Click(null, null); RefreshPayrollGrid();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void DgvPayroll_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow r = dgvPayroll.Rows[e.RowIndex];
                selectedPayrollId = Convert.ToInt32(r.Cells["ID"].Value);
                cmbEmployees.Text = r.Cells["Employee"].Value.ToString();
                numBaseSalary.Value = Convert.ToDecimal(r.Cells["Base ($)"].Value);
                numBonuses.Value = Convert.ToDecimal(r.Cells["Bonuses ($)"].Value);
                numDeductions.Value = Convert.ToDecimal(r.Cells["Deductions ($)"].Value);
                cmbStatus.SelectedItem = r.Cells["Status"].Value.ToString();
                btnProcess.Enabled = false;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            selectedPayrollId = -1; numBaseSalary.Value = 0; numBonuses.Value = 0; numDeductions.Value = 0;
            cmbStatus.SelectedIndex = 0; btnProcess.Enabled = true;
        }
        #endregion
    }
}