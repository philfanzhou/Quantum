namespace Quantum.Presentation.WinForm
{
    partial class MiniRealTime
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.picBoxTop = new System.Windows.Forms.PictureBox();
            this.picBoxButtom = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxButtom)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.picBoxTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.picBoxButtom);
            this.splitContainer1.Size = new System.Drawing.Size(315, 508);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 0;
            // 
            // picBoxTop
            // 
            this.picBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxTop.Location = new System.Drawing.Point(0, 0);
            this.picBoxTop.Name = "picBoxTop";
            this.picBoxTop.Size = new System.Drawing.Size(315, 239);
            this.picBoxTop.TabIndex = 0;
            this.picBoxTop.TabStop = false;
            // 
            // picBoxButtom
            // 
            this.picBoxButtom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxButtom.Location = new System.Drawing.Point(0, 0);
            this.picBoxButtom.Name = "picBoxButtom";
            this.picBoxButtom.Size = new System.Drawing.Size(315, 265);
            this.picBoxButtom.TabIndex = 0;
            this.picBoxButtom.TabStop = false;
            // 
            // MiniRealTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MiniRealTime";
            this.Size = new System.Drawing.Size(315, 508);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxButtom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox picBoxTop;
        private System.Windows.Forms.PictureBox picBoxButtom;
    }
}
