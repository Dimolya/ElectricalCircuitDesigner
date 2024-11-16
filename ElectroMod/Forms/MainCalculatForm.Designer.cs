namespace ElectroMod
{
    partial class MainCalculatForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btBus = new System.Windows.Forms.Button();
            this.btTransformator = new System.Windows.Forms.Button();
            this.btRecloser = new System.Windows.Forms.Button();
            this.btLine = new System.Windows.Forms.Button();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.btClear = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteElement = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_DeleteOne = new System.Windows.Forms.Button();
            this.lbR = new System.Windows.Forms.Label();
            this.lbV = new System.Windows.Forms.Label();
            this.lbA = new System.Windows.Forms.Label();
            this.drawPanel1 = new ElectroMod.DrawPanel();
            this.btZoom = new System.Windows.Forms.Button();
            this.menu.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btBus
            // 
            this.btBus.Location = new System.Drawing.Point(12, 56);
            this.btBus.Name = "btBus";
            this.btBus.Size = new System.Drawing.Size(174, 32);
            this.btBus.TabIndex = 1;
            this.btBus.Text = "Шина ПС";
            this.btBus.UseVisualStyleBackColor = true;
            this.btBus.Click += new System.EventHandler(this.btBus_Click);
            // 
            // btTransformator
            // 
            this.btTransformator.Location = new System.Drawing.Point(12, 94);
            this.btTransformator.Name = "btTransformator";
            this.btTransformator.Size = new System.Drawing.Size(174, 32);
            this.btTransformator.TabIndex = 2;
            this.btTransformator.Text = "Трансформатор";
            this.btTransformator.UseVisualStyleBackColor = true;
            this.btTransformator.Click += new System.EventHandler(this.btTransformator_Click);
            // 
            // btRecloser
            // 
            this.btRecloser.Location = new System.Drawing.Point(12, 132);
            this.btRecloser.Name = "btRecloser";
            this.btRecloser.Size = new System.Drawing.Size(174, 32);
            this.btRecloser.TabIndex = 3;
            this.btRecloser.Text = "Реклоузер";
            this.btRecloser.UseVisualStyleBackColor = true;
            this.btRecloser.Click += new System.EventHandler(this.btRecloser_Click);
            // 
            // btLine
            // 
            this.btLine.Location = new System.Drawing.Point(12, 170);
            this.btLine.Name = "btLine";
            this.btLine.Size = new System.Drawing.Size(174, 32);
            this.btLine.TabIndex = 4;
            this.btLine.Text = "Линия";
            this.btLine.UseVisualStyleBackColor = true;
            this.btLine.Click += new System.EventHandler(this.btLine_Click);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.SystemColors.HighlightText;
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1419, 30);
            this.menu.TabIndex = 6;
            this.menu.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFile,
            this.SaveFile});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 26);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // OpenFile
            // 
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(166, 26);
            this.OpenFile.Text = "Открыть";
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // SaveFile
            // 
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.Size = new System.Drawing.Size(166, 26);
            this.SaveFile.Text = "Сохранить";
            this.SaveFile.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(15, 502);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(171, 34);
            this.btClear.TabIndex = 18;
            this.btClear.Text = "Очистить поле";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteElement});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 28);
            // 
            // deleteElement
            // 
            this.deleteElement.Name = "deleteElement";
            this.deleteElement.Size = new System.Drawing.Size(134, 24);
            this.deleteElement.Text = "Удалить";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(189, 416);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 16);
            this.label6.TabIndex = 27;
            this.label6.Text = "Ом";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(189, 383);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 26;
            this.label5.Text = "B";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(189, 346);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 413);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Сопротивление:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 380);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "Напряжение:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 19;
            this.label1.Text = "Сила тока:";
            // 
            // bt_DeleteOne
            // 
            this.bt_DeleteOne.Location = new System.Drawing.Point(15, 452);
            this.bt_DeleteOne.Name = "bt_DeleteOne";
            this.bt_DeleteOne.Size = new System.Drawing.Size(171, 44);
            this.bt_DeleteOne.TabIndex = 28;
            this.bt_DeleteOne.Text = "Удалить выделенный элемент";
            this.bt_DeleteOne.UseVisualStyleBackColor = true;
            this.bt_DeleteOne.Click += new System.EventHandler(this.bt_DeleteOne_Click);
            // 
            // lbR
            // 
            this.lbR.AutoSize = true;
            this.lbR.Location = new System.Drawing.Point(130, 416);
            this.lbR.Name = "lbR";
            this.lbR.Size = new System.Drawing.Size(14, 16);
            this.lbR.TabIndex = 29;
            this.lbR.Text = "0";
            this.lbR.TextChanged += new System.EventHandler(this.lbR_TextChanged);
            // 
            // lbV
            // 
            this.lbV.AutoSize = true;
            this.lbV.Location = new System.Drawing.Point(130, 383);
            this.lbV.Name = "lbV";
            this.lbV.Size = new System.Drawing.Size(14, 16);
            this.lbV.TabIndex = 30;
            this.lbV.Text = "0";
            this.lbV.TextChanged += new System.EventHandler(this.lbV_TextChanged);
            // 
            // lbA
            // 
            this.lbA.AutoSize = true;
            this.lbA.Location = new System.Drawing.Point(130, 346);
            this.lbA.Name = "lbA";
            this.lbA.Size = new System.Drawing.Size(14, 16);
            this.lbA.TabIndex = 31;
            this.lbA.Text = "0";
            this.lbA.TextChanged += new System.EventHandler(this.lbA_TextChanged);
            // 
            // drawPanel1
            // 
            this.drawPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.drawPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.drawPanel1.Location = new System.Drawing.Point(226, 31);
            this.drawPanel1.Name = "drawPanel1";
            this.drawPanel1.Size = new System.Drawing.Size(1181, 510);
            this.drawPanel1.TabIndex = 0;
            // 
            // btZoom
            // 
            this.btZoom.Location = new System.Drawing.Point(1352, 547);
            this.btZoom.Name = "btZoom";
            this.btZoom.Size = new System.Drawing.Size(55, 23);
            this.btZoom.TabIndex = 32;
            this.btZoom.Text = "100%";
            this.btZoom.UseVisualStyleBackColor = true;
            this.btZoom.Click += new System.EventHandler(this.btZoom_Click);
            // 
            // MainCalculatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 578);
            this.Controls.Add(this.btZoom);
            this.Controls.Add(this.lbA);
            this.Controls.Add(this.lbV);
            this.Controls.Add(this.lbR);
            this.Controls.Add(this.bt_DeleteOne);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.btLine);
            this.Controls.Add(this.btRecloser);
            this.Controls.Add(this.btTransformator);
            this.Controls.Add(this.btBus);
            this.Controls.Add(this.drawPanel1);
            this.Controls.Add(this.menu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menu;
            this.MaximizeBox = false;
            this.Name = "MainCalculatForm";
            this.Text = "Моделирование электрических цепей";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DrawPanel drawPanel1;
        private System.Windows.Forms.Button btBus;
        private System.Windows.Forms.Button btTransformator;
        private System.Windows.Forms.Button btRecloser;
        private System.Windows.Forms.Button btLine;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFile;
        private System.Windows.Forms.ToolStripMenuItem SaveFile;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteElement;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_DeleteOne;
        private System.Windows.Forms.Label lbR;
        private System.Windows.Forms.Label lbV;
        private System.Windows.Forms.Label lbA;
        private System.Windows.Forms.Button btZoom;
    }
}

