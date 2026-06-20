using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HRSYSTEM
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblEmail = new Label();
            lblPassword = new Label();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            pictureBox1 = new PictureBox();
            pnlDivider = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.banner_auth1;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(475, 540); // Stretched to meet the exact center split
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage; // Centers the image inside the panel cleanly
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // pnlDivider
            // 
            pnlDivider.BackColor = Color.FromArgb(226, 232, 240); // Subtle, soft border color
            pnlDivider.Location = new Point(475, 0); // Placed exactly where the picture box ends
            pnlDivider.Name = "pnlDivider";
            pnlDivider.Size = new Size(1, 540); // 1-pixel crisp divider
            pnlDivider.TabIndex = 5;

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //pictureBox1.TabIndex = 4;
            //pictureBox1.TabStop = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitle.Location = new Point(535, 75); // Re-aligned nicely for the right-hand panel
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(207, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Welcome Back";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(539, 125);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(245, 21);
            lblSubtitle.TabIndex = 6;
            lblSubtitle.Text = "Please enter your details to sign in.";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(71, 85, 105);
            lblEmail.Location = new Point(539, 185);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(119, 21);
            lblEmail.TabIndex = 1;
            lblEmail.Text = "Email Address";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.White;
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.ForeColor = Color.FromArgb(15, 23, 42);
            txtEmail.Location = new Point(539, 212);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(350, 32);
            txtEmail.PlaceholderText = "name@company.com";
            txtEmail.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(71, 85, 105);
            lblPassword.Location = new Point(539, 270);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(81, 21);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.White;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.ForeColor = Color.FromArgb(15, 23, 42);
            txtPassword.Location = new Point(539, 297);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(350, 32);
            txtPassword.PlaceholderText = "••••••••";
            txtPassword.TabIndex = 2;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(79, 70, 229);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(539, 370);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(350, 48);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Sign In";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(950, 540); // Total width = 950
            Controls.Add(pnlDivider);
            Controls.Add(lblSubtitle);
            Controls.Add(pictureBox1);
            Controls.Add(lblTitle);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HR System - Authorization";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblEmail;
        private Label lblPassword;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private PictureBox pictureBox1;
        private Panel pnlDivider;
    }
}