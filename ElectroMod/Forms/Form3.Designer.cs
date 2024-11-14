namespace ElectroMod
{
    partial class ResistanceChar
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
            this.tbRAll = new System.Windows.Forms.TextBox();
            this.bAllSave = new System.Windows.Forms.Button();
            this.lbR = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbRAll
            // 
            this.tbRAll.Location = new System.Drawing.Point(182, 11);
            this.tbRAll.Name = "tbRAll";
            this.tbRAll.Size = new System.Drawing.Size(100, 22);
            this.tbRAll.TabIndex = 9;
            // 
            // bAllSave
            // 
            this.bAllSave.Location = new System.Drawing.Point(98, 49);
            this.bAllSave.Name = "bAllSave";
            this.bAllSave.Size = new System.Drawing.Size(96, 30);
            this.bAllSave.TabIndex = 8;
            this.bAllSave.Text = "Сохранить";
            this.bAllSave.UseVisualStyleBackColor = true;
            this.bAllSave.Click += new System.EventHandler(this.bAllSave_Click);
            // 
            // lbR
            // 
            this.lbR.AutoSize = true;
            this.lbR.Location = new System.Drawing.Point(10, 16);
            this.lbR.Name = "lbR";
            this.lbR.Size = new System.Drawing.Size(111, 17);
            this.lbR.TabIndex = 7;
            this.lbR.Text = "Сопротивление";
            // 
            // ResistanceChar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 92);
            this.Controls.Add(this.tbRAll);
            this.Controls.Add(this.bAllSave);
            this.Controls.Add(this.lbR);
            this.Name = "ResistanceChar";
            this.Text = "Сопротивление";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbRAll;
        private System.Windows.Forms.Button bAllSave;
        private System.Windows.Forms.Label lbR;
    }
}