namespace ElectroMod.Forms
{
    partial class PreCalculationForm
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
            this.btnFormCalculate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbReconnect = new System.Windows.Forms.ComboBox();
            this.cbEarlyConnect = new System.Windows.Forms.ComboBox();
            this.panelOptionOneReconnect = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPowerKBT = new System.Windows.Forms.TextBox();
            this.tbNumberTY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelOptionTwoReconnect = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPowerKBA = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panelOptionOneEarlyConnect = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panelOptionTwoEarlyConnect = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.tbCapacityKBA = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbCapacityKBT = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panelOptionOneReconnect.SuspendLayout();
            this.panelOptionTwoReconnect.SuspendLayout();
            this.panelOptionOneEarlyConnect.SuspendLayout();
            this.panelOptionTwoEarlyConnect.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFormCalculate
            // 
            this.btnFormCalculate.Location = new System.Drawing.Point(511, 342);
            this.btnFormCalculate.Name = "btnFormCalculate";
            this.btnFormCalculate.Size = new System.Drawing.Size(173, 43);
            this.btnFormCalculate.TabIndex = 0;
            this.btnFormCalculate.Text = "Сформировать расчет";
            this.btnFormCalculate.UseVisualStyleBackColor = true;
            this.btnFormCalculate.Click += new System.EventHandler(this.btnFormCalculate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Выберите тип вновь подключаемой нагрузки";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(306, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Выберите тип ранее подключаемой нагрузки";
            // 
            // cbReconnect
            // 
            this.cbReconnect.FormattingEnabled = true;
            this.cbReconnect.Items.AddRange(new object[] {
            "Расчет по мощности",
            "Расчет по проектируемым нагрузкам"});
            this.cbReconnect.Location = new System.Drawing.Point(328, 43);
            this.cbReconnect.Name = "cbReconnect";
            this.cbReconnect.Size = new System.Drawing.Size(291, 24);
            this.cbReconnect.TabIndex = 3;
            this.cbReconnect.Text = "Расчет по мощности";
            this.cbReconnect.SelectedIndexChanged += new System.EventHandler(this.cbReconnect_SelectedIndexChanged);
            // 
            // cbEarlyConnect
            // 
            this.cbEarlyConnect.FormattingEnabled = true;
            this.cbEarlyConnect.Items.AddRange(new object[] {
            "Расчет по мощности кВт",
            "Расчет по мощности кВа"});
            this.cbEarlyConnect.Location = new System.Drawing.Point(329, 188);
            this.cbEarlyConnect.Name = "cbEarlyConnect";
            this.cbEarlyConnect.Size = new System.Drawing.Size(208, 24);
            this.cbEarlyConnect.TabIndex = 4;
            this.cbEarlyConnect.Text = "Расчет по мощности кВт";
            this.cbEarlyConnect.SelectedIndexChanged += new System.EventHandler(this.cbEarlyConnect_SelectedIndexChanged);
            // 
            // panelOptionOneReconnect
            // 
            this.panelOptionOneReconnect.Controls.Add(this.label5);
            this.panelOptionOneReconnect.Controls.Add(this.tbPowerKBT);
            this.panelOptionOneReconnect.Controls.Add(this.tbNumberTY);
            this.panelOptionOneReconnect.Controls.Add(this.label4);
            this.panelOptionOneReconnect.Controls.Add(this.label3);
            this.panelOptionOneReconnect.Location = new System.Drawing.Point(12, 91);
            this.panelOptionOneReconnect.Name = "panelOptionOneReconnect";
            this.panelOptionOneReconnect.Size = new System.Drawing.Size(604, 59);
            this.panelOptionOneReconnect.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(522, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "кВт";
            // 
            // tbPowerKBT
            // 
            this.tbPowerKBT.Location = new System.Drawing.Point(401, 14);
            this.tbPowerKBT.Name = "tbPowerKBT";
            this.tbPowerKBT.Size = new System.Drawing.Size(115, 22);
            this.tbPowerKBT.TabIndex = 3;
            // 
            // tbNumberTY
            // 
            this.tbNumberTY.Location = new System.Drawing.Point(120, 14);
            this.tbNumberTY.Name = "tbNumberTY";
            this.tbNumberTY.Size = new System.Drawing.Size(115, 22);
            this.tbNumberTY.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Введите мощность";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Введите № ТУ";
            // 
            // panelOptionTwoReconnect
            // 
            this.panelOptionTwoReconnect.Controls.Add(this.label6);
            this.panelOptionTwoReconnect.Controls.Add(this.tbPowerKBA);
            this.panelOptionTwoReconnect.Controls.Add(this.label7);
            this.panelOptionTwoReconnect.Location = new System.Drawing.Point(12, 91);
            this.panelOptionTwoReconnect.Name = "panelOptionTwoReconnect";
            this.panelOptionTwoReconnect.Size = new System.Drawing.Size(646, 59);
            this.panelOptionTwoReconnect.TabIndex = 6;
            this.panelOptionTwoReconnect.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(266, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "кВа";
            // 
            // tbPowerKBA
            // 
            this.tbPowerKBA.Location = new System.Drawing.Point(145, 17);
            this.tbPowerKBA.Name = "tbPowerKBA";
            this.tbPowerKBA.Size = new System.Drawing.Size(115, 22);
            this.tbPowerKBA.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Введите мощность";
            // 
            // panelOptionOneEarlyConnect
            // 
            this.panelOptionOneEarlyConnect.Controls.Add(this.label8);
            this.panelOptionOneEarlyConnect.Controls.Add(this.tbCapacityKBT);
            this.panelOptionOneEarlyConnect.Controls.Add(this.label9);
            this.panelOptionOneEarlyConnect.Location = new System.Drawing.Point(12, 223);
            this.panelOptionOneEarlyConnect.Name = "panelOptionOneEarlyConnect";
            this.panelOptionOneEarlyConnect.Size = new System.Drawing.Size(582, 59);
            this.panelOptionOneEarlyConnect.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(365, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 16);
            this.label8.TabIndex = 7;
            this.label8.Text = "кВт";
            // 
            // panelOptionTwoEarlyConnect
            // 
            this.panelOptionTwoEarlyConnect.Controls.Add(this.label10);
            this.panelOptionTwoEarlyConnect.Controls.Add(this.tbCapacityKBA);
            this.panelOptionTwoEarlyConnect.Controls.Add(this.label11);
            this.panelOptionTwoEarlyConnect.Location = new System.Drawing.Point(12, 223);
            this.panelOptionTwoEarlyConnect.Name = "panelOptionTwoEarlyConnect";
            this.panelOptionTwoEarlyConnect.Size = new System.Drawing.Size(582, 59);
            this.panelOptionTwoEarlyConnect.TabIndex = 9;
            this.panelOptionTwoEarlyConnect.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(365, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 16);
            this.label10.TabIndex = 7;
            this.label10.Text = "кВа";
            // 
            // tbCapacityKBA
            // 
            this.tbCapacityKBA.Location = new System.Drawing.Point(240, 17);
            this.tbCapacityKBA.Name = "tbCapacityKBA";
            this.tbCapacityKBA.Size = new System.Drawing.Size(115, 22);
            this.tbCapacityKBA.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(224, 16);
            this.label11.TabIndex = 5;
            this.label11.Text = "Введите значение сущ. нагрузки";
            // 
            // tbCapacityKBT
            // 
            this.tbCapacityKBT.Location = new System.Drawing.Point(240, 17);
            this.tbCapacityKBT.Name = "tbCapacityKBT";
            this.tbCapacityKBT.Size = new System.Drawing.Size(115, 22);
            this.tbCapacityKBT.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(224, 16);
            this.label9.TabIndex = 5;
            this.label9.Text = "Введите значение сущ. нагрузки";
            // 
            // PreCalculationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 397);
            this.Controls.Add(this.panelOptionTwoReconnect);
            this.Controls.Add(this.panelOptionTwoEarlyConnect);
            this.Controls.Add(this.panelOptionOneEarlyConnect);
            this.Controls.Add(this.panelOptionOneReconnect);
            this.Controls.Add(this.cbEarlyConnect);
            this.Controls.Add(this.cbReconnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFormCalculate);
            this.Name = "PreCalculationForm";
            this.Text = "PreCalculationForm";
            this.panelOptionOneReconnect.ResumeLayout(false);
            this.panelOptionOneReconnect.PerformLayout();
            this.panelOptionTwoReconnect.ResumeLayout(false);
            this.panelOptionTwoReconnect.PerformLayout();
            this.panelOptionOneEarlyConnect.ResumeLayout(false);
            this.panelOptionOneEarlyConnect.PerformLayout();
            this.panelOptionTwoEarlyConnect.ResumeLayout(false);
            this.panelOptionTwoEarlyConnect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFormCalculate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbReconnect;
        private System.Windows.Forms.ComboBox cbEarlyConnect;
        private System.Windows.Forms.Panel panelOptionOneReconnect;
        private System.Windows.Forms.Panel panelOptionTwoReconnect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbPowerKBA;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPowerKBT;
        private System.Windows.Forms.TextBox tbNumberTY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelOptionOneEarlyConnect;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbCapacityKBT;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panelOptionTwoEarlyConnect;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbCapacityKBA;
        private System.Windows.Forms.Label label11;
    }
}