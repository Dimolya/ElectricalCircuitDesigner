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
            this.tbElementName = new System.Windows.Forms.TextBox();
            this.lbTextName = new System.Windows.Forms.Label();
            this.rbCurrent = new System.Windows.Forms.RadioButton();
            this.rbResistance = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCurrentMax = new System.Windows.Forms.TextBox();
            this.tbCurrentMin = new System.Windows.Forms.TextBox();
            this.panelForCurrent = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelForResistance = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.tbActiveResistMin = new System.Windows.Forms.Label();
            this.tbReactiveResistMin = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbReactiveResistMax = new System.Windows.Forms.TextBox();
            this.tbActiveResistMax = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelForCurrent.SuspendLayout();
            this.panelForResistance.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbVoltage
            // 
            this.tbVoltage.Location = new System.Drawing.Point(206, 36);
            this.tbVoltage.Name = "tbVoltage";
            this.tbVoltage.Size = new System.Drawing.Size(91, 22);
            this.tbVoltage.TabIndex = 31;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(516, 278);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(420, 278);
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
            // tbElementName
            // 
            this.tbElementName.Location = new System.Drawing.Point(206, 8);
            this.tbElementName.Name = "tbElementName";
            this.tbElementName.Size = new System.Drawing.Size(400, 22);
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
            // rbCurrent
            // 
            this.rbCurrent.AutoSize = true;
            this.rbCurrent.Checked = true;
            this.rbCurrent.Location = new System.Drawing.Point(3, 3);
            this.rbCurrent.Name = "rbCurrent";
            this.rbCurrent.Size = new System.Drawing.Size(52, 20);
            this.rbCurrent.TabIndex = 32;
            this.rbCurrent.TabStop = true;
            this.rbCurrent.Text = "Ток";
            this.rbCurrent.UseVisualStyleBackColor = true;
            this.rbCurrent.CheckedChanged += new System.EventHandler(this.rbCurrent_CheckedChanged);
            // 
            // rbResistance
            // 
            this.rbResistance.AutoSize = true;
            this.rbResistance.Location = new System.Drawing.Point(107, 3);
            this.rbResistance.Name = "rbResistance";
            this.rbResistance.Size = new System.Drawing.Size(132, 20);
            this.rbResistance.TabIndex = 33;
            this.rbResistance.TabStop = true;
            this.rbResistance.Text = "Сопротивление";
            this.rbResistance.UseVisualStyleBackColor = true;
            this.rbResistance.CheckedChanged += new System.EventHandler(this.rbResistance_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbResistance);
            this.panel1.Controls.Add(this.rbCurrent);
            this.panel1.Location = new System.Drawing.Point(208, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 39);
            this.panel1.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "Введите ток К.З. а макс. режиме";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 16);
            this.label2.TabIndex = 36;
            this.label2.Text = "Введите ток К.З. а мин. режиме";
            // 
            // tbCurrentMax
            // 
            this.tbCurrentMax.Location = new System.Drawing.Point(262, 8);
            this.tbCurrentMax.Name = "tbCurrentMax";
            this.tbCurrentMax.Size = new System.Drawing.Size(100, 22);
            this.tbCurrentMax.TabIndex = 37;
            // 
            // tbCurrentMin
            // 
            this.tbCurrentMin.Location = new System.Drawing.Point(262, 38);
            this.tbCurrentMin.Name = "tbCurrentMin";
            this.tbCurrentMin.Size = new System.Drawing.Size(100, 22);
            this.tbCurrentMin.TabIndex = 38;
            // 
            // panelForCurrent
            // 
            this.panelForCurrent.Controls.Add(this.label4);
            this.panelForCurrent.Controls.Add(this.label3);
            this.panelForCurrent.Controls.Add(this.label1);
            this.panelForCurrent.Controls.Add(this.tbCurrentMin);
            this.panelForCurrent.Controls.Add(this.label2);
            this.panelForCurrent.Controls.Add(this.tbCurrentMax);
            this.panelForCurrent.Location = new System.Drawing.Point(14, 112);
            this.panelForCurrent.Name = "panelForCurrent";
            this.panelForCurrent.Size = new System.Drawing.Size(465, 69);
            this.panelForCurrent.TabIndex = 39;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(368, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 16);
            this.label4.TabIndex = 40;
            this.label4.Text = "кА";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(368, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 16);
            this.label3.TabIndex = 39;
            this.label3.Text = "кА";
            // 
            // panelForResistance
            // 
            this.panelForResistance.Controls.Add(this.label12);
            this.panelForResistance.Controls.Add(this.label11);
            this.panelForResistance.Controls.Add(this.textBox6);
            this.panelForResistance.Controls.Add(this.textBox5);
            this.panelForResistance.Controls.Add(this.tbActiveResistMin);
            this.panelForResistance.Controls.Add(this.tbReactiveResistMin);
            this.panelForResistance.Controls.Add(this.label8);
            this.panelForResistance.Controls.Add(this.label5);
            this.panelForResistance.Controls.Add(this.label6);
            this.panelForResistance.Controls.Add(this.label7);
            this.panelForResistance.Controls.Add(this.tbReactiveResistMax);
            this.panelForResistance.Controls.Add(this.tbActiveResistMax);
            this.panelForResistance.Location = new System.Drawing.Point(12, 112);
            this.panelForResistance.Name = "panelForResistance";
            this.panelForResistance.Size = new System.Drawing.Size(592, 125);
            this.panelForResistance.TabIndex = 41;
            this.panelForResistance.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(563, 94);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 16);
            this.label12.TabIndex = 47;
            this.label12.Text = "Ом";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(563, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 16);
            this.label11.TabIndex = 46;
            this.label11.Text = "Ом";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(457, 63);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 22);
            this.textBox6.TabIndex = 45;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(457, 91);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 22);
            this.textBox5.TabIndex = 44;
            // 
            // tbActiveResistMin
            // 
            this.tbActiveResistMin.AutoSize = true;
            this.tbActiveResistMin.Location = new System.Drawing.Point(4, 66);
            this.tbActiveResistMin.Name = "tbActiveResistMin";
            this.tbActiveResistMin.Size = new System.Drawing.Size(386, 16);
            this.tbActiveResistMin.TabIndex = 43;
            this.tbActiveResistMin.Text = "Введите активное сопротивление системы в мин. режиме";
            // 
            // tbReactiveResistMin
            // 
            this.tbReactiveResistMin.AutoSize = true;
            this.tbReactiveResistMin.Location = new System.Drawing.Point(4, 94);
            this.tbReactiveResistMin.Name = "tbReactiveResistMin";
            this.tbReactiveResistMin.Size = new System.Drawing.Size(402, 16);
            this.tbReactiveResistMin.TabIndex = 42;
            this.tbReactiveResistMin.Text = "Введите реактивное сопротивление системы в мин. режиме";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(408, 16);
            this.label8.TabIndex = 41;
            this.label8.Text = "Введите реактивное сопротивление системы в макс. режиме";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(563, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 16);
            this.label5.TabIndex = 40;
            this.label5.Text = "Ом";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(563, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 16);
            this.label6.TabIndex = 39;
            this.label6.Text = "Ом";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(392, 16);
            this.label7.TabIndex = 35;
            this.label7.Text = "Введите активное сопротивление системы в макс. режиме";
            // 
            // tbReactiveResistMax
            // 
            this.tbReactiveResistMax.Location = new System.Drawing.Point(457, 35);
            this.tbReactiveResistMax.Name = "tbReactiveResistMax";
            this.tbReactiveResistMax.Size = new System.Drawing.Size(100, 22);
            this.tbReactiveResistMax.TabIndex = 38;
            // 
            // tbActiveResistMax
            // 
            this.tbActiveResistMax.Location = new System.Drawing.Point(457, 8);
            this.tbActiveResistMax.Name = "tbActiveResistMax";
            this.tbActiveResistMax.Size = new System.Drawing.Size(100, 22);
            this.tbActiveResistMax.TabIndex = 37;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(303, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 16);
            this.label9.TabIndex = 42;
            this.label9.Text = "кВ";
            // 
            // InputParametersBus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 320);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panelForResistance);
            this.Controls.Add(this.panelForCurrent);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbVoltage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lbVoltage);
            this.Controls.Add(this.lbDataTypes);
            this.Controls.Add(this.tbElementName);
            this.Controls.Add(this.lbTextName);
            this.Name = "InputParametersBus";
            this.Text = "Вводные данные";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelForCurrent.ResumeLayout(false);
            this.panelForCurrent.PerformLayout();
            this.panelForResistance.ResumeLayout(false);
            this.panelForResistance.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbVoltage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lbVoltage;
        private System.Windows.Forms.Label lbDataTypes;
        private System.Windows.Forms.TextBox tbElementName;
        private System.Windows.Forms.Label lbTextName;
        private System.Windows.Forms.RadioButton rbCurrent;
        private System.Windows.Forms.RadioButton rbResistance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCurrentMax;
        private System.Windows.Forms.TextBox tbCurrentMin;
        private System.Windows.Forms.Panel panelForCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelForResistance;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label tbActiveResistMin;
        private System.Windows.Forms.Label tbReactiveResistMin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbReactiveResistMax;
        private System.Windows.Forms.TextBox tbActiveResistMax;
        private System.Windows.Forms.Label label9;
    }
}