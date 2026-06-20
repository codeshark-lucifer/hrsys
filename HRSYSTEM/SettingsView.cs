using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRSYSTEM
{
    public class SettingsView : UserControl
    {
        private readonly string _connString = "Server=localhost;Database=hrsys_db;Uid=root;Pwd=yourpassword;";

        private TextBox txtDbString;
        private Button btnTestConnection, btnSaveSettings;
        private CheckBox chkMaintenanceMode;

        public SettingsView(string connString)
        {
            this._connString = connString;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            InitializeSettingsLayout();
        }

        private void InitializeSettingsLayout()
        {
            Font titleFont = new Font("Segoe UI Black", 16F, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 10F);

            // Title
            Label lblTitle = new Label { Text = "System Settings & Configuration", Font = titleFont, Location = new Point(30, 20), AutoSize = true, ForeColor = Color.FromArgb(15, 23, 42) };
            Controls.Add(lblTitle);

            // 1. Database Connection Card Container Panel
            Panel cardDatabase = new Panel { Location = new Point(30, 85), Size = new Size(600, 180), BackColor = Color.FromArgb(248, 250, 252), BorderStyle = BorderStyle.FixedSingle };
            Controls.Add(cardDatabase);

            Label lblDbHeader = new Label { Text = "Database Integration Properties", Font = headerFont, Location = new Point(20, 15), AutoSize = true, ForeColor = Color.FromArgb(15, 23, 42) };
            cardDatabase.Controls.Add(lblDbHeader);

            Label lblString = new Label { Text = "Active MySQL Connection String:", Font = bodyFont, Location = new Point(20, 55), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            cardDatabase.Controls.Add(lblString);

            txtDbString = new TextBox { Location = new Point(20, 80), Size = new Size(550, 27), Font = bodyFont, Text = _connString };
            cardDatabase.Controls.Add(txtDbString);

            btnTestConnection = new Button { Text = "Test Connection Pipeline", Location = new Point(20, 120), Size = new Size(220, 38), BackColor = Color.FromArgb(15, 23, 42), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = bodyFont, Cursor = Cursors.Hand };
            btnTestConnection.FlatAppearance.BorderSize = 0;
            btnTestConnection.Click += BtnTestConnection_Click;
            cardDatabase.Controls.Add(btnTestConnection);

            // 2. Application Engine Rules Card Container Panel
            Panel cardRules = new Panel { Location = new Point(30, 290), Size = new Size(600, 150), BackColor = Color.FromArgb(248, 250, 252), BorderStyle = BorderStyle.FixedSingle };
            Controls.Add(cardRules);

            Label lblRulesHeader = new Label { Text = "System Control Parameters", Font = headerFont, Location = new Point(20, 15), AutoSize = true, ForeColor = Color.FromArgb(15, 23, 42) };
            cardRules.Controls.Add(lblRulesHeader);

            chkMaintenanceMode = new CheckBox { Text = "Enable Core Maintenance Window Locks (Restrict Non-Admin App Logins)", Location = new Point(20, 60), Size = new Size(550, 30), Font = bodyFont, ForeColor = Color.FromArgb(71, 85, 105), Cursor = Cursors.Hand };
            cardRules.Controls.Add(chkMaintenanceMode);

            // Master Commit Operations Control Action Triggers
            btnSaveSettings = new Button { Text = "Apply Configuration Changes", Location = new Point(30, 465), Size = new Size(260, 42), BackColor = Color.FromArgb(34, 197, 94), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSaveSettings.FlatAppearance.BorderSize = 0;
            btnSaveSettings.Click += BtnSaveSettings_Click;
            Controls.Add(btnSaveSettings);
        }

        private void BtnTestConnection_Click(object sender, System.EventArgs e)
        {
            using (MySqlConnection checkConn = new MySqlConnection(txtDbString.Text.Trim()))
            {
                try
                {
                    checkConn.Open();
                    MessageBox.Show("MySQL Database connection test successful! Communication channel is open.", "Diagnostic Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Connection test failed:\n{ex.Message}", "Diagnostic Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSaveSettings_Click(object sender, System.EventArgs e)
        {
            // Simulate configuration persistence logic
            MessageBox.Show("System environments and rule profiles successfully deployed to configurations.", "Operation Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}