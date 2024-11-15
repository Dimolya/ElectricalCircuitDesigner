namespace ElectroMod.Forms
{
    partial class InputParametersElement
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
            this.tbElementName.Location = new System.Drawing.Point(125, 10);
            this.tbElementName.Name = "tbElementName";
            this.tbElementName.Size = new System.Drawing.Size(273, 22);
            this.tbElementName.TabIndex = 1;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(323, 84);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Принять";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // InputParametersElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 119);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tbElementName);
            this.Controls.Add(this.lbTextName);
            this.Name = "InputParametersElement";
            this.Text = "Вводные параметры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTextName;
        private System.Windows.Forms.TextBox tbElementName;
        private System.Windows.Forms.Button btnApply;
    }
}