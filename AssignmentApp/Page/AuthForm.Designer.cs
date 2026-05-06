using System.Drawing;

namespace AssignmentApp.Page
{
    partial class AuthForm
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
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label2 = new Label();
            label1 = new Label();
            materialButton2 = new MaterialSkin.Controls.MaterialButton();
            materialButton1 = new MaterialSkin.Controls.MaterialButton();
            hopeGroupBox1 = new ReaLTaiizor.Controls.HopeGroupBox();
            uiSymbolLabel2 = new Sunny.UI.UISymbolLabel();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            uiGroupBox1.SuspendLayout();
            hopeGroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // uiGroupBox1
            // 
            uiGroupBox1.BackColor = Color.FromArgb(248, 243, 230);
            uiGroupBox1.Controls.Add(textBox2);
            uiGroupBox1.Controls.Add(textBox1);
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
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox2.Font = new Font("Microsoft Tai Le", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(174, 133);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(312, 46);
            textBox2.TabIndex = 9;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Font = new Font("Microsoft Tai Le", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(174, 59);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(312, 46);
            textBox1.TabIndex = 8;
            textBox1.TextChanged += textBox1_TextChanged;
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
            // materialButton2
            // 
            materialButton2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            materialButton2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton2.Depth = 0;
            materialButton2.Font = new Font("Microsoft New Tai Lue", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            materialButton2.HighEmphasis = true;
            materialButton2.Icon = null;
            materialButton2.Location = new Point(483, 268);
            materialButton2.Margin = new Padding(4, 6, 4, 6);
            materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton2.Name = "materialButton2";
            materialButton2.NoAccentTextColor = Color.Empty;
            materialButton2.Size = new Size(77, 36);
            materialButton2.TabIndex = 1;
            materialButton2.Text = "CANCEL";
            materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton2.UseAccentColor = true;
            materialButton2.UseVisualStyleBackColor = true;
            // 
            // materialButton1
            // 
            materialButton1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            materialButton1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton1.Depth = 0;
            materialButton1.Font = new Font("Microsoft New Tai Lue", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            materialButton1.HighEmphasis = true;
            materialButton1.Icon = null;
            materialButton1.Location = new Point(395, 268);
            materialButton1.Margin = new Padding(4, 6, 4, 6);
            materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton1.Name = "materialButton1";
            materialButton1.NoAccentTextColor = Color.Empty;
            materialButton1.Size = new Size(64, 36);
            materialButton1.TabIndex = 0;
            materialButton1.Text = "LOGIN";
            materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton1.UseAccentColor = false;
            materialButton1.UseVisualStyleBackColor = true;
            // 
            // hopeGroupBox1
            // 
            hopeGroupBox1.BackColor = Color.FromArgb(254, 249, 238);
            hopeGroupBox1.BorderColor = Color.FromArgb(220, 223, 230);
            hopeGroupBox1.Controls.Add(uiSymbolLabel2);
            hopeGroupBox1.Controls.Add(uiGroupBox1);
            hopeGroupBox1.Controls.Add(materialButton1);
            hopeGroupBox1.Controls.Add(materialButton2);
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
            // uiSymbolLabel2
            // 
            uiSymbolLabel2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            uiSymbolLabel2.BackColor = Color.FromArgb(254, 249, 238);
            uiSymbolLabel2.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolLabel2.ForeColor = Color.FromArgb(103, 101, 86);
            uiSymbolLabel2.Location = new Point(32, 268);
            uiSymbolLabel2.MinimumSize = new Size(1, 1);
            uiSymbolLabel2.Name = "uiSymbolLabel2";
            uiSymbolLabel2.Size = new Size(242, 37);
            uiSymbolLabel2.Symbol = 61713;
            uiSymbolLabel2.SymbolColor = Color.FromArgb(0, 112, 112);
            uiSymbolLabel2.TabIndex = 6;
            uiSymbolLabel2.Text = "SERVER CONNECTION: SECURE";
            uiSymbolLabel2.TextAlign = ContentAlignment.MiddleLeft;
            uiSymbolLabel2.Click += uiSymbolLabel2_Click;
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
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(254, 249, 238);
            ClientSize = new Size(1054, 656);
            Controls.Add(materialLabel1);
            Controls.Add(hopeGroupBox1);
            Name = "AuthForm";
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
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private ReaLTaiizor.Controls.HopeGroupBox hopeGroupBox1;
        private Label label2;
        private Label label1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private Sunny.UI.UISymbolLabel uiSymbolLabel2;
        private TextBox textBox1;
        private TextBox textBox2;
    }
}