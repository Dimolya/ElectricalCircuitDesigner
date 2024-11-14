namespace ElectroMod
{
    partial class PowerSupplyCharacterization
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
            this.lbA = new System.Windows.Forms.Label();
            this.lbV = new System.Windows.Forms.Label();
            this.lbR = new System.Windows.Forms.Label();
            this.bPSSave = new System.Windows.Forms.Button();
            this.tbAPS = new System.Windows.Forms.TextBox();
            this.tbVPS = new System.Windows.Forms.TextBox();
            this.tbRPS = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbA
            // 
            this.lbA.AutoSize = true;
            this.lbA.Location = new System.Drawing.Point(13, 13);
            this.lbA.Name = "lbA";
            this.lbA.Size = new System.Drawing.Size(75, 17);
            this.lbA.TabIndex = 0;
            this.lbA.Text = "Сила тока";
            // 
            // lbV
            // 
            this.lbV.AutoSize = true;
            this.lbV.Location = new System.Drawing.Point(13, 56);
            this.lbV.Name = "lbV";
            this.lbV.Size = new System.Drawing.Size(91, 17);
            this.lbV.TabIndex = 1;
            this.lbV.Text = "Напряжение";
            // 
            // lbR
            // 
            this.lbR.AutoSize = true;
            this.lbR.Location = new System.Drawing.Point(13, 102);
            this.lbR.Name = "lbR";
            this.lbR.Size = new System.Drawing.Size(111, 17);
            this.lbR.TabIndex = 2;
            this.lbR.Text = "Сопротивление";
            // 
            // bPSSave
            // 
            this.bPSSave.Location = new System.Drawing.Point(101, 135);
            this.bPSSave.Name = "bPSSave";
            this.bPSSave.Size = new System.Drawing.Size(96, 30);
            this.bPSSave.TabIndex = 3;
            this.bPSSave.Text = "Сохранить";
            this.bPSSave.UseVisualStyleBackColor = true;
            this.bPSSave.Click += new System.EventHandler(this.bPSSave_Click);
            // 
            // tbAPS
            // 
            this.tbAPS.Location = new System.Drawing.Point(185, 13);
            this.tbAPS.Name = "tbAPS";
            this.tbAPS.Size = new System.Drawing.Size(100, 22);
            this.tbAPS.TabIndex = 4;
            // 
            // tbVPS
            // 
            this.tbVPS.Location = new System.Drawing.Point(185, 56);
            this.tbVPS.Name = "tbVPS";
            this.tbVPS.Size = new System.Drawing.Size(100, 22);
            this.tbVPS.TabIndex = 5;
            // 
            // tbRPS
            // 
            this.tbRPS.Location = new System.Drawing.Point(185, 97);
            this.tbRPS.Name = "tbRPS";
            this.tbRPS.Size = new System.Drawing.Size(100, 22);
            this.tbRPS.TabIndex = 6;
            // 
            // PowerSupplyCharacterization
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(309, 177);
            this.Controls.Add(this.tbRPS);
            this.Controls.Add(this.tbVPS);
            this.Controls.Add(this.tbAPS);
            this.Controls.Add(this.bPSSave);
            this.Controls.Add(this.lbR);
            this.Controls.Add(this.lbV);
            this.Controls.Add(this.lbA);
            this.MaximizeBox = false;
            this.Name = "PowerSupplyCharacterization";
            this.Text = "Характеристики";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbA;
        private System.Windows.Forms.Label lbV;
        private System.Windows.Forms.Label lbR;
        private System.Windows.Forms.Button bPSSave;
        private System.Windows.Forms.TextBox tbAPS;
        private System.Windows.Forms.TextBox tbVPS;
        private System.Windows.Forms.TextBox tbRPS;
    }
}