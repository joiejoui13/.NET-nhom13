using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace AssignmentApp.GUI
{
    partial class frmAuth
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAuth));
            guna2Elipse1 = new Guna2Elipse(components);
            guna2Panel1 = new Guna2Panel();
            uiGroupBox1 = new Guna2GroupBox();
            txtPass = new Guna2TextBox();
            txtUser = new Guna2TextBox();
            label2 = new Label();
            label1 = new Label();
            btnLogin = new Guna2Button();
            btnCancel = new Guna2Button();
            lblConnect = new Guna2Button();
            guna2Panel1.SuspendLayout();
            uiGroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Elipse1
            // 
            guna2Elipse1.BorderRadius = 20;
            guna2Elipse1.TargetControl = this;
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.White;
            guna2Panel1.Controls.Add(uiGroupBox1);
            guna2Panel1.Controls.Add(btnLogin);
            guna2Panel1.Controls.Add(btnCancel);
            guna2Panel1.Controls.Add(lblConnect);
            guna2Panel1.CustomizableEdges = customizableEdges13;
            guna2Panel1.Location = new Point(157, 87);
            guna2Panel1.Margin = new Padding(3, 2, 3, 2);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges14;
            guna2Panel1.ShadowDecoration.Enabled = true;
            guna2Panel1.Size = new Size(551, 267);
            guna2Panel1.TabIndex = 2;
            // 
            // uiGroupBox1
            // 
            uiGroupBox1.BorderRadius = 10;
            uiGroupBox1.Controls.Add(txtPass);
            uiGroupBox1.Controls.Add(txtUser);
            uiGroupBox1.Controls.Add(label2);
            uiGroupBox1.Controls.Add(label1);
            uiGroupBox1.CustomBorderColor = Color.FromArgb(0, 126, 249);
            uiGroupBox1.CustomizableEdges = customizableEdges5;
            uiGroupBox1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            uiGroupBox1.ForeColor = Color.White;
            uiGroupBox1.Location = new Point(28, 26);
            uiGroupBox1.Margin = new Padding(3, 2, 3, 2);
            uiGroupBox1.Name = "uiGroupBox1";
            uiGroupBox1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            uiGroupBox1.Size = new Size(498, 164);
            uiGroupBox1.TabIndex = 2;
            uiGroupBox1.Text = "LOGIN";
            // 
            // txtPass
            // 
            txtPass.BorderRadius = 8;
            txtPass.Cursor = Cursors.IBeam;
            txtPass.CustomizableEdges = customizableEdges1;
            txtPass.DefaultText = "";
            txtPass.Font = new Font("Segoe UI", 12F);
            txtPass.HoverState.BorderColor = Color.FromArgb(0, 126, 249);
            txtPass.Location = new Point(150, 109);
            txtPass.Margin = new Padding(4, 4, 4, 4);
            txtPass.Name = "txtPass";
            txtPass.PasswordChar = '●';
            txtPass.PlaceholderText = "Password";
            txtPass.SelectedText = "";
            txtPass.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPass.Size = new Size(312, 34);
            txtPass.TabIndex = 9;
            txtPass.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            txtUser.BorderRadius = 8;
            txtUser.Cursor = Cursors.IBeam;
            txtUser.CustomizableEdges = customizableEdges3;
            txtUser.DefaultText = "";
            txtUser.Font = new Font("Segoe UI", 12F);
            txtUser.HoverState.BorderColor = Color.FromArgb(0, 126, 249);
            txtUser.Location = new Point(150, 53);
            txtUser.Margin = new Padding(4, 4, 4, 4);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "Username";
            txtUser.SelectedText = "";
            txtUser.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtUser.Size = new Size(312, 34);
            txtUser.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.FromArgb(64, 64, 64);
            label2.Location = new Point(33, 119);
            label2.Name = "label2";
            label2.Size = new Size(83, 21);
            label2.TabIndex = 7;
            label2.Text = "Password:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(33, 63);
            label1.Name = "label1";
            label1.Size = new Size(87, 21);
            label1.TabIndex = 6;
            label1.Text = "Username:";
            // 
            // btnLogin
            // 
            btnLogin.BorderRadius = 10;
            btnLogin.CustomizableEdges = customizableEdges7;
            btnLogin.DisabledState.BorderColor = Color.DarkGray;
            btnLogin.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLogin.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLogin.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLogin.FillColor = Color.FromArgb(0, 126, 249);
            btnLogin.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(346, 214);
            btnLogin.Margin = new Padding(3, 2, 3, 2);
            btnLogin.Name = "btnLogin";
            btnLogin.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnLogin.Size = new Size(88, 30);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "LOGIN";
            btnLogin.Click += btnLogin_Click;
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 10;
            btnCancel.CustomizableEdges = customizableEdges9;
            btnCancel.DisabledState.BorderColor = Color.DarkGray;
            btnCancel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancel.FillColor = Color.FromArgb(231, 76, 60);
            btnCancel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(438, 214);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnCancel.Size = new Size(88, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "CANCEL";
            btnCancel.Click += btnCancel_Click;
            btnCancel.DoubleClick += btnCancel_DoubleClick;
            // 
            // lblConnect
            // 
            lblConnect.BackColor = Color.Transparent;
            lblConnect.BorderRadius = 5;
            lblConnect.CustomizableEdges = customizableEdges11;
            lblConnect.DisabledState.BorderColor = Color.DarkGray;
            lblConnect.DisabledState.CustomBorderColor = Color.DarkGray;
            lblConnect.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            lblConnect.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            lblConnect.FillColor = Color.FromArgb(242, 245, 250);
            lblConnect.Font = new Font("Segoe UI", 9F);
            lblConnect.ForeColor = Color.FromArgb(103, 101, 86);
            lblConnect.Location = new Point(28, 214);
            lblConnect.Margin = new Padding(3, 2, 3, 2);
            lblConnect.Name = "lblConnect";
            lblConnect.ShadowDecoration.CustomizableEdges = customizableEdges12;
            lblConnect.Size = new Size(212, 28);
            lblConnect.TabIndex = 6;
            lblConnect.Text = "SERVER CONNECTION: SECURE";
            lblConnect.TextAlign = HorizontalAlignment.Left;
            // 
            // frmAuth
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 245, 250);
            ClientSize = new Size(861, 446);
            Controls.Add(guna2Panel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmAuth";
            Text = "USER AUTHENTICATION";
            Load += AuthForm_Load;
            guna2Panel1.ResumeLayout(false);
            uiGroupBox1.ResumeLayout(false);
            uiGroupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna2Panel guna2Panel1;
        private Guna2GroupBox uiGroupBox1;
        private Guna2Button btnCancel;
        private Guna2Button btnLogin;
        private Label label2;
        private Label label1;
        private Guna2Button lblConnect;
        private Guna2TextBox txtUser;
        private Guna2TextBox txtPass;
        private Guna2Elipse guna2Elipse1;
    }
}