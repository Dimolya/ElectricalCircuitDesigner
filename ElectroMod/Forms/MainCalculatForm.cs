﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ElectroMod.Forms;
using ElectroMod.DataBase.Dtos;
using ElectroMod.DataBase;
using ElectroMod.DataBase.Dtos.StaticDtos;
using ElectroMod.Dtos.StaticDtos;

namespace ElectroMod
{
    [Serializable]
    public partial class MainCalculatForm : Form
    {
        private float _defaultScale = 1.0f;
        private float _currentScale = 1.0f;

        private Elements _elements = new Elements();
        private ISelectable _slctElement;

        public MainCalculatForm()
        {
            InitializeComponent();
            drawPanel1.Build(_elements);
            drawPanel1.ScaleChanged += DrawPanel1_ScaleChanged;
            drawPanel1.ElementSelected += UpdateElementDetails;
            UpdateZoomInfo(1.0f);
        }

        private void DrawPanel1_ScaleChanged(object sender, float scale)
        {
            UpdateZoomInfo(scale);
        }

        private void btBus_Click(object sender, EventArgs e)
        {
            _elements.Add(new Bus(_elements));
            btBus.Enabled = false;
            drawPanel1.Invalidate();
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            _elements.Add(new Line(_elements));
            drawPanel1.Invalidate();
        }

        private void btRecloser_Click(object sender, EventArgs e)
        {
            _elements.Add(new Recloser(_elements));
            drawPanel1.Invalidate();
        }

        private void btTransformator_Click(object sender, EventArgs e)
        {
            _elements.Add(new Transormator(_elements));
            drawPanel1.Invalidate();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Schema|*.schema" };
            if (ofd.ShowDialog(this) == DialogResult.OK)
                using (FileStream fs = File.OpenRead(ofd.FileName))
                {
                    _elements = (Elements)new BinaryFormatter().Deserialize(fs);
                    drawPanel1.Build(_elements);
                }
            drawPanel1.Invalidate();
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Schema|*.schema" };
            if (sfd.ShowDialog(this) == DialogResult.OK)
                using (FileStream fs = File.Create(sfd.FileName))
                    new BinaryFormatter().Serialize(fs, _elements);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Очистить поле?", "Сообщение", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                _elements.Clear();
                drawPanel1.Invalidate();
                btBus.Enabled = true;
            }
        }

        private void bt_DeleteOne_Click(object sender, EventArgs e)
        {
            drawPanel1.RemoveSelected();
            drawPanel1.Invalidate();
            if(_slctElement is Bus)
                btBus.Enabled = true;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            using (var preCalculateForm = new PreCalculationForm())
            {
                if (preCalculateForm.ShowDialog() == DialogResult.OK)
                {
                    var calcul = new CenterCalculation(_elements);
                    calcul.CalculationFormuls();

                    calcul.CalculationMTOandMTZ(preCalculateForm);

                    var docx = new Docx();
                    docx.CreateReportDocument(calcul);
                }
            }
        }

        private void UpdateZoomInfo(float scale)
        {
            btZoom.Text = $"{(scale * 100):0}%";
        }

        private void btZoom_Click(object sender, EventArgs e)
        {
            drawPanel1.SetDefaultScale(_defaultScale);
        }

        private void UpdateElementDetails(ISelectable selectedElement)
        {
            if (selectedElement is Bus bus)
            {
                var directoryPath = @"C:\Users\79871";
                var fileName = "DataBus.json";
                try
                {
                    var file = Directory.GetFiles(directoryPath, fileName, SearchOption.AllDirectories);
                }
                catch(Exception ex)
                {

                }
                var dto = JsonProvider.LoadData<DataBusDto>("..\\..\\DataBase\\StaticData\\DataBus.json");
                _slctElement = bus;
                panelPropertyBus.Visible = true;
                panelPropertyLine.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;
                tbBusName.Text = bus.Name;
                cbBusVoltage.DataSource = dto[0].Voltage;
                cbBusVoltage.DisplayMember = "Voltage";
                if (rbBusCurrent.Checked)
                {
                    bus.isCurrent = true;
                    bus.isResistanse = false;
                    tbBusCurrentMax.Text = bus.IcsMax.ToString();
                    tbBusCurrentMin.Text = bus.IcsMin.ToString();
                }
                else if (rbBusResistance.Checked)
                {
                    bus.isResistanse = true;
                    bus.isCurrent = false;
                    tbBusActiveResistMax.Text = bus.ActiveResistMax.ToString();
                    tbBusReactiveResistMax.Text = bus.ReactiveResistMax.ToString();
                    tbBusActiveResistMin.Text = bus.ActiveResistMin.ToString();
                    tbBusReactiveResistMin.Text = bus.ReactiveResistMin.ToString();
                }
                rbBusCurrent.CheckedChanged += RbBusCurrent_CheckedChanged;
                rbBusResistance.CheckedChanged += RbBusCurrent_CheckedChanged;
            }
            else if (selectedElement is Line line)
            {
                _slctElement = line;
                panelPropertyLine.Visible = true;
                panelPropertyBus.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;

                tbLineName.Text = line.Name;
                tbLineLength.Text = line.Length.ToString();
                var dto = JsonProvider.LoadData<DataLineDto>("..\\..\\DataBase\\StaticData\\DataLine.json");
                cbLineMarks.DataSource = dto[0].Marks;
            }
            else if (selectedElement is Recloser recloser)
            {
                _slctElement = recloser;
                panelPropertyRecloser.Visible = true;
                panelPropertyLine.Visible = false;
                panelPropertyBus.Visible = false;
                panelPropertyTransformator.Visible = false;

                var dto = JsonProvider.LoadData<DataRecloserDto>("..\\..\\DataBase\\StaticData\\DataRecloser.json");
                tbRecloserName.Text = recloser.Name;
                cbRecloserType.DataSource = dto[0].TypeRecloser;
                cbRecloserTypeTT.DataSource = dto[0].TypeTT;
                cbIsCalculate.Checked = recloser.isCalculated;
            }
            else if (selectedElement is Transormator transormator)
            {
                _slctElement = transormator;
                panelPropertyTransformator.Visible = true;
                panelPropertyRecloser.Visible = false;
                panelPropertyLine.Visible = false;
                panelPropertyBus.Visible = false;

                tbTransformatorName.Text = transormator.Name;
                var dto = JsonProvider.LoadData<DataTransformatorDto>("..\\..\\DataBase\\StaticData\\DataTransformator.json");
                cbTransformatorTypesKTP.DataSource = dto[0].TypeKTP;
                cbTransformatorTypesKTP.DisplayMember = "TypeKTP";
                cbTransformatorSchemes.DataSource = dto[0].Scheme;
                cbTransformatorSchemes.DisplayMember = "Scheme";
            }
            else
            {
                panelPropertyBus.Visible = false;
                panelPropertyLine.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;
            }
        }

        private void RbBusCurrent_CheckedChanged(object sender, EventArgs e)
        {
            var bus = _slctElement as Bus;

            if (bus == null)
                return;
            if (rbBusCurrent.Checked)
            {
                bus.isCurrent = true;
                bus.isResistanse = false;
                tbBusCurrentMax.Text = bus.IcsMax.ToString();
                tbBusCurrentMin.Text = bus.IcsMin.ToString();
            }
            else if (rbBusResistance.Checked)
            {
                bus.isResistanse = true;
                bus.isCurrent = false;
                tbBusActiveResistMax.Text = bus.ActiveResistMax.ToString();
                tbBusReactiveResistMax.Text = bus.ReactiveResistMax.ToString();
                tbBusActiveResistMin.Text = bus.ActiveResistMin.ToString();
                tbBusReactiveResistMin.Text = bus.ReactiveResistMin.ToString();
            }
        }

        private void cbRecloserType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void rbBusCurrent_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                panelForCurrent.Visible = true;
                panelForResistance.Visible = false;
            }
        }

        private void rbBusResistance_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                panelForResistance.Visible = true;
                panelForCurrent.Visible = false;
            }
        }

        private void btnSaveProp_Click(object sender, EventArgs e)
        {
            if (_slctElement == null)
                return;
            if (_slctElement is Bus bus)
            {
                bus.Name = tbBusName.Text;
                bus.Voltage = double.Parse(cbBusVoltage.Text);
                if (bus.isCurrent)
                {
                    bus.IcsMax = double.Parse(tbBusCurrentMax.Text);
                    bus.IcsMin = double.Parse(tbBusCurrentMin.Text);
                }
                else if (bus.isResistanse)
                {
                    bus.ActiveResistMax = double.Parse(tbBusActiveResistMax.Text);
                    bus.ReactiveResistMax = double.Parse(tbBusReactiveResistMax.Text);
                    bus.ActiveResistMin = double.Parse(tbBusActiveResistMin.Text);
                    bus.ReactiveResistMin = double.Parse(tbBusReactiveResistMin.Text);
                }
            }
            else if (_slctElement is Line line)
            {
                line.Name = tbLineName.Text;
                line.Length = double.Parse(tbLineLength.Text);
                var dto = JsonProvider.LoadData<LineDataTypeDto>("..\\..\\DataBase\\LineDataTypesDB.json"); //ToDo: нужно как то сдлеать универсальный поиск независимо от места
                for (int i = 0; i < dto.Count; i++)
                {
                    if (cbLineMarks.Text == dto[i].Mark)
                    {
                        line.ActiveResistance = dto[i].ActiveResistance * line.Length;
                        line.ReactiveResistance = dto[i].ReactiveResistance * line.Length;
                        break;
                    }
                }
            }
            else if (_slctElement is Recloser recloser)
            {
                var dtoRec = JsonProvider.LoadData<RecloserDataTypeDto>("..\\..\\DataBase\\RecloserDataType.json");
                var dtoTT = JsonProvider.LoadData<DataTypeTTDto>("..\\..\\DataBase\\DataTypeTT.json");
                recloser.Name = tbRecloserName.Text;
                recloser.TypeRecloser = cbRecloserType.Text;
                recloser.TypeTT = cbRecloserTypeTT.Text;
                recloser.isCalculated = cbIsCalculate.Checked;
                foreach(var recType in dtoRec)
                {
                    if(recloser.TypeRecloser == recType.TypeRecloser)
                    {
                        recloser.Kb = recType.Kb;
                        recloser.Kcz = recType.Kcz;
                        recloser.Kn = recType.Kn;
                        break;
                    }
                }
                foreach(var typeTT in dtoTT)
                {
                    if(recloser.TypeTT == typeTT.TypeTT)
                    {
                        recloser.Ntt = typeTT.Ntt;
                        break;
                    }
                }
            }
            else if (_slctElement is Transormator transormator)
            {
                transormator.Name = tbTransformatorName.Text;
                transormator.TypeKTP = cbTransformatorTypesKTP.Text;
                transormator.Scheme = cbTransformatorSchemes.Text;
                var dto = JsonProvider.LoadData<TransformatorContainerDto>("..\\..\\DataBase\\TransformatorContainer.json");

                for (int i = 0; i < dto.Count; i++)
                {
                    if (transormator.TypeKTP == dto[i].TypeKTP)
                    {
                        transormator.FullResistance = 10 * (dto[i].Uk * Math.Pow(0.4, 2) / dto[i].S);
                        transormator.ActiveResistance = dto[i].Pk * Math.Pow(0.4, 2) / Math.Pow(dto[i].S, 2);
                        transormator.ReactiveResistance = Math.Sqrt(Math.Pow(transormator.FullResistance, 2) - Math.Pow(transormator.ActiveResistance, 2));
                        break;
                    }
                }
            }
        }
    }
}
