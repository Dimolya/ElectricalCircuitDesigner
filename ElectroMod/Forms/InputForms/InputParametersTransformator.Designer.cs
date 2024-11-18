namespace ElectroMod.Forms
{
    partial class InputParametersTransformator
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
            this.lbTextName = new System.Windows.Forms.Label();
            this.tbElementName = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.cbTypeKTP = new System.Windows.Forms.ComboBox();
            this.cbShemeConnectWinding = new System.Windows.Forms.ComboBox();
            this.lbTypeKTP = new System.Windows.Forms.Label();
            this.lbShemeConnectWinding = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbTextName
            // 
            this.lbTextName.AutoSize = true;
            this.lbTextName.Location = new System.Drawing.Point(13, 13);
            this.lbTextName.Name = "lbTextName";
            this.lbTextName.Size = new System.Drawing.Size(106, 16);
            this.lbTextName.TabIndex = 0;
            this.lbTextName.Text = "Наименование";
            // 
            // tbElementName
            // 
            this.tbElementName.Location = new System.Drawing.Point(210, 12);
            this.tbElementName.Name = "tbElementName";
            this.tbElementName.Size = new System.Drawing.Size(273, 22);
            this.tbElementName.TabIndex = 1;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(297, 141);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 30);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Принять";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cbTypeKTP
            // 
            this.cbTypeKTP.FormattingEnabled = true;
            this.cbTypeKTP.Location = new System.Drawing.Point(210, 39);
            this.cbTypeKTP.Name = "cbTypeKTP";
            this.cbTypeKTP.Size = new System.Drawing.Size(273, 24);
            this.cbTypeKTP.TabIndex = 3;
            // 
            // cbShemeConnectWinding
            // 
            this.cbShemeConnectWinding.FormattingEnabled = true;
            this.cbShemeConnectWinding.Location = new System.Drawing.Point(210, 69);
            this.cbShemeConnectWinding.Name = "cbShemeConnectWinding";
            this.cbShemeConnectWinding.Size = new System.Drawing.Size(273, 24);
            this.cbShemeConnectWinding.TabIndex = 4;
            // 
            // lbTypeKTP
            // 
            this.lbTypeKTP.AutoSize = true;
            this.lbTypeKTP.Location = new System.Drawing.Point(13, 42);
            this.lbTypeKTP.Name = "lbTypeKTP";
            this.lbTypeKTP.Size = new System.Drawing.Size(62, 16);
            this.lbTypeKTP.TabIndex = 5;
            this.lbTypeKTP.Text = "Тип КТП";
            // 
            // lbShemeConnectWinding
            // 
            this.lbShemeConnectWinding.AutoSize = true;
            this.lbShemeConnectWinding.Location = new System.Drawing.Point(13, 72);
            this.lbShemeConnectWinding.Name = "lbShemeConnectWinding";
            this.lbShemeConnectWinding.Size = new System.Drawing.Size(186, 16);
            this.lbShemeConnectWinding.TabIndex = 6;
            this.lbShemeConnectWinding.Text = "Схема соединения обмоток";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(393, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // InputParametersTransformator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 176);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbShemeConnectWinding);
            this.Controls.Add(this.lbTypeKTP);
            this.Controls.Add(this.cbShemeConnectWinding);
            this.Controls.Add(this.cbTypeKTP);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tbElementName);
            this.Controls.Add(this.lbTextName);
            this.Name = "InputParametersTransformator";
            this.Text = "Вводные параметры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTextName;
        private System.Windows.Forms.TextBox tbElementName;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox cbTypeKTP;
        private System.Windows.Forms.ComboBox cbShemeConnectWinding;
        private System.Windows.Forms.Label lbTypeKTP;
        private System.Windows.Forms.Label lbShemeConnectWinding;
        private System.Windows.Forms.Button btnCancel;
    }
}