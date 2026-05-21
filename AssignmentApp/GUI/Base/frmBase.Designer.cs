namespace AssignmentApp.GUI.Base
{
    partial class frmBase
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(components);
            guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(components);
            SuspendLayout();
            // 
            // guna2AnimateWindow1
            // 
            guna2AnimateWindow1.AnimationType = Guna.UI2.WinForms.Guna2AnimateWindow.AnimateWindowType.AW_BLEND;
            guna2AnimateWindow1.TargetForm = this;
            // 
            // frmBase
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1333, 865);
            Margin = new Padding(5, 6, 5, 6);
            Name = "frmBase";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmBase";
            ResumeLayout(false);

        }

        #endregion

        protected Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
        protected Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
    }
}
