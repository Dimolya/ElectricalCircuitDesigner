using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Threading;
using static ElectroMod.PowerSupplyCharacterization;
using static ElectroMod.ResistanceChar;
using ElectroMod.Forms;
using static System.Windows.Forms.AxHost;
using ElectroMod.Forms.InputForms;

namespace ElectroMod
{
    [Serializable]
    public partial class MainCalculatForm : Form
    {
        private float defaultScale = 1.0f;
        private float currentScale = 1.0f;

        Elements elements = new Elements();
        Element element = new Element();

        public MainCalculatForm()
        {
            InitializeComponent();
            drawPanel1.Build(elements);
            element.DataChanged += DataChange;
            drawPanel1.ScaleChanged += DrawPanel1_ScaleChanged;
            UpdateZoomInfo(1.0f);
        }

        private void DrawPanel1_ScaleChanged(object sender, float scale)
        {
            UpdateZoomInfo(scale);
        }

        public void DataChange(double cStr, double volt, double resist)
        {
            lbA.Text = Convert.ToString(cStr);
            lbV.Text = Convert.ToString(volt);
            lbR.Text = Convert.ToString(resist);
        }

        private void btBus_Click(object sender, EventArgs e)
        {
            try
            {
                using (var inputParametersBus = new InputParametersBus())
                {
                    if (inputParametersBus.ShowDialog() == DialogResult.OK)
                    {
                        elements.Add(new Bus(elements,
                            inputParametersBus.ElementName,
                            inputParametersBus.Voltage,
                            inputParametersBus.DataType));
                        btBus.Enabled = false;
                        drawPanel1.Invalidate();
                    }
                }

            }
            catch { Exception(); }
        }

        private void btTransformator_Click(object sender, EventArgs e)
        {
            try
            {
                using (var inputParametersTransformator = new InputParametersTransformator())
                {
                    if (inputParametersTransformator.ShowDialog() == DialogResult.OK)
                    {
                        elements.Add(new Transormator(elements,
                            inputParametersTransformator.ElementName,
                            inputParametersTransformator.TypeKTP,
                            inputParametersTransformator.ShemeConnectingWinding));

                        drawPanel1.Invalidate();
                    }
                }
            }
            catch
            {
                Exception();
            }
        }
        private void btRecloser_Click(object sender, EventArgs e)
        {
            try
            {
                using (var inputParametersRecloser = new InputParametersRecloser())
                {
                    if (inputParametersRecloser.ShowDialog() == DialogResult.OK)
                    {
                        elements.Add(new Recloser(elements,
                                                  inputParametersRecloser.ElementName,
                                                  inputParametersRecloser.TypeRecloser,
                                                  inputParametersRecloser.TypeTT));
                        drawPanel1.Invalidate();
                    }
                }
            }
            catch { Exception(); }
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            try
            {
                using (var inputParametersLine = new InputParametersLine())
                {
                    if (inputParametersLine.ShowDialog() == DialogResult.OK)
                    {
                        elements.Add(new Line(elements,
                                              inputParametersLine.ElementName,
                                              inputParametersLine.ElementLength,
                                              inputParametersLine.Mark));
                        drawPanel1.Invalidate();
                    }
                }
            }
            catch { Exception(); }
        }

        private void Add_CharacterizationPSupply(object sender, ElementsEvenAtgs e)
        {
            Bus pw = new Bus();
            {
                pw.CurrentStrenghtPS = e.CurrentStrenghtPS;
                pw.VoltagePS = e.VoltagePS;
                pw.ResistancePS = e.ResistancePS;
            };
            lbA.Text = Convert.ToString(pw.CurrentStrenghtPS);
            lbV.Text = Convert.ToString(pw.VoltagePS);
            lbR.Text = Convert.ToString(pw.ResistancePS);
        }
        private void Add_CharacterizationAll(object sender, ElementsEvenAtgsAll e)
        {
            Element r = new Element();
            {
                r.Resistance = e.Resistance;
            };
            if (Convert.ToDouble(lbR.Text) + r.Resistance != 0)
            {
                element.OnDataChanged(Convert.ToDouble(lbA.Text), Convert.ToDouble(lbV.Text), Convert.ToDouble(lbR.Text) + r.Resistance);
            }
            else
            {
                element.OnDataChanged(Convert.ToDouble(lbA.Text), Convert.ToDouble(lbV.Text), Convert.ToDouble(lbR.Text));
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
            DataChange(element.CurrentStrength, element.Voltage, element.Resistance);
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
    }
}
