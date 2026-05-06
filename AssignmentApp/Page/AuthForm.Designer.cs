using System.Drawing;

namespace AssignmentApp.Page
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
            uiGroupBox1 = new Sunny.UI.UIGroupBox();
            txtPass = new TextBox();
            txtUser = new TextBox();
            label2 = new Label();
            label1 = new Label();
            btnCancel = new MaterialSkin.Controls.MaterialButton();
            btnLogin = new MaterialSkin.Controls.MaterialButton();
            hopeGroupBox1 = new ReaLTaiizor.Controls.HopeGroupBox();
            lblConnect = new Sunny.UI.UISymbolLabel();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            uiGroupBox1.SuspendLayout();
            hopeGroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // uiGroupBox1
            // 
            uiGroupBox1.BackColor = Color.FromArgb(248, 243, 230);
            uiGroupBox1.Controls.Add(txtPass);
            uiGroupBox1.Controls.Add(txtUser);
            uiGroupBox1.Controls.Add(label2);
            uiGroupBox1.Controls.Add(label1);
            uiGroupBox1.FillColor = Color.FromArgb(248, 243, 230);
            uiGroupBox1.FillColor2 = Color.Empty;
            uiGroupBox1.FillDisableColor = Color.FromArgb(248, 243, 230);
            uiGroupBox1.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiGroupBox1.ForeColor = Color.FromArgb(103, 101, 86);
            uiGroupBox1.ForeDisableColor = Color.Empty;
            uiGroupBox1.Location = new Point(32, 22);
            uiGroupBox1.Margin = new Padding(4, 5, 4, 5);
            uiGroupBox1.MinimumSize = new Size(1, 1);
            uiGroupBox1.Name = "uiGroupBox1";
            uiGroupBox1.Padding = new Padding(0, 32, 0, 0);
            uiGroupBox1.RectColor = Color.FromArgb(63, 81, 181);
            uiGroupBox1.RectDisableColor = Color.Empty;
            uiGroupBox1.Size = new Size(528, 219);
            uiGroupBox1.TabIndex = 2;
            uiGroupBox1.Text = "LOGIN";
            uiGroupBox1.TextAlignment = ContentAlignment.BottomLeft;
            // 
            // txtPass
            // 
            txtPass.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtPass.Font = new Font("Microsoft Tai Le", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPass.Location = new Point(174, 133);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(312, 46);
            txtPass.TabIndex = 9;
            // 
            // txtUser
            // 
            txtUser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtUser.Font = new Font("Microsoft Tai Le", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUser.Location = new Point(174, 59);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(312, 46);
            txtUser.TabIndex = 8;
            txtUser.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(248, 243, 230);
            label2.Location = new Point(40, 146);
            label2.Name = "label2";
            label2.Size = new Size(106, 26);
            label2.TabIndex = 7;
            label2.Text = "Password:";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(248, 243, 230);
            label1.Location = new Point(40, 72);
            label1.Name = "label1";
            label1.Size = new Size(111, 26);
            label1.TabIndex = 6;
            label1.Text = "Username:";
            label1.Click += label1_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnCancel.Depth = 0;
            btnCancel.Font = new Font("Microsoft New Tai Lue", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.HighEmphasis = true;
            btnCancel.Icon = null;
            btnCancel.Location = new Point(483, 268);
            btnCancel.Margin = new Padding(4, 6, 4, 6);
            btnCancel.MouseState = MaterialSkin.MouseState.HOVER;
            btnCancel.Name = "btnCancel";
            btnCancel.NoAccentTextColor = Color.Empty;
            btnCancel.Size = new Size(77, 36);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "CANCEL";
            btnCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnCancel.UseAccentColor = true;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnLogin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLogin.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnLogin.Depth = 0;
            btnLogin.Font = new Font("Microsoft New Tai Lue", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.HighEmphasis = true;
            btnLogin.Icon = null;
            btnLogin.Location = new Point(395, 268);
            btnLogin.Margin = new Padding(4, 6, 4, 6);
            btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            btnLogin.Name = "btnLogin";
            btnLogin.NoAccentTextColor = Color.Empty;
            btnLogin.Size = new Size(64, 36);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "LOGIN";
            btnLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnLogin.UseAccentColor = false;
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // hopeGroupBox1
            // 
            hopeGroupBox1.BackColor = Color.FromArgb(254, 249, 238);
            hopeGroupBox1.BorderColor = Color.FromArgb(220, 223, 230);
            hopeGroupBox1.Controls.Add(lblConnect);
            hopeGroupBox1.Controls.Add(uiGroupBox1);
            hopeGroupBox1.Controls.Add(btnLogin);
            hopeGroupBox1.Controls.Add(btnCancel);
            hopeGroupBox1.Font = new Font("Segoe UI", 12F);
            hopeGroupBox1.ForeColor = Color.FromArgb(48, 48, 48);
            hopeGroupBox1.LineColor = Color.FromArgb(220, 223, 230);
            hopeGroupBox1.Location = new Point(254, 200);
            hopeGroupBox1.Name = "hopeGroupBox1";
            hopeGroupBox1.ShowText = false;
            hopeGroupBox1.Size = new Size(588, 328);
            hopeGroupBox1.TabIndex = 2;
            hopeGroupBox1.TabStop = false;
            hopeGroupBox1.Text = "hopeGroupBox1";
            hopeGroupBox1.ThemeColor = Color.FromArgb(254, 249, 238);
            // 
            // lblConnect
            // 
            lblConnect.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblConnect.BackColor = Color.FromArgb(254, 249, 238);
            lblConnect.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblConnect.ForeColor = Color.FromArgb(103, 101, 86);
            lblConnect.Location = new Point(32, 268);
            lblConnect.MinimumSize = new Size(1, 1);
            lblConnect.Name = "lblConnect";
            lblConnect.Size = new Size(242, 37);
            lblConnect.Symbol = 61713;
            lblConnect.SymbolColor = Color.FromArgb(0, 112, 112);
            lblConnect.TabIndex = 6;
            lblConnect.Text = "SERVER CONNECTION: SECURE";
            lblConnect.TextAlign = ContentAlignment.MiddleLeft;
            lblConnect.Click += uiSymbolLabel2_Click;
            // 
            // materialLabel1
            // 
            materialLabel1.AutoSize = true;
            materialLabel1.Depth = 0;
            materialLabel1.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel1.Location = new Point(0, 0);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new Size(1, 0);
            materialLabel1.TabIndex = 3;
            // 
            // frmAuth
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(254, 249, 238);
            ClientSize = new Size(1054, 656);
            Controls.Add(materialLabel1);
            Controls.Add(hopeGroupBox1);
            Name = "frmAuth";
            Text = "USER AUTHENTICATION";
            Load += AuthForm_Load;
            uiGroupBox1.ResumeLayout(false);
            uiGroupBox1.PerformLayout();
            hopeGroupBox1.ResumeLayout(false);
            hopeGroupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private MaterialSkin.Controls.MaterialButton btnCancel;
        private MaterialSkin.Controls.MaterialButton btnLogin;
        private ReaLTaiizor.Controls.HopeGroupBox hopeGroupBox1;
        private Label label2;
        private Label label1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private Sunny.UI.UISymbolLabel lblConnect;
        private TextBox txtUser;
        private TextBox txtPass;
    }
}