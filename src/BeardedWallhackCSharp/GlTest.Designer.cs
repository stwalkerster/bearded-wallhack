namespace BeardedWallhackCSharp
{
    partial class GlTest
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
            this.glControl1 = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(12, 12);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(606, 405);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.GlControl1Paint);
            this.glControl1.Resize += new System.EventHandler(this.GlControl1Resize);
            // 
            // GlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 430);
            this.Controls.Add(this.glControl1);
            this.Name = "GlTest";
            this.Text = "GlTest";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
    }
}