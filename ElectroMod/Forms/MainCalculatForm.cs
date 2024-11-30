using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Threading;
using ElectroMod.Forms;
using static System.Windows.Forms.AxHost;
using ElectroMod.Forms.InputForms;
using System.Collections.Generic;
using Newtonsoft.Json;
using ElectroMod.DataBase.Dtos;
using ElectroMod.DataBase;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ElectroMod
{
    [Serializable]
    public partial class MainCalculatForm : Form
    {
        private float defaultScale = 1.0f;
        private float currentScale = 1.0f;

        Elements elements = new Elements();
        Element element = new Element();
        private ISelectable slctElement;

        public MainCalculatForm()
        {
            InitializeComponent();
            drawPanel1.Build(elements);
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
            try
            {
                elements.Add(new Bus(elements));
                btBus.Enabled = false;
                drawPanel1.Invalidate();
            }
            catch { Exception(); }
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            try
            {
                elements.Add(new Line(elements));
                drawPanel1.Invalidate();
            }
            catch { Exception(); }
        }

        private void btRecloser_Click(object sender, EventArgs e)
        {
            try
            {
                elements.Add(new Recloser(elements));
                drawPanel1.Invalidate();
            }
            catch { Exception(); }
        }

        private void btTransformator_Click(object sender, EventArgs e)
        {
            try
            {
                elements.Add(new Transormator(elements));
                drawPanel1.Invalidate();
            }
            catch
            {
                Exception();
            }
        }


        public void Exception()
        {
            DialogResult result = MessageBox.Show("Вы не добавили источник питания!", "Предупреждение", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {
                return;
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Schema|*.schema" };
            if (ofd.ShowDialog(this) == DialogResult.OK)
                using (FileStream fs = File.OpenRead(ofd.FileName))
                {
                    elements = (Elements)new BinaryFormatter().Deserialize(fs);
                    drawPanel1.Build(elements);
                }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Schema|*.schema" };
            if (sfd.ShowDialog(this) == DialogResult.OK)
                using (FileStream fs = File.Create(sfd.FileName))
                    new BinaryFormatter().Serialize(fs, elements);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
           "Очистить поле?",
           "Сообщение",
           MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                elements.Clear();
                drawPanel1.Invalidate();
            }
            else return;
            btBus.Enabled = true;
            lbA.Text = Convert.ToString(0);
            lbR.Text = Convert.ToString(0);
            lbV.Text = Convert.ToString(0);
        }

        private void bt_DeleteOne_Click(object sender, EventArgs e)
        {
            drawPanel1.RemoveSelected();
            drawPanel1.Invalidate();
        }

        private void lbA_TextChanged(object sender, EventArgs e)
        {
            if (lbA.Text.Length > 6)
                lbA.Text = lbA.Text.Substring(0, 6);
        }

        private void lbV_TextChanged(object sender, EventArgs e)
        {
            if (lbV.Text.Length > 6)
                lbV.Text = lbV.Text.Substring(0, 6);
        }

        private void lbR_TextChanged(object sender, EventArgs e)
        {
            if (lbR.Text.Length > 6)
                lbR.Text = lbR.Text.Substring(0, 6);
        }
        private void UpdateZoomInfo(float scale)
        {
            btZoom.Text = $"{(scale * 100):0}%";
        }

        private void btZoom_Click(object sender, EventArgs e)
        {
            drawPanel1.SetDefaultScale(defaultScale);
        }

        private void UpdateElementDetails(ISelectable selectedElement)
        {
            if (selectedElement is Bus bus)
            {
                slctElement = bus;
                panelPropertyBus.Visible = true;
                panelPropertyLine.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;

                tbBusName.Text = bus.Name;
                tbBusVoltage.Text = bus.Voltage.ToString();
                tbBusCurrentMax.Text = bus.CurrentMax.ToString();
                tbBusCurrentMin.Text = bus.CurrentMin.ToString();
                tbBusActiveResistMax.Text = bus.ActiveResistMax.ToString();
                tbBusReactiveResistMax.Text = bus.ReactiveResistMax.ToString();
                tbBusActiveResistMin.Text = bus.ActiveResistMin.ToString();
                tbBusReactiveResistMin.Text = bus.ReactiveResistMin.ToString();
            }
            else if (selectedElement is Line line)
            {
                slctElement = line;
                panelPropertyLine.Visible = true;
                panelPropertyBus.Visible = false;
                panelPropertyRecloser.Visible = false;
                panelPropertyTransformator.Visible = false;

                tbLineName.Text = line.Name;
                tbLineLength.Text = line.Length.ToString();
                var dto = JsonProvider.LoadData<LineDataTypeDto>("..\\..\\DataBase\\LineDataTypesDB.json");
                cbLineMarks.DataSource = dto[0].Mark;
                cbLineMarks.DisplayMember = "Name";
                cbLineMarks.ValueMember = "Id";
            }
            else if (selectedElement is Recloser recloser)
            {
                slctElement = recloser;
                panelPropertyRecloser.Visible = true;
                panelPropertyLine.Visible = false;
                panelPropertyBus.Visible = false;
                panelPropertyTransformator.Visible = false;

                tbRecloserName.Text = recloser.Name;
                var dto = JsonProvider.LoadData<RecloserDataTypeDto>("..\\..\\DataBase\\RecloserDataTypeDB.json");
                cbRecloserType.DataSource = dto[0].TypeRecloser;
                cbRecloserType.DisplayMember = "Name";
                cbRecloserType.ValueMember = "Id";
                cbRecloserTypeTT.DataSource = (cbRecloserType.SelectedItem as InternalPropRecloser).TypeTT;
                cbRecloserType.SelectedIndexChanged += cbRecloserType_SelectedIndexChanged;
            }
            else if (selectedElement is Transormator transormator)
            {
                slctElement = transormator;
                panelPropertyTransformator.Visible = true;
                panelPropertyRecloser.Visible = false;
                panelPropertyLine.Visible = false;
                panelPropertyBus.Visible = false;

                tbTransformatorName.Text = transormator.Name;
                var dto = JsonProvider.LoadData<TransformatorDataTypeDto>("..\\..\\DataBase\\TransformatorDataTypeDB.json");
                cbTransformatorTypesKTP.DataSource = dto[0].TypesKTP;
                cbTransformatorTypesKTP.DisplayMember = "Type";
                cbTransformatorSchemes.DataSource = dto[0].SchemesConnectingWinding;
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

        private void cbRecloserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedRecloser = cbRecloserType.SelectedItem as InternalPropRecloser;
            if (selectedRecloser != null)
            {
                cbRecloserTypeTT.DataSource = selectedRecloser.TypeTT;
            }
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
            if (slctElement == null)
                return;

            if (slctElement is Bus bus)
            {
                bus.Name = tbBusName.Text;
                bus.Voltage = double.Parse(tbBusVoltage.Text);
                bus.CurrentMin = double.Parse(tbBusCurrentMin.Text);
                bus.CurrentMax = double.Parse(tbBusCurrentMax.Text);
                bus.ActiveResistMax = double.Parse(tbBusActiveResistMax.Text);
                bus.ReactiveResistMax = double.Parse(tbBusReactiveResistMax.Text);
                bus.ActiveResistMin = double.Parse(tbBusActiveResistMin.Text);
                bus.ReactiveResistMin = double.Parse(tbBusReactiveResistMin.Text);
            }
            else if (slctElement is Line line)
            {
                line.Name = tbLineName.Text;
                line.Length = double.Parse(tbLineLength.Text);
                line.Mark = cbLineMarks.SelectedText;
                line.ActiveResistance = (cbLineMarks.SelectedItem as LineDataTypeDto).ActiveResistance;
                line.ReactiveResistance = (cbLineMarks.SelectedItem as LineDataTypeDto).ReactiveResistance;
            }
            else if(slctElement is Recloser recloser)
            {
                recloser.Name = tbRecloserName.Text;
                recloser.TypeRecloser = cbRecloserType.Text;

                var input = cbRecloserTypeTT.Text;
                var parts = input.Split('/');
                var numerator = double.Parse(parts[0].Trim()); 
                var denominator = double.Parse(parts[1].Trim());
                var result = numerator / denominator;
                recloser.TypeTT = result;

            }
            else if(slctElement is Transormator transormator)
            {
                transormator.Name = tbTransformatorName.Text;
                transormator.TypeKTP = cbTransformatorTypesKTP.SelectedText;
                transormator.ShemeConnectingWinding = cbTransformatorSchemes.SelectedText;
                transormator.ResistanceOne = (cbTransformatorTypesKTP.SelectedItem as TypeProp).ResistanceOne;
                transormator.ResistanceTwo = (cbTransformatorTypesKTP.SelectedItem as TypeProp).ResistanceTwo;
            }
        }
    }
}
