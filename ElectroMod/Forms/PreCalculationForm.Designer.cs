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
            this.cbReconnect = new System.Windows.Forms.ComboBox();
            this.panelHasTY = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPowerKBT = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbPowerSuchKBT = new System.Windows.Forms.TextBox();
            this.tbNumberTY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelNotTY = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbPowerKBA = new System.Windows.Forms.TextBox();
            this.tbPowerSuchKBA = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panelHasTY.SuspendLayout();
            this.panelNotTY.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFormCalculate
            // 
            this.btnFormCalculate.Location = new System.Drawing.Point(511, 262);
            this.btnFormCalculate.Name = "btnFormCalculate";
            this.btnFormCalculate.Size = new System.Drawing.Size(173, 43);
            this.btnFormCalculate.TabIndex = 5;
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
            // cbReconnect
            // 
            this.cbReconnect.FormattingEnabled = true;
            this.cbReconnect.Items.AddRange(new object[] {
            "Расчет по мощности ТУ",
            "Расчет по мощности кВа"});
            this.cbReconnect.Location = new System.Drawing.Point(328, 43);
            this.cbReconnect.Name = "cbReconnect";
            this.cbReconnect.Size = new System.Drawing.Size(291, 24);
            this.cbReconnect.TabIndex = 10;
            this.cbReconnect.Text = "Расчет по мощности ТУ";
            this.cbReconnect.SelectedIndexChanged += new System.EventHandler(this.cbReconnect_SelectedIndexChanged);
            // 
            // panelHasTY
            // 
            this.panelHasTY.Controls.Add(this.label4);
            this.panelHasTY.Controls.Add(this.label5);
            this.panelHasTY.Controls.Add(this.label8);
            this.panelHasTY.Controls.Add(this.tbPowerKBT);
            this.panelHasTY.Controls.Add(this.label9);
            this.panelHasTY.Controls.Add(this.tbPowerSuchKBT);
            this.panelHasTY.Controls.Add(this.tbNumberTY);
            this.panelHasTY.Controls.Add(this.label3);
            this.panelHasTY.Location = new System.Drawing.Point(12, 88);
            this.panelHasTY.Name = "panelHasTY";
            this.panelHasTY.Size = new System.Drawing.Size(607, 126);
            this.panelHasTY.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Введите мощность";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(374, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "кВт";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(373, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 16);
            this.label8.TabIndex = 7;
            this.label8.Text = "кВт";
            // 
            // tbPowerKBT
            // 
            this.tbPowerKBT.Location = new System.Drawing.Point(248, 82);
            this.tbPowerKBT.Name = "tbPowerKBT";
            this.tbPowerKBT.Size = new System.Drawing.Size(115, 22);
            this.tbPowerKBT.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(224, 16);
            this.label9.TabIndex = 5;
            this.label9.Text = "Введите значение сущ. нагрузки";
            // 
            // tbPowerSuchKBT
            // 
            this.tbPowerSuchKBT.Location = new System.Drawing.Point(248, 50);
            this.tbPowerSuchKBT.Name = "tbPowerSuchKBT";
            this.tbPowerSuchKBT.Size = new System.Drawing.Size(115, 22);
            this.tbPowerSuchKBT.TabIndex = 1;
            // 
            // tbNumberTY
            // 
            this.tbNumberTY.Location = new System.Drawing.Point(145, 14);
            this.tbNumberTY.Name = "tbNumberTY";
            this.tbNumberTY.Size = new System.Drawing.Size(437, 22);
            this.tbNumberTY.TabIndex = 0;
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
            // panelNotTY
            // 
            this.panelNotTY.Controls.Add(this.label6);
            this.panelNotTY.Controls.Add(this.label10);
            this.panelNotTY.Controls.Add(this.tbPowerKBA);
            this.panelNotTY.Controls.Add(this.tbPowerSuchKBA);
            this.panelNotTY.Controls.Add(this.label7);
            this.panelNotTY.Controls.Add(this.label11);
            this.panelNotTY.Location = new System.Drawing.Point(12, 88);
            this.panelNotTY.Name = "panelNotTY";
            this.panelNotTY.Size = new System.Drawing.Size(607, 126);
            this.panelNotTY.TabIndex = 9;
            this.panelNotTY.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(370, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "кВа";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(370, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 16);
            this.label10.TabIndex = 7;
            this.label10.Text = "кВа";
            // 
            // tbPowerKBA
            // 
            this.tbPowerKBA.Location = new System.Drawing.Point(245, 47);
            this.tbPowerKBA.Name = "tbPowerKBA";
            this.tbPowerKBA.Size = new System.Drawing.Size(115, 22);
            this.tbPowerKBA.TabIndex = 1;
            // 
            // tbPowerSuchKBA
            // 
            this.tbPowerSuchKBA.Location = new System.Drawing.Point(245, 17);
            this.tbPowerSuchKBA.Name = "tbPowerSuchKBA";
            this.tbPowerSuchKBA.Size = new System.Drawing.Size(115, 22);
            this.tbPowerSuchKBA.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(159, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Введите мощность КТП";
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
            // PreCalculationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 317);
            this.Controls.Add(this.panelNotTY);
            this.Controls.Add(this.panelHasTY);
            this.Controls.Add(this.cbReconnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFormCalculate);
            this.Name = "PreCalculationForm";
            this.Text = "PreCalculationForm";
            this.panelHasTY.ResumeLayout(false);
            this.panelHasTY.PerformLayout();
            this.panelNotTY.ResumeLayout(false);
            this.panelNotTY.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFormCalculate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbReconnect;
        private System.Windows.Forms.Panel panelHasTY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbPowerKBA;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPowerKBT;
        private System.Windows.Forms.TextBox tbNumberTY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPowerSuchKBT;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panelNotTY;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbPowerSuchKBA;
        private System.Windows.Forms.Label label11;
    }
}