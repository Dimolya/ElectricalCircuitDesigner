namespace ElectroMod.Forms.InputForms
{
    partial class InputParametersBus
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
            this.tbVoltage = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lbVoltage = new System.Windows.Forms.Label();
            this.lbDataTypes = new System.Windows.Forms.Label();
            this.cbDataTypes = new System.Windows.Forms.ComboBox();
            this.tbElementName = new System.Windows.Forms.TextBox();
            this.lbTextName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbVoltage
            // 
            this.tbVoltage.Location = new System.Drawing.Point(206, 36);
            this.tbVoltage.Name = "tbVoltage";
            this.tbVoltage.Size = new System.Drawing.Size(273, 22);
            this.tbVoltage.TabIndex = 31;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(389, 139);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(293, 139);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 30);
            this.btnApply.TabIndex = 29;
            this.btnApply.Text = "Принять";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lbVoltage
            // 
            this.lbVoltage.AutoSize = true;
            this.lbVoltage.Location = new System.Drawing.Point(9, 39);
            this.lbVoltage.Name = "lbVoltage";
            this.lbVoltage.Size = new System.Drawing.Size(89, 16);
            this.lbVoltage.TabIndex = 28;
            this.lbVoltage.Text = "Напряжение";
            // 
            // lbDataTypes
            // 
            this.lbDataTypes.AutoSize = true;
            this.lbDataTypes.Location = new System.Drawing.Point(9, 67);
            this.lbDataTypes.Name = "lbDataTypes";
            this.lbDataTypes.Size = new System.Drawing.Size(123, 16);
            this.lbDataTypes.TabIndex = 27;
            this.lbDataTypes.Text = "Исходные данные";
            // 
            // cbDataTypes
            // 
            this.cbDataTypes.FormattingEnabled = true;
            this.cbDataTypes.Location = new System.Drawing.Point(206, 64);
            this.cbDataTypes.Name = "cbDataTypes";
            this.cbDataTypes.Size = new System.Drawing.Size(273, 24);
            this.cbDataTypes.TabIndex = 26;
            // 
            // tbElementName
            // 
            this.tbElementName.Location = new System.Drawing.Point(206, 8);
            this.tbElementName.Name = "tbElementName";
            this.tbElementName.Size = new System.Drawing.Size(273, 22);
            this.tbElementName.TabIndex = 25;
            // 
            // lbTextName
            // 
            this.lbTextName.AutoSize = true;
            this.lbTextName.Location = new System.Drawing.Point(9, 11);
            this.lbTextName.Name = "lbTextName";
            this.lbTextName.Size = new System.Drawing.Size(106, 16);
            this.lbTextName.TabIndex = 24;
            this.lbTextName.Text = "Наименование";
            // 
            // InputParametersBus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 176);
            this.Controls.Add(this.tbVoltage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lbVoltage);
            this.Controls.Add(this.lbDataTypes);
            this.Controls.Add(this.cbDataTypes);
            this.Controls.Add(this.tbElementName);
            this.Controls.Add(this.lbTextName);
            this.Name = "InputParametersBus";
            this.Text = "Вводные данные";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbVoltage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lbVoltage;
        private System.Windows.Forms.Label lbDataTypes;
        private System.Windows.Forms.ComboBox cbDataTypes;
        private System.Windows.Forms.TextBox tbElementName;
        private System.Windows.Forms.Label lbTextName;
    }
}