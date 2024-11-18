namespace ElectroMod.Forms.InputForms
{
    partial class InputParametersRecloser
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
            this.lbTypeTT = new System.Windows.Forms.Label();
            this.lbTypeRecloser = new System.Windows.Forms.Label();
            this.cbTypeTT = new System.Windows.Forms.ComboBox();
            this.cbTypeRecloser = new System.Windows.Forms.ComboBox();
            this.tbElementName = new System.Windows.Forms.TextBox();
            this.lbTextName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbTypeTT
            // 
            this.lbTypeTT.AutoSize = true;
            this.lbTypeTT.Location = new System.Drawing.Point(12, 72);
            this.lbTypeTT.Name = "lbTypeTT";
            this.lbTypeTT.Size = new System.Drawing.Size(87, 16);
            this.lbTypeTT.TabIndex = 12;
            this.lbTypeTT.Text = "Номинал ТТ";
            // 
            // lbTypeRecloser
            // 
            this.lbTypeRecloser.AutoSize = true;
            this.lbTypeRecloser.Location = new System.Drawing.Point(12, 42);
            this.lbTypeRecloser.Name = "lbTypeRecloser";
            this.lbTypeRecloser.Size = new System.Drawing.Size(32, 16);
            this.lbTypeRecloser.TabIndex = 11;
            this.lbTypeRecloser.Text = "Тип";
            // 
            // cbTypeTT
            // 
            this.cbTypeTT.FormattingEnabled = true;
            this.cbTypeTT.Location = new System.Drawing.Point(209, 69);
            this.cbTypeTT.Name = "cbTypeTT";
            this.cbTypeTT.Size = new System.Drawing.Size(273, 24);
            this.cbTypeTT.TabIndex = 10;
            // 
            // cbTypeRecloser
            // 
            this.cbTypeRecloser.FormattingEnabled = true;
            this.cbTypeRecloser.Location = new System.Drawing.Point(209, 39);
            this.cbTypeRecloser.Name = "cbTypeRecloser";
            this.cbTypeRecloser.Size = new System.Drawing.Size(273, 24);
            this.cbTypeRecloser.TabIndex = 9;
            // 
            // tbElementName
            // 
            this.tbElementName.Location = new System.Drawing.Point(209, 12);
            this.tbElementName.Name = "tbElementName";
            this.tbElementName.Size = new System.Drawing.Size(273, 22);
            this.tbElementName.TabIndex = 8;
            // 
            // lbTextName
            // 
            this.lbTextName.AutoSize = true;
            this.lbTextName.Location = new System.Drawing.Point(12, 13);
            this.lbTextName.Name = "lbTextName";
            this.lbTextName.Size = new System.Drawing.Size(106, 16);
            this.lbTextName.TabIndex = 7;
            this.lbTextName.Text = "Наименование";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(392, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(296, 143);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 30);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "Принять";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // InputParametersRecloser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 176);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lbTypeTT);
            this.Controls.Add(this.lbTypeRecloser);
            this.Controls.Add(this.cbTypeTT);
            this.Controls.Add(this.cbTypeRecloser);
            this.Controls.Add(this.tbElementName);
            this.Controls.Add(this.lbTextName);
            this.Name = "InputParametersRecloser";
            this.Text = "InputParametersRecloser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTypeTT;
        private System.Windows.Forms.Label lbTypeRecloser;
        private System.Windows.Forms.ComboBox cbTypeTT;
        private System.Windows.Forms.ComboBox cbTypeRecloser;
        private System.Windows.Forms.TextBox tbElementName;
        private System.Windows.Forms.Label lbTextName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
    }
}