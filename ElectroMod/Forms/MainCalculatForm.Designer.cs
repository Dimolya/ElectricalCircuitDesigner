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
            this.bt_DeleteOne = new System.Windows.Forms.Button();
            this.btZoom = new System.Windows.Forms.Button();
            this.panelPropertyBus = new System.Windows.Forms.Panel();
            this.cbBusVoltage = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panelForResistance = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbBusActiveResistMin = new System.Windows.Forms.TextBox();
            this.tbBusReactiveResistMin = new System.Windows.Forms.TextBox();
            this.tbActiveResistMin = new System.Windows.Forms.Label();
            this.tbReactiveResistMin = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbBusReactiveResistMax = new System.Windows.Forms.TextBox();
            this.tbBusActiveResistMax = new System.Windows.Forms.TextBox();
            this.panelForCurrent = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbBusCurrentMin = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tbBusCurrentMax = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbBusResistance = new System.Windows.Forms.RadioButton();
            this.rbBusCurrent = new System.Windows.Forms.RadioButton();
            this.lbVoltage = new System.Windows.Forms.Label();
            this.lbDataTypes = new System.Windows.Forms.Label();
            this.tbBusName = new System.Windows.Forms.TextBox();
            this.lbTextName = new System.Windows.Forms.Label();
            this.panelPropertyLine = new System.Windows.Forms.Panel();
            this.tbLineLength = new System.Windows.Forms.TextBox();
            this.lbLength = new System.Windows.Forms.Label();
            this.lbMarks = new System.Windows.Forms.Label();
            this.cbLineMarks = new System.Windows.Forms.ComboBox();
            this.tbLineName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.panelPropertyRecloser = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.cbIsCalculate = new System.Windows.Forms.CheckBox();
            this.lbTypeTT = new System.Windows.Forms.Label();
            this.lbTypeRecloser = new System.Windows.Forms.Label();
            this.cbRecloserTypeTT = new System.Windows.Forms.ComboBox();
            this.cbRecloserType = new System.Windows.Forms.ComboBox();
            this.tbRecloserName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.panelPropertyTransformator = new System.Windows.Forms.Panel();
            this.lbShemeConnectWinding = new System.Windows.Forms.Label();
            this.lbTypeKTP = new System.Windows.Forms.Label();
            this.cbTransformatorSchemes = new System.Windows.Forms.ComboBox();
            this.cbTransformatorTypesKTP = new System.Windows.Forms.ComboBox();
            this.tbTransformatorName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.btnSaveProp = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.drawPanel1 = new ElectroMod.DrawPanel();
            this.menu.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panelPropertyBus.SuspendLayout();
            this.panelForResistance.SuspendLayout();
            this.panelForCurrent.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelPropertyLine.SuspendLayout();
            this.panelPropertyRecloser.SuspendLayout();
            this.panelPropertyTransformator.SuspendLayout();
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
            this.menu.Size = new System.Drawing.Size(1406, 28);
            this.menu.TabIndex = 6;
            this.menu.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFile,
            this.SaveFile});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // OpenFile
            // 
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(224, 26);
            this.OpenFile.Text = "Открыть";
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // SaveFile
            // 
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.Size = new System.Drawing.Size(224, 26);
            this.SaveFile.Text = "Сохранить";
            this.SaveFile.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(15, 439);
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
            // bt_DeleteOne
            // 
            this.bt_DeleteOne.Location = new System.Drawing.Point(15, 389);
            this.bt_DeleteOne.Name = "bt_DeleteOne";
            this.bt_DeleteOne.Size = new System.Drawing.Size(171, 44);
            this.bt_DeleteOne.TabIndex = 28;
            this.bt_DeleteOne.Text = "Удалить выделенный элемент";
            this.bt_DeleteOne.UseVisualStyleBackColor = true;
            this.bt_DeleteOne.Click += new System.EventHandler(this.bt_DeleteOne_Click);
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
            // panelPropertyBus
            // 
            this.panelPropertyBus.BackColor = System.Drawing.SystemColors.Control;
            this.panelPropertyBus.Controls.Add(this.cbBusVoltage);
            this.panelPropertyBus.Controls.Add(this.label9);
            this.panelPropertyBus.Controls.Add(this.panelForResistance);
            this.panelPropertyBus.Controls.Add(this.panelForCurrent);
            this.panelPropertyBus.Controls.Add(this.panel1);
            this.panelPropertyBus.Controls.Add(this.lbVoltage);
            this.panelPropertyBus.Controls.Add(this.lbDataTypes);
            this.panelPropertyBus.Controls.Add(this.tbBusName);
            this.panelPropertyBus.Controls.Add(this.lbTextName);
            this.panelPropertyBus.Location = new System.Drawing.Point(226, 547);
            this.panelPropertyBus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPropertyBus.Name = "panelPropertyBus";
            this.panelPropertyBus.Size = new System.Drawing.Size(800, 235);
            this.panelPropertyBus.TabIndex = 33;
            this.panelPropertyBus.Visible = false;
            // 
            // cbBusVoltage
            // 
            this.cbBusVoltage.FormattingEnabled = true;
            this.cbBusVoltage.Location = new System.Drawing.Point(196, 30);
            this.cbBusVoltage.Name = "cbBusVoltage";
            this.cbBusVoltage.Size = new System.Drawing.Size(121, 24);
            this.cbBusVoltage.TabIndex = 52;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(323, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 16);
            this.label9.TabIndex = 51;
            this.label9.Text = "кВ";
            // 
            // panelForResistance
            // 
            this.panelForResistance.Controls.Add(this.label12);
            this.panelForResistance.Controls.Add(this.label11);
            this.panelForResistance.Controls.Add(this.tbBusActiveResistMin);
            this.panelForResistance.Controls.Add(this.tbBusReactiveResistMin);
            this.panelForResistance.Controls.Add(this.tbActiveResistMin);
            this.panelForResistance.Controls.Add(this.tbReactiveResistMin);
            this.panelForResistance.Controls.Add(this.label8);
            this.panelForResistance.Controls.Add(this.label7);
            this.panelForResistance.Controls.Add(this.label10);
            this.panelForResistance.Controls.Add(this.label13);
            this.panelForResistance.Controls.Add(this.tbBusReactiveResistMax);
            this.panelForResistance.Controls.Add(this.tbBusActiveResistMax);
            this.panelForResistance.Location = new System.Drawing.Point(3, 106);
            this.panelForResistance.Name = "panelForResistance";
            this.panelForResistance.Size = new System.Drawing.Size(592, 125);
            this.panelForResistance.TabIndex = 50;
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
            // tbBusActiveResistMin
            // 
            this.tbBusActiveResistMin.Location = new System.Drawing.Point(457, 63);
            this.tbBusActiveResistMin.Name = "tbBusActiveResistMin";
            this.tbBusActiveResistMin.Size = new System.Drawing.Size(100, 22);
            this.tbBusActiveResistMin.TabIndex = 45;
            // 
            // tbBusReactiveResistMin
            // 
            this.tbBusReactiveResistMin.Location = new System.Drawing.Point(457, 91);
            this.tbBusReactiveResistMin.Name = "tbBusReactiveResistMin";
            this.tbBusReactiveResistMin.Size = new System.Drawing.Size(100, 22);
            this.tbBusReactiveResistMin.TabIndex = 44;
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(563, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 16);
            this.label7.TabIndex = 40;
            this.label7.Text = "Ом";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(563, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 16);
            this.label10.TabIndex = 39;
            this.label10.Text = "Ом";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(392, 16);
            this.label13.TabIndex = 35;
            this.label13.Text = "Введите активное сопротивление системы в макс. режиме";
            // 
            // tbBusReactiveResistMax
            // 
            this.tbBusReactiveResistMax.Location = new System.Drawing.Point(457, 35);
            this.tbBusReactiveResistMax.Name = "tbBusReactiveResistMax";
            this.tbBusReactiveResistMax.Size = new System.Drawing.Size(100, 22);
            this.tbBusReactiveResistMax.TabIndex = 38;
            // 
            // tbBusActiveResistMax
            // 
            this.tbBusActiveResistMax.Location = new System.Drawing.Point(457, 8);
            this.tbBusActiveResistMax.Name = "tbBusActiveResistMax";
            this.tbBusActiveResistMax.Size = new System.Drawing.Size(100, 22);
            this.tbBusActiveResistMax.TabIndex = 37;
            // 
            // panelForCurrent
            // 
            this.panelForCurrent.Controls.Add(this.label14);
            this.panelForCurrent.Controls.Add(this.label15);
            this.panelForCurrent.Controls.Add(this.label16);
            this.panelForCurrent.Controls.Add(this.tbBusCurrentMin);
            this.panelForCurrent.Controls.Add(this.label17);
            this.panelForCurrent.Controls.Add(this.tbBusCurrentMax);
            this.panelForCurrent.Location = new System.Drawing.Point(4, 106);
            this.panelForCurrent.Name = "panelForCurrent";
            this.panelForCurrent.Size = new System.Drawing.Size(465, 69);
            this.panelForCurrent.TabIndex = 49;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(368, 41);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 16);
            this.label14.TabIndex = 40;
            this.label14.Text = "кА";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(368, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 16);
            this.label15.TabIndex = 39;
            this.label15.Text = "кА";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(215, 16);
            this.label16.TabIndex = 35;
            this.label16.Text = "Введите ток К.З. а макс. режиме";
            // 
            // tbBusCurrentMin
            // 
            this.tbBusCurrentMin.Location = new System.Drawing.Point(262, 38);
            this.tbBusCurrentMin.Name = "tbBusCurrentMin";
            this.tbBusCurrentMin.Size = new System.Drawing.Size(100, 22);
            this.tbBusCurrentMin.TabIndex = 38;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(209, 16);
            this.label17.TabIndex = 36;
            this.label17.Text = "Введите ток К.З. а мин. режиме";
            // 
            // tbBusCurrentMax
            // 
            this.tbBusCurrentMax.Location = new System.Drawing.Point(262, 8);
            this.tbBusCurrentMax.Name = "tbBusCurrentMax";
            this.tbBusCurrentMax.Size = new System.Drawing.Size(100, 22);
            this.tbBusCurrentMax.TabIndex = 37;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbBusResistance);
            this.panel1.Controls.Add(this.rbBusCurrent);
            this.panel1.Location = new System.Drawing.Point(198, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 39);
            this.panel1.TabIndex = 48;
            // 
            // rbBusResistance
            // 
            this.rbBusResistance.AutoSize = true;
            this.rbBusResistance.Location = new System.Drawing.Point(107, 3);
            this.rbBusResistance.Name = "rbBusResistance";
            this.rbBusResistance.Size = new System.Drawing.Size(132, 20);
            this.rbBusResistance.TabIndex = 33;
            this.rbBusResistance.TabStop = true;
            this.rbBusResistance.Text = "Сопротивление";
            this.rbBusResistance.UseVisualStyleBackColor = true;
            this.rbBusResistance.CheckedChanged += new System.EventHandler(this.rbBusResistance_CheckedChanged);
            // 
            // rbBusCurrent
            // 
            this.rbBusCurrent.AutoSize = true;
            this.rbBusCurrent.Checked = true;
            this.rbBusCurrent.Location = new System.Drawing.Point(3, 3);
            this.rbBusCurrent.Name = "rbBusCurrent";
            this.rbBusCurrent.Size = new System.Drawing.Size(52, 20);
            this.rbBusCurrent.TabIndex = 32;
            this.rbBusCurrent.TabStop = true;
            this.rbBusCurrent.Text = "Ток";
            this.rbBusCurrent.UseVisualStyleBackColor = true;
            this.rbBusCurrent.CheckedChanged += new System.EventHandler(this.rbBusCurrent_CheckedChanged);
            // 
            // lbVoltage
            // 
            this.lbVoltage.AutoSize = true;
            this.lbVoltage.Location = new System.Drawing.Point(-1, 34);
            this.lbVoltage.Name = "lbVoltage";
            this.lbVoltage.Size = new System.Drawing.Size(89, 16);
            this.lbVoltage.TabIndex = 46;
            this.lbVoltage.Text = "Напряжение";
            // 
            // lbDataTypes
            // 
            this.lbDataTypes.AutoSize = true;
            this.lbDataTypes.Location = new System.Drawing.Point(-1, 62);
            this.lbDataTypes.Name = "lbDataTypes";
            this.lbDataTypes.Size = new System.Drawing.Size(123, 16);
            this.lbDataTypes.TabIndex = 45;
            this.lbDataTypes.Text = "Исходные данные";
            // 
            // tbBusName
            // 
            this.tbBusName.Location = new System.Drawing.Point(196, 2);
            this.tbBusName.Name = "tbBusName";
            this.tbBusName.Size = new System.Drawing.Size(400, 22);
            this.tbBusName.TabIndex = 44;
            // 
            // lbTextName
            // 
            this.lbTextName.AutoSize = true;
            this.lbTextName.Location = new System.Drawing.Point(-1, 6);
            this.lbTextName.Name = "lbTextName";
            this.lbTextName.Size = new System.Drawing.Size(106, 16);
            this.lbTextName.TabIndex = 43;
            this.lbTextName.Text = "Наименование";
            // 
            // panelPropertyLine
            // 
            this.panelPropertyLine.BackColor = System.Drawing.SystemColors.Control;
            this.panelPropertyLine.Controls.Add(this.tbLineLength);
            this.panelPropertyLine.Controls.Add(this.lbLength);
            this.panelPropertyLine.Controls.Add(this.lbMarks);
            this.panelPropertyLine.Controls.Add(this.cbLineMarks);
            this.panelPropertyLine.Controls.Add(this.tbLineName);
            this.panelPropertyLine.Controls.Add(this.label18);
            this.panelPropertyLine.Location = new System.Drawing.Point(228, 546);
            this.panelPropertyLine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPropertyLine.Name = "panelPropertyLine";
            this.panelPropertyLine.Size = new System.Drawing.Size(800, 235);
            this.panelPropertyLine.TabIndex = 52;
            this.panelPropertyLine.Visible = false;
            // 
            // tbLineLength
            // 
            this.tbLineLength.Location = new System.Drawing.Point(196, 31);
            this.tbLineLength.Name = "tbLineLength";
            this.tbLineLength.Size = new System.Drawing.Size(273, 22);
            this.tbLineLength.TabIndex = 29;
            // 
            // lbLength
            // 
            this.lbLength.AutoSize = true;
            this.lbLength.Location = new System.Drawing.Point(-1, 34);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(48, 16);
            this.lbLength.TabIndex = 28;
            this.lbLength.Text = "Длина";
            // 
            // lbMarks
            // 
            this.lbMarks.AutoSize = true;
            this.lbMarks.Location = new System.Drawing.Point(-1, 62);
            this.lbMarks.Name = "lbMarks";
            this.lbMarks.Size = new System.Drawing.Size(108, 16);
            this.lbMarks.TabIndex = 27;
            this.lbMarks.Text = "Марка провода";
            // 
            // cbLineMarks
            // 
            this.cbLineMarks.FormattingEnabled = true;
            this.cbLineMarks.Location = new System.Drawing.Point(196, 59);
            this.cbLineMarks.Name = "cbLineMarks";
            this.cbLineMarks.Size = new System.Drawing.Size(273, 24);
            this.cbLineMarks.TabIndex = 26;
            // 
            // tbLineName
            // 
            this.tbLineName.Location = new System.Drawing.Point(196, 3);
            this.tbLineName.Name = "tbLineName";
            this.tbLineName.Size = new System.Drawing.Size(273, 22);
            this.tbLineName.TabIndex = 25;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(-1, 6);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(106, 16);
            this.label18.TabIndex = 24;
            this.label18.Text = "Наименование";
            // 
            // panelPropertyRecloser
            // 
            this.panelPropertyRecloser.BackColor = System.Drawing.SystemColors.Control;
            this.panelPropertyRecloser.Controls.Add(this.label21);
            this.panelPropertyRecloser.Controls.Add(this.cbIsCalculate);
            this.panelPropertyRecloser.Controls.Add(this.lbTypeTT);
            this.panelPropertyRecloser.Controls.Add(this.lbTypeRecloser);
            this.panelPropertyRecloser.Controls.Add(this.cbRecloserTypeTT);
            this.panelPropertyRecloser.Controls.Add(this.cbRecloserType);
            this.panelPropertyRecloser.Controls.Add(this.tbRecloserName);
            this.panelPropertyRecloser.Controls.Add(this.label19);
            this.panelPropertyRecloser.Location = new System.Drawing.Point(228, 545);
            this.panelPropertyRecloser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPropertyRecloser.Name = "panelPropertyRecloser";
            this.panelPropertyRecloser.Size = new System.Drawing.Size(800, 233);
            this.panelPropertyRecloser.TabIndex = 53;
            this.panelPropertyRecloser.Visible = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(1, 97);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(97, 16);
            this.label21.TabIndex = 20;
            this.label21.Text = "Вычисляемый";
            // 
            // cbIsCalculate
            // 
            this.cbIsCalculate.AutoSize = true;
            this.cbIsCalculate.Location = new System.Drawing.Point(196, 97);
            this.cbIsCalculate.Name = "cbIsCalculate";
            this.cbIsCalculate.Size = new System.Drawing.Size(18, 17);
            this.cbIsCalculate.TabIndex = 19;
            this.cbIsCalculate.UseVisualStyleBackColor = true;
            // 
            // lbTypeTT
            // 
            this.lbTypeTT.AutoSize = true;
            this.lbTypeTT.Location = new System.Drawing.Point(0, 64);
            this.lbTypeTT.Name = "lbTypeTT";
            this.lbTypeTT.Size = new System.Drawing.Size(87, 16);
            this.lbTypeTT.TabIndex = 18;
            this.lbTypeTT.Text = "Номинал ТТ";
            // 
            // lbTypeRecloser
            // 
            this.lbTypeRecloser.AutoSize = true;
            this.lbTypeRecloser.Location = new System.Drawing.Point(0, 34);
            this.lbTypeRecloser.Name = "lbTypeRecloser";
            this.lbTypeRecloser.Size = new System.Drawing.Size(32, 16);
            this.lbTypeRecloser.TabIndex = 17;
            this.lbTypeRecloser.Text = "Тип";
            // 
            // cbRecloserTypeTT
            // 
            this.cbRecloserTypeTT.FormattingEnabled = true;
            this.cbRecloserTypeTT.Location = new System.Drawing.Point(196, 61);
            this.cbRecloserTypeTT.Name = "cbRecloserTypeTT";
            this.cbRecloserTypeTT.Size = new System.Drawing.Size(273, 24);
            this.cbRecloserTypeTT.TabIndex = 16;
            // 
            // cbRecloserType
            // 
            this.cbRecloserType.FormattingEnabled = true;
            this.cbRecloserType.Location = new System.Drawing.Point(196, 31);
            this.cbRecloserType.Name = "cbRecloserType";
            this.cbRecloserType.Size = new System.Drawing.Size(273, 24);
            this.cbRecloserType.TabIndex = 15;
            // 
            // tbRecloserName
            // 
            this.tbRecloserName.Location = new System.Drawing.Point(196, 4);
            this.tbRecloserName.Name = "tbRecloserName";
            this.tbRecloserName.Size = new System.Drawing.Size(273, 22);
            this.tbRecloserName.TabIndex = 14;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(0, 5);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(106, 16);
            this.label19.TabIndex = 13;
            this.label19.Text = "Наименование";
            // 
            // panelPropertyTransformator
            // 
            this.panelPropertyTransformator.BackColor = System.Drawing.SystemColors.Control;
            this.panelPropertyTransformator.Controls.Add(this.lbShemeConnectWinding);
            this.panelPropertyTransformator.Controls.Add(this.lbTypeKTP);
            this.panelPropertyTransformator.Controls.Add(this.cbTransformatorSchemes);
            this.panelPropertyTransformator.Controls.Add(this.cbTransformatorTypesKTP);
            this.panelPropertyTransformator.Controls.Add(this.tbTransformatorName);
            this.panelPropertyTransformator.Controls.Add(this.label20);
            this.panelPropertyTransformator.Location = new System.Drawing.Point(228, 545);
            this.panelPropertyTransformator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPropertyTransformator.Name = "panelPropertyTransformator";
            this.panelPropertyTransformator.Size = new System.Drawing.Size(800, 235);
            this.panelPropertyTransformator.TabIndex = 54;
            this.panelPropertyTransformator.Visible = false;
            // 
            // lbShemeConnectWinding
            // 
            this.lbShemeConnectWinding.AutoSize = true;
            this.lbShemeConnectWinding.Location = new System.Drawing.Point(2, 66);
            this.lbShemeConnectWinding.Name = "lbShemeConnectWinding";
            this.lbShemeConnectWinding.Size = new System.Drawing.Size(186, 16);
            this.lbShemeConnectWinding.TabIndex = 12;
            this.lbShemeConnectWinding.Text = "Схема соединения обмоток";
            // 
            // lbTypeKTP
            // 
            this.lbTypeKTP.AutoSize = true;
            this.lbTypeKTP.Location = new System.Drawing.Point(2, 36);
            this.lbTypeKTP.Name = "lbTypeKTP";
            this.lbTypeKTP.Size = new System.Drawing.Size(62, 16);
            this.lbTypeKTP.TabIndex = 11;
            this.lbTypeKTP.Text = "Тип КТП";
            // 
            // cbTransformatorSchemes
            // 
            this.cbTransformatorSchemes.FormattingEnabled = true;
            this.cbTransformatorSchemes.Location = new System.Drawing.Point(205, 63);
            this.cbTransformatorSchemes.Name = "cbTransformatorSchemes";
            this.cbTransformatorSchemes.Size = new System.Drawing.Size(273, 24);
            this.cbTransformatorSchemes.TabIndex = 10;
            // 
            // cbTransformatorTypesKTP
            // 
            this.cbTransformatorTypesKTP.FormattingEnabled = true;
            this.cbTransformatorTypesKTP.Location = new System.Drawing.Point(205, 33);
            this.cbTransformatorTypesKTP.Name = "cbTransformatorTypesKTP";
            this.cbTransformatorTypesKTP.Size = new System.Drawing.Size(273, 24);
            this.cbTransformatorTypesKTP.TabIndex = 9;
            // 
            // tbTransformatorName
            // 
            this.tbTransformatorName.Location = new System.Drawing.Point(205, 6);
            this.tbTransformatorName.Name = "tbTransformatorName";
            this.tbTransformatorName.Size = new System.Drawing.Size(273, 22);
            this.tbTransformatorName.TabIndex = 8;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(2, 7);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(106, 16);
            this.label20.TabIndex = 7;
            this.label20.Text = "Наименование";
            // 
            // btnSaveProp
            // 
            this.btnSaveProp.Location = new System.Drawing.Point(15, 493);
            this.btnSaveProp.Name = "btnSaveProp";
            this.btnSaveProp.Size = new System.Drawing.Size(171, 48);
            this.btnSaveProp.TabIndex = 55;
            this.btnSaveProp.Text = "Сохранить свойства элемента";
            this.btnSaveProp.UseVisualStyleBackColor = true;
            this.btnSaveProp.Click += new System.EventHandler(this.btnSaveProp_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(1223, 593);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(171, 48);
            this.btnCalculate.TabIndex = 56;
            this.btnCalculate.Text = "Расчет";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
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
            // MainCalculatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1406, 844);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.btnSaveProp);
            this.Controls.Add(this.panelPropertyTransformator);
            this.Controls.Add(this.panelPropertyRecloser);
            this.Controls.Add(this.panelPropertyLine);
            this.Controls.Add(this.panelPropertyBus);
            this.Controls.Add(this.btZoom);
            this.Controls.Add(this.bt_DeleteOne);
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
            this.panelPropertyBus.ResumeLayout(false);
            this.panelPropertyBus.PerformLayout();
            this.panelForResistance.ResumeLayout(false);
            this.panelForResistance.PerformLayout();
            this.panelForCurrent.ResumeLayout(false);
            this.panelForCurrent.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelPropertyLine.ResumeLayout(false);
            this.panelPropertyLine.PerformLayout();
            this.panelPropertyRecloser.ResumeLayout(false);
            this.panelPropertyRecloser.PerformLayout();
            this.panelPropertyTransformator.ResumeLayout(false);
            this.panelPropertyTransformator.PerformLayout();
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
        private System.Windows.Forms.Button bt_DeleteOne;
        private System.Windows.Forms.Button btZoom;
        private System.Windows.Forms.Panel panelPropertyBus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panelForResistance;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbBusActiveResistMin;
        private System.Windows.Forms.TextBox tbBusReactiveResistMin;
        private System.Windows.Forms.Label tbActiveResistMin;
        private System.Windows.Forms.Label tbReactiveResistMin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbBusReactiveResistMax;
        private System.Windows.Forms.TextBox tbBusActiveResistMax;
        private System.Windows.Forms.Panel panelForCurrent;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbBusCurrentMin;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbBusCurrentMax;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbBusResistance;
        private System.Windows.Forms.RadioButton rbBusCurrent;
        private System.Windows.Forms.Label lbVoltage;
        private System.Windows.Forms.Label lbDataTypes;
        private System.Windows.Forms.TextBox tbBusName;
        private System.Windows.Forms.Label lbTextName;
        private System.Windows.Forms.Panel panelPropertyLine;
        private System.Windows.Forms.TextBox tbLineLength;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Label lbMarks;
        private System.Windows.Forms.ComboBox cbLineMarks;
        private System.Windows.Forms.TextBox tbLineName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panelPropertyRecloser;
        private System.Windows.Forms.Label lbTypeTT;
        private System.Windows.Forms.Label lbTypeRecloser;
        private System.Windows.Forms.ComboBox cbRecloserTypeTT;
        private System.Windows.Forms.ComboBox cbRecloserType;
        private System.Windows.Forms.TextBox tbRecloserName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panelPropertyTransformator;
        private System.Windows.Forms.Label lbShemeConnectWinding;
        private System.Windows.Forms.Label lbTypeKTP;
        private System.Windows.Forms.ComboBox cbTransformatorSchemes;
        private System.Windows.Forms.ComboBox cbTransformatorTypesKTP;
        private System.Windows.Forms.TextBox tbTransformatorName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnSaveProp;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.ComboBox cbBusVoltage;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox cbIsCalculate;
    }
}

