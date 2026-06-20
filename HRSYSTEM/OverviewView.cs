using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HRSYSTEM
{
    public class OverviewView : UserControl
    {
        private readonly string _connString;
        private FlowLayoutPanel cardContainer;

        public OverviewView(string connString)
        {
            this._connString = connString;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            Label title = new Label
            {
                Text = "Dashboard System Overview",
                Font = new Font("Segoe UI Black", 16F, FontStyle.Bold),
                Location = new Point(25, 20),
                Size = new Size(400, 40),
                ForeColor = Color.FromArgb(15, 23, 42)
            };
            this.Controls.Add(title);

            cardContainer = new FlowLayoutPanel
            {
                Location = new Point(25, 80),
                Size = new Size(1200, 600),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoScroll = true
            };
            this.Controls.Add(cardContainer);

            CalculateDashboardMetrics();
        }

        private void CalculateDashboardMetrics()
        {
            int totalActive = 0, totalLeft = 0, totalLateIncidents = 0;
            double averagePerformanceScore = 100.0;

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();

                    // Query 1: Employee Status Counts
                    using (MySqlCommand cmd = new MySqlCommand("SELECT status, COUNT(*) FROM employees GROUP BY status", conn))
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string status = rdr.GetString(0);
                            int count = rdr.GetInt32(1);
                            if (status == "Active") totalActive = count;
                            else if (status == "Terminated") totalLeft = count;
                        }
                    }

                    // Query 2: Attendance Metrics
                    string attSql = "SELECT COUNT(CASE WHEN is_late = 1 THEN 1 END), AVG(punctuality_score) FROM attendance";
                    using (MySqlCommand cmd = new MySqlCommand(attSql, conn))
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            totalLateIncidents = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                            averagePerformanceScore = rdr.IsDBNull(1) ? 100.0 : rdr.GetDouble(1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Overview metrics failure: " + ex.Message);
                }
            }

            // Render KPI display metric cards
            cardContainer.Controls.Add(CreateKPIControlCard("ACTIVE EMPLOYEES", totalActive.ToString(), Color.FromArgb(34, 197, 94)));
            cardContainer.Controls.Add(CreateKPIControlCard("EMPLOYEES DEPARTED", totalLeft.ToString(), Color.FromArgb(239, 68, 68)));
            cardContainer.Controls.Add(CreateKPIControlCard("LATE INCIDENTS RECORDED", totalLateIncidents.ToString(), Color.FromArgb(245, 158, 11)));
            cardContainer.Controls.Add(CreateKPIControlCard("AVG PUNCTUALITY SCORE", $"{Math.Round(averagePerformanceScore, 1)} / 100", Color.FromArgb(59, 130, 246)));
        }

        private Panel CreateKPIControlCard(string title, string textMetric, Color themeBg)
        {
            Panel card = new Panel { Size = new Size(260, 140), BackColor = Color.FromArgb(248, 250, 252), Margin = new Padding(0, 0, 25, 25), BorderStyle = BorderStyle.FixedSingle };

            Panel accentBorder = new Panel { Height = 5, Dock = DockStyle.Top, BackColor = themeBg };
            card.Controls.Add(accentBorder);

            Label lblTitle = new Label { Text = title, Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(15, 25), AutoSize = true };
            Label lblValue = new Label { Text = textMetric, Font = new Font("Segoe UI Black", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), Location = new Point(15, 55), AutoSize = true };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            return card;
        }
    }
}