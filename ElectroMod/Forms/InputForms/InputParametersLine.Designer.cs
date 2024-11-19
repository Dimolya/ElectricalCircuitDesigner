namespace ElectroMod.Forms.InputForms
{
    partial class InputParametersLine
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lbLength = new System.Windows.Forms.Label();
            this.lbMarks = new System.Windows.Forms.Label();
            this.cbMarks = new System.Windows.Forms.ComboBox();
            this.tbElementName = new System.Windows.Forms.TextBox();
            this.lbTextName = new System.Windows.Forms.Label();
            this.tbLength = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(389, 139);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(293, 139);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 30);
            this.btnApply.TabIndex = 21;
            this.btnApply.Text = "Принять";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lbLength
            // 
            this.lbLength.AutoSize = true;
            this.lbLength.Location = new System.Drawing.Point(9, 39);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(48, 16);
            this.lbLength.TabIndex = 20;
            this.lbLength.Text = "Длина";
            // 
            // lbMarks
            // 
            this.lbMarks.AutoSize = true;
            this.lbMarks.Location = new System.Drawing.Point(9, 67);
            this.lbMarks.Name = "lbMarks";
            this.lbMarks.Size = new System.Drawing.Size(108, 16);
            this.lbMarks.TabIndex = 19;
            this.lbMarks.Text = "Марка провода";
            // 
            // cbMarks
            // 
            this.cbMarks.FormattingEnabled = true;
            this.cbMarks.Location = new System.Drawing.Point(206, 64);
            this.cbMarks.Name = "cbMarks";
            this.cbMarks.Size = new System.Drawing.Size(273, 24);
            this.cbMarks.TabIndex = 17;
            // 
            // tbElementName
            // 
            this.tbElementName.Location = new System.Drawing.Point(206, 8);
            this.tbElementName.Name = "tbElementName";
            this.tbElementName.Size = new System.Drawing.Size(273, 22);
            this.tbElementName.TabIndex = 16;
            // 
            // lbTextName
            // 
            this.lbTextName.AutoSize = true;
            this.lbTextName.Location = new System.Drawing.Point(9, 11);
            this.lbTextName.Name = "lbTextName";
            this.lbTextName.Size = new System.Drawing.Size(106, 16);
            this.lbTextName.TabIndex = 15;
            this.lbTextName.Text = "Наименование";
            // 
            // tbLength
            // 
            this.tbLength.Location = new System.Drawing.Point(206, 36);
            this.tbLength.Name = "tbLength";
            this.tbLength.Size = new System.Drawing.Size(273, 22);
            this.tbLength.TabIndex = 23;
            // 
            // InputParametersLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 176);
            this.Controls.Add(this.tbLength);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lbLength);
            this.Controls.Add(this.lbMarks);
            this.Controls.Add(this.cbMarks);
            this.Controls.Add(this.tbElementName);
            this.Controls.Add(this.lbTextName);
            this.Name = "InputParametersLine";
            this.Text = "Вводные параметры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Label lbMarks;
        private System.Windows.Forms.ComboBox cbMarks;
        private System.Windows.Forms.TextBox tbElementName;
        private System.Windows.Forms.Label lbTextName;
        private System.Windows.Forms.TextBox tbLength;
    }
}