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
using DocumentFormat.OpenXml.Office.Word;
using Newtonsoft.Json;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        private void SavePNG_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png";
                saveFileDialog.Title = "Save an Image File";
                saveFileDialog.ShowDialog();

                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    drawPanel1.SaveToPng(saveFileDialog.FileName);
                }
            }
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

        private async void btnCalculate_Click(object sender, EventArgs e)
        {
            var calculationCenter = new CenterCalculation(_elements);
            var transformators = new List<Transormator>(calculationCenter.CalculationElementList.Select(x => x.OfType<Transormator>().FirstOrDefault()));

            using (var preCalculateForm = new PreCalculationForm(transformators))
            {
                if (preCalculateForm.ShowDialog() == DialogResult.OK)
                {
                    calculationCenter.CalculationMTOandMTZ(preCalculateForm);
                    lbProgressProcess.Visible = true;
                    progressBar.Visible = true;
                    progressBar.Value = 0;
                    try
                    {
                        var progress = new Progress<int>(percent =>
                        {
                            progressBar.Value = percent;
                        });
                        var docx = new Docx();
                        await docx.CreateReportDocumentAsync(calculationCenter, this, progress);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        progressBar.Visible = false;
                        lbProgressProcess.Visible = false;
                    }
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
                var dataBusJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "StaticData", "DataBus.json");
                var dataBusTypeJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "StaticData", "DataRecloser.json");

                if (File.Exists(dataBusJsonPath) && File.Exists(dataBusTypeJsonPath))
                {
                    var dto = JsonProvider.LoadData<DataBusDto>(dataBusJsonPath);
                    var dto2 = JsonProvider.LoadData<DataBusDto>(dataBusTypeJsonPath);
                    cbBusVoltage.DataSource = dto[0].Voltage;
                    cbBusVoltage.Text = bus.Voltage.ToString();
                    cbBusTypeTT.DataSource = dto2[0].TypeTT;
                    cbBusTypeTT.Text = bus.TypeTT;
                    cbBusType.DataSource = dto2[0].TypeRecloser;
                    cbBusType.Text = bus.TypeTT;
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {dataBusJsonPath}");

                _slctElement = bus;
                panelPropertyBus.Visible = true;
                panelPropertyLine.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;
                tbBusName.Text = bus.Name;
                tbBusMTO.Text = bus.MTO.ToString();
                tbBusMTZ.Text = bus.MTZ.ToString();
                rbBusCurrent.Checked = bus.IsCurrent;
                rbBusResistance.Checked = bus.IsResistanse;
                if (rbBusCurrent.Checked)
                {
                    bus.IsCurrent = true;
                    bus.IsResistanse = false;
                    tbBusCurrentMax.Text = bus.IkzMax.ToString();
                    tbBusCurrentMin.Text = bus.IkzMin.ToString();
                }   
                else if (rbBusResistance.Checked)
                {
                    bus.IsResistanse = true;
                    bus.IsCurrent = false;
                    tbBusActiveResistMax.Text = bus.ActiveResistMax.ToString();
                    tbBusReactiveResistMax.Text = bus.ReactiveResistMax.ToString();
                    tbBusActiveResistMin.Text = bus.ActiveResistMin.ToString();
                    tbBusReactiveResistMin.Text = bus.ReactiveResistMin.ToString();
                }
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
                    cbLineMarks.Text = line.Mark;
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
                cbIsCalculate.Checked = recloser.IsCalculated;
                tbRecloserMTO.Text = recloser.MTO.ToString();
                tbRecloserMTZ.Text = recloser.MTZ.ToString();
                tbPsuch.Text = recloser.Psuch.ToString();

                if (File.Exists(dataRecloserJsonPath))
                {
                    var dto = JsonProvider.LoadData<DataRecloserDto>(dataRecloserJsonPath);
                    cbRecloserType.DataSource = dto[0].TypeRecloser;
                    cbRecloserType.Text = recloser.TypeRecloser;
                    cbRecloserTypeTT.DataSource = dto[0].TypeTT;
                    cbRecloserTypeTT.Text = recloser.TypeTT;
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
                    cbTransformatorTypesKTP.Text = transormator.TypeKTP;
                    cbTransformatorSchemes.DataSource = dto[0].Scheme;
                    cbTransformatorSchemes.Text = transormator.Scheme;
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

        private void cbRecloserType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void rbBusCurrent_CheckedChanged(object sender, EventArgs e)
        {
            var bus = _slctElement as Bus;

            if (bus == null)
                return;
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                panelForCurrent.Visible = true;
                panelForResistance.Visible = false;
                bus.IsCurrent = true;
                bus.IsResistanse = false;
                tbBusCurrentMax.Text = bus.IkzMax.ToString();
                tbBusCurrentMin.Text = bus.IkzMin.ToString();
            }
        }

        private void rbBusResistance_CheckedChanged(object sender, EventArgs e)
        {
            var bus = _slctElement as Bus;

            if (bus == null)
                return;
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                panelForResistance.Visible = true;
                panelForCurrent.Visible = false;
                bus.IsResistanse = true;
                bus.IsCurrent = false;
                tbBusActiveResistMax.Text = bus.ActiveResistMax.ToString();
                tbBusReactiveResistMax.Text = bus.ReactiveResistMax.ToString();
                tbBusActiveResistMin.Text = bus.ActiveResistMin.ToString();
                tbBusReactiveResistMin.Text = bus.ReactiveResistMin.ToString();
            }
        }

        private void btnSaveProp_Click(object sender, EventArgs e)
        {
            if (_slctElement == null)
                return;
            if (_slctElement is Bus bus)
            {
                var typeTTJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "TypeTT.json");
                var recloserTypesJsonPath = Path.Combine(_baseDirectory + "..//..//", "DataBase", "RecloserTypes.json");

                try
                {
                    bus.Name = tbBusName.Text;
                    bus.Voltage = double.Parse(cbBusVoltage.Text.Replace('.', ','));
                    bus.TypeTT = cbBusTypeTT.Text;
                    bus.Type = cbBusType.Text;
                    bus.MTO = double.Parse(tbBusMTO.Text.Replace('.', ','));
                    bus.MTZ = double.Parse(tbBusMTZ.Text.Replace('.', ','));
                    if (File.Exists(typeTTJsonPath) && File.Exists(recloserTypesJsonPath))
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
                        var dtoType = JsonProvider.LoadData<RecloserDataTypeDto>(recloserTypesJsonPath);
                        foreach (var type in dtoType)
                        {
                            if (bus.Type == type.TypeRecloser)
                            {
                                bus.Kb = type.Kb;
                                bus.Kcz = type.Kcz;
                                bus.Kn = type.Kn;
                                break;
                            }
                        }
                    }
                    else
                        MessageBox.Show($"Неверно указан путь к файлу {typeTTJsonPath}");

                    if (bus.IsCurrent)
                    {
                        bus.IkzMax = double.Parse(tbBusCurrentMax.Text.Replace('.', ','));
                        bus.IkzMin = double.Parse(tbBusCurrentMin.Text.Replace('.', ','));
                    }
                    else if (bus.IsResistanse)
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
                line.Mark = cbLineMarks.Text;
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
                            line.ActiveResistanceFromDto = dto[i].ActiveResistance;
                            line.ReactiveResistanceFromDto = dto[i].ReactiveResistance;
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
                try
                {
                    recloser.Psuch = double.Parse(tbPsuch.Text.Replace('.', ','));
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                recloser.IsCalculated = cbIsCalculate.Checked;
                if (recloser.IsCalculated)
                {
                    recloser.MTO = 0;
                    recloser.MTZ = 0;
                }
                else
                {
                    try
                    {
                        recloser.MTO = double.Parse(tbRecloserMTO.Text.Replace('.', ','));
                        recloser.MTZ = double.Parse(tbRecloserMTZ.Text.Replace('.', ','));
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

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
                    if (!string.IsNullOrEmpty(cbBusVoltage.Text))
                        transormator.Voltage = double.Parse(cbBusVoltage.Text);
                    for (int i = 0; i < dto.Count; i++)
                    {
                        if (transormator.TypeKTP == dto[i].TypeKTP)
                        {
                            transormator.Uk = dto[i].Uk;
                            transormator.Pk = dto[i].Pk;
                            transormator.S = dto[i].S;

                            var fullResistance = 10 * (dto[i].Uk * Math.Pow(transormator.Voltage, 2) / dto[i].S);
                            var activeResistance = dto[i].Pk * Math.Pow(transormator.Voltage, 2) / Math.Pow(dto[i].S, 2);
                            var reactiveResistance = Math.Sqrt(Math.Pow(fullResistance, 2) - Math.Pow(activeResistance, 2));
                            transormator.FullResistance = Math.Round(fullResistance, 3);
                            transormator.ActiveResistance = Math.Round(activeResistance, 3);
                            transormator.ReactiveResistance = Math.Round(reactiveResistance, 3);
                            break;
                        }
                    }
                }
                else
                    MessageBox.Show($"Неверно указан путь к файлу {transformatorTypesJsonPath}");
            }
        }

        private void cbIsCalculate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsCalculate.Checked)
                panelRecloserMTOMTZ.Visible = false;
            else
                panelRecloserMTOMTZ.Visible = true;
        }
    }
}
