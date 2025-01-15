using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ElectroMod.Forms;
using ElectroMod.DataBase.Dtos;
using ElectroMod.DataBase;
using ElectroMod.DataBase.Dtos.StaticDtos;
using ElectroMod.Dtos.StaticDtos;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Globalization;

namespace ElectroMod
{
    [Serializable]
    public partial class MainCalculatForm : Form
    {
        private float _defaultScale = 1.0f;
        private float _currentScale = 1.0f;
        private string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private Elements _elements = new Elements();
        private ISelectable _slctElement;

        public MainCalculatForm()
        {
            InitializeComponent();
            drawPanel1.Build(_elements, this);
            drawPanel1.ScaleChanged += DrawPanel1_ScaleChanged;
            drawPanel1.ElementSelected += UpdateElementDetails;
            UpdateZoomInfo(1.0f);
        }

        public List<ConnectingWare> SelectedCalculationPoints { get; private set; } = new List<ConnectingWare>();

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
                    drawPanel1.Build(_elements, this);
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
            if (_slctElement is Bus)
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
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => docx.CreateReportDocument(calcul, this)));
                    }
                    else
                    {
                        docx.CreateReportDocument(calcul, this);
                    }
                }
            }
        }

        public void AddSelectedPoint(ConnectingWare ware)
        {
            SelectedCalculationPoints.Add(ware);
        }

        public void RemoveSelectedPoint(ConnectingWare ware)
        {
            SelectedCalculationPoints.Remove(ware);
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
                var dataBusJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "StaticData", "DataBus.json");

                if (File.Exists(dataBusJsonPath))
                {
                    var dto = JsonProvider.LoadData<DataBusDto>(dataBusJsonPath);
                    cbBusVoltage.DataSource = dto[0].Voltage;
                    cbBusTypeTT.DataSource = dto[0].TypeTT;
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {dataBusJsonPath}");

                _slctElement = bus;
                panelPropertyBus.Visible = true;
                panelPropertyLine.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;
                tbBusName.Text = bus.Name;
                if (rbBusCurrent.Checked)
                {
                    bus.isCurrent = true;
                    bus.isResistanse = false;
                    tbBusCurrentMax.Text = bus.IszMax.ToString();
                    tbBusCurrentMin.Text = bus.IszMin.ToString();
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
                var dataLineJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "StaticData", "DataLine.json");

                _slctElement = line;
                panelPropertyLine.Visible = true;
                panelPropertyBus.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;

                tbLineName.Text = line.Name;
                tbLineLength.Text = line.Length.ToString();

                if (File.Exists(dataLineJsonPath))
                {
                    var dto = JsonProvider.LoadData<DataLineDto>(dataLineJsonPath);
                    cbLineMarks.DataSource = dto[0].Marks;
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {dataLineJsonPath}");
            }
            else if (selectedElement is Recloser recloser)
            {
                var dataRecloserJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "StaticData", "DataRecloser.json");

                _slctElement = recloser;
                panelPropertyRecloser.Visible = true;
                panelPropertyLine.Visible = false;
                panelPropertyBus.Visible = false;
                panelPropertyTransformator.Visible = false;

                tbRecloserName.Text = recloser.Name;
                cbIsCalculate.Checked = recloser.isCalculated;

                if (File.Exists(dataRecloserJsonPath))
                {
                    var dto = JsonProvider.LoadData<DataRecloserDto>(dataRecloserJsonPath);
                    cbRecloserType.DataSource = dto[0].TypeRecloser;
                    cbRecloserTypeTT.DataSource = dto[0].TypeTT;
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {dataRecloserJsonPath}");

            }
            else if (selectedElement is Transormator transormator)
            {
                var dataTransformatorJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "StaticData", "DataTransformator.json");

                _slctElement = transormator;
                panelPropertyTransformator.Visible = true;
                panelPropertyRecloser.Visible = false;
                panelPropertyLine.Visible = false;
                panelPropertyBus.Visible = false;

                tbTransformatorName.Text = transormator.Name;

                if (File.Exists(dataTransformatorJsonPath))
                {
                    var dto = JsonProvider.LoadData<DataTransformatorDto>(dataTransformatorJsonPath);
                    cbTransformatorTypesKTP.DataSource = dto[0].TypeKTP;
                    cbTransformatorTypesKTP.DisplayMember = "TypeKTP";
                    cbTransformatorSchemes.DataSource = dto[0].Scheme;
                    cbTransformatorSchemes.DisplayMember = "Scheme";
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {dataTransformatorJsonPath}");
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
                tbBusCurrentMax.Text = bus.IszMax.ToString();
                tbBusCurrentMin.Text = bus.IszMin.ToString();
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
                var typeTTJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "TypeTT.json");

                try
                {
                    bus.Name = tbBusName.Text;
                    bus.Voltage = double.Parse(cbBusVoltage.Text.Replace('.', ','));
                    bus.TypeTT = cbBusTypeTT.Text;

                    if (File.Exists(typeTTJsonPath))
                    {
                        var dtoTT = JsonProvider.LoadData<DataTypeTTDto>(typeTTJsonPath);
                        foreach (var typeTT in dtoTT)
                        {
                            if (bus.TypeTT == typeTT.TypeTT)
                            {
                                bus.Ntt = typeTT.Ntt;
                                break;
                            }
                        }
                    }
                    else
                        MessageBox.Show($"Неверно указан путь к файлу {typeTTJsonPath}");

                    if (bus.isCurrent)
                    {
                        bus.IszMax = double.Parse(tbBusCurrentMax.Text.Replace('.', ','));
                        bus.IszMin = double.Parse(tbBusCurrentMin.Text.Replace('.', ','));
                    }
                    else if (bus.isResistanse)
                    {
                        bus.ActiveResistMax = double.Parse(tbBusActiveResistMax.Text.Replace('.', ','));
                        bus.ReactiveResistMax = double.Parse(tbBusReactiveResistMax.Text.Replace('.', ','));
                        bus.ActiveResistMin = double.Parse(tbBusActiveResistMin.Text.Replace('.', ','));
                        bus.ReactiveResistMin = double.Parse(tbBusReactiveResistMin.Text.Replace('.', ','));
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (_slctElement is Line line)
            {
                line.Name = tbLineName.Text;
                try
                {
                    line.Length = double.Parse(tbLineLength.Text.Replace('.', ','));
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message); 
                }

                var lineTypesJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "LineTypes.json");
                if (File.Exists(lineTypesJsonPath))
                {
                    var dto = JsonProvider.LoadData<LineDataTypeDto>(lineTypesJsonPath);
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
                else
                    MessageBox.Show($"Неверно указан путь к файлу {lineTypesJsonPath}");

            }
            else if (_slctElement is Recloser recloser)
            {
                var recloserTypesJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "RecloserTypes.json");
                var typeTTJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "TypeTT.json");

                recloser.Name = tbRecloserName.Text;
                recloser.TypeRecloser = cbRecloserType.Text;
                recloser.TypeTT = cbRecloserTypeTT.Text;
                recloser.isCalculated = cbIsCalculate.Checked;

                if (File.Exists(recloserTypesJsonPath))
                {
                    var dtoRec = JsonProvider.LoadData<RecloserDataTypeDto>(recloserTypesJsonPath);
                    foreach (var recType in dtoRec)
                    {
                        if (recloser.TypeRecloser == recType.TypeRecloser)
                        {
                            recloser.Kb = recType.Kb;
                            recloser.Kcz = recType.Kcz;
                            recloser.Kn = recType.Kn;
                            break;
                        }
                    }
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {recloserTypesJsonPath}");

                if (File.Exists(typeTTJsonPath))
                {
                    var dtoTT = JsonProvider.LoadData<DataTypeTTDto>(typeTTJsonPath);
                    foreach (var typeTT in dtoTT)
                    {
                        if (recloser.TypeTT == typeTT.TypeTT)
                        {
                            recloser.Ntt = typeTT.Ntt;
                            break;
                        }
                    }
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {recloserTypesJsonPath}");
            }
            else if (_slctElement is Transormator transormator)
            {
                var transformatorTypesJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "TransformatorTypes.json");
                transormator.Name = tbTransformatorName.Text;
                transormator.TypeKTP = cbTransformatorTypesKTP.Text;
                transormator.Scheme = cbTransformatorSchemes.Text;

                if (File.Exists(transformatorTypesJsonPath))
                {
                    var dto = JsonProvider.LoadData<TransformatorContainerDto>(transformatorTypesJsonPath);
                    var voltage = double.Parse(cbBusVoltage.Text);
                    for (int i = 0; i < dto.Count; i++)
                    {
                        if (transormator.TypeKTP == dto[i].TypeKTP)
                        {
                            transormator.FullResistance = 10 * (dto[i].Uk * Math.Pow(voltage, 2) / dto[i].S);
                            transormator.ActiveResistance = dto[i].Pk * Math.Pow(voltage, 2) / Math.Pow(dto[i].S, 2);
                            transormator.ReactiveResistance = Math.Sqrt(Math.Pow(transormator.FullResistance, 2) - Math.Pow(transormator.ActiveResistance, 2));
                            break;
                        }
                    }
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {transformatorTypesJsonPath}");
            }
        }
    }
}
