using System;
using System.Drawing;
using System.Windows.Forms;

namespace HRSYSTEM
{
    public class DashboardForm : Form
    {
        private readonly DatabaseHelper db = new DatabaseHelper();
        private Panel panel1; // Content wrapper
        private Panel panel2; // Sidebar wrapper
        private Panel panel5; // Header bar
        private Panel panel4; // Scrollable nav panel
        private Panel panel3; // Brand logo bar
        private Label label2;
        private Label label3;
        private UserSession _session;
        private Label label1;
        private Panel contentBodyPanel;

        public DashboardForm(UserSession session)
        {
            this._session = session;
            this.Text = "HR System | CODESHARK";
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();
            SetupNavigationButtons();

            // Safe fallback mapping if DisplayName property changes
            label1.Text = $"Hello, {_session.FirstName} {_session.LastName}!";

            // Show default view on load
            SwitchView(new EmployeesView(db.connection));
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            panel1 = new Panel();
            contentBodyPanel = new Panel();
            panel5 = new Panel();
            label3 = new Label();
            panel2 = new Panel();
            panel4 = new Panel();
            panel3 = new Panel();
            label2 = new Label();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel2 (SIDEBAR CONTAINER - DOCKED LEFT)
            // 
            panel2.BackColor = Color.FromArgb(15, 23, 42); // Deep Slate Gray
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(240, 864); // Widened slightly for better breathing room
            panel2.TabIndex = 0;
            // 
            // panel3 (Sidebar Header Logo Area)
            // 
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(240, 80);
            panel3.TabIndex = 1;
            // 
            // label2 (Brand Text)
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(25, 25);
            label2.Name = "label2";
            label2.Size = new Size(153, 32);
            label2.Text = "CODESHARK";
            // 
            // panel4 (NAVIGATION LINKS LIST)
            // 
            panel4.AutoScroll = true;
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 80);
            panel4.Name = "panel4";
            panel4.Size = new Size(240, 784);
            panel4.TabIndex = 0;
            // 
            // panel1 (MAIN CONTENT WRAPPER - STRETCHES DYNAMICALLY)
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(contentBodyPanel);
            panel1.Controls.Add(panel5);
            panel1.Location = new Point(255, 15);
            panel1.Name = "panel1";
            panel1.Size = new Size(1260, 835); // Takes full remainder of 1536x864 resolution
            panel1.TabIndex = 1;
            // 
            // panel5 (Top Dashboard Greeting Header)
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel5.BackColor = Color.FromArgb(241, 245, 249);
            panel5.Controls.Add(label3);
            panel5.Controls.Add(label1);
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(1260, 75);
            panel5.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(15, 23, 42);
            label1.Location = new Point(20, 14);
            label1.Name = "label1";
            label1.Size = new Size(122, 25);
            label1.Text = "Hello, {user}!";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.5F);
            label3.ForeColor = Color.FromArgb(100, 116, 139);
            label3.Location = new Point(20, 42);
            label3.Name = "label3";
            label3.Size = new Size(244, 21);
            label3.Text = "Welcome back, let's explore now!";
            // 
            // contentBodyPanel (ACTIVE VIEW PORTAL)
            // 
            contentBodyPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            contentBodyPanel.BackColor = Color.White;
            contentBodyPanel.Location = new Point(0, 90);
            contentBodyPanel.Name = "contentBodyPanel";
            contentBodyPanel.Size = new Size(1260, 745);
            contentBodyPanel.TabIndex = 0;
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1536, 864);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "DashboardForm";
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        private void SetupNavigationButtons()
        {
            string[] menuItems = { "Overview", "Employees", "Payroll", "Attendance", "Settings" };
            int startY = 15;
            int buttonHeight = 48;
            int spacing = 6;

            for (int i = 0; i < menuItems.Length; i++)
            {
                Button btn = new Button
                {
                    Text = "     " + menuItems[i],
                    Size = new Size(210, buttonHeight),
                    Location = new Point(15, startY + (i * (buttonHeight + spacing))),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.FromArgb(148, 163, 184),
                    Font = new Font("Segoe UI Semibold", 10F),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand
                };

                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);

                string menuText = menuItems[i];
                btn.Click += (s, e) => Navigation_Click(menuText);

                panel4.Controls.Add(btn);
            }
        }

        private void Navigation_Click(string route)
        {
            switch (route)
            {
                case "Overview":
                    SwitchView(new OverviewView(db.connection));
                    break;
                case "Employees":
                    SwitchView(new EmployeesView(db.connection));
                    break;
                case "Attendance":
                    SwitchView(new AttendanceView(db.connection));
                    break;
                case "Payroll":
                    SwitchView(new PayrollView(db.connection));
                    break;
                case "Settings":
                    SwitchView(new SettingsView(db.connection));
                    break;
                default:
                    MessageBox.Show($"{route} view is currently unmapped.", "Notification");
                    break;
            }
        }

        private void SwitchView(UserControl newView)
        {
            if (contentBodyPanel.Controls.Count > 0)
            {
                Control oldView = contentBodyPanel.Controls[0];
                contentBodyPanel.Controls.Remove(oldView);
                oldView.Dispose();
            }

            newView.Dock = DockStyle.Fill;
            contentBodyPanel.Controls.Add(newView);
        }
    }
}