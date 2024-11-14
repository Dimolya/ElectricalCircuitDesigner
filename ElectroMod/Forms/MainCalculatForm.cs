using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Threading;
using static ElectroMod.PowerSupplyCharacterization;
using static ElectroMod.ResistanceChar;

namespace ElectroMod
{
    [Serializable]
    public partial class MainCalculatForm : Form
    {
        Elements elements = new Elements();
        Element element = new Element();
        private int RCounter;

        public MainCalculatForm()
        {
            InitializeComponent();
            drawPanel1.Build(elements);
            element.DataChanged += DataChange;
        }
        public void DataChange(double cStr, double volt, double resist)
        {

            lbA.Text = Convert.ToString(cStr);
            lbV.Text = Convert.ToString(volt);
            lbR.Text = Convert.ToString(resist);
        }

        /// <summary>
        /// Источник питания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPowerSupply_Click(object sender, EventArgs e)
        {
            PowerSupplyCharacterization pwChar = new PowerSupplyCharacterization();
            pwChar.Show();
            pwChar.PowerSupplyCharacterizationChanged += Add_CharacterizationPSupply;

            btPowerSupply.Enabled = false;
            elements.Add(new PowerSupply(elements) { Location = new Point(250, 200) });
            drawPanel1.Invalidate();
        }
        private  void Add_CharacterizationPSupply(object sender, ElementsEvenAtgs e)
        {
            PowerSupply pw = new PowerSupply();
            {
                pw.CurrentStrenghtPS = e.CurrentStrenghtPS;
                pw.VoltagePS = e.VoltagePS;
                pw.ResistancePS = e.ResistancePS;
            };
            lbA.Text = Convert.ToString(pw.CurrentStrenghtPS);
            lbV.Text = Convert.ToString(pw.VoltagePS);
            lbR.Text = Convert.ToString(pw.ResistancePS);
        }
        /// <summary>
        /// Резистор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTransformator_Click(object sender, EventArgs e)
        {
            try
            {
                ResistanceChar resistanceChar = new ResistanceChar();
                resistanceChar.Show();
                resistanceChar.ResistanceCharacterizationChanged += Add_CharacterizationAll;


                elements.Add(new Transormator(elements) { Location = new Point(250, 200)});
                drawPanel1.Invalidate();
            }
            catch
            {
                Exception();
            }
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
                element.OnDataChanged(Convert.ToDouble(lbA.Text), Convert.ToDouble(lbV.Text), Convert.ToDouble(lbR.Text) );
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
        private void btRecloser_Click(object sender, EventArgs e)
        {
            try
            {
                ResistanceChar resistanceChar = new ResistanceChar();
                resistanceChar.Show();
                resistanceChar.ResistanceCharacterizationChanged += Add_CharacterizationAll;
                elements.Add(new Recloser(elements) { Location = new Point(250, 200), Tag="kfhdsjhbfjdhsbjhsdbvjhdsbhjvbdhjvbhjdsbvhjsdv"});
                drawPanel1.Invalidate();
            }
            catch { Exception(); }
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            try
            {
                ResistanceChar resistanceChar = new ResistanceChar();
                resistanceChar.Show();
                resistanceChar.ResistanceCharacterizationChanged += Add_CharacterizationAll;

                elements.Add(new Line(elements) { Location = new Point(250, 200), Tag = "оdfvdfgdfgfdgdfgdgля жож"});
                drawPanel1.Invalidate();
            }
            catch { Exception(); }
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
                RCounter = 0;
                elements.Clear();
                drawPanel1.Invalidate();
            }
            else return;
            btPowerSupply.Enabled = true;
            lbA.Text = Convert.ToString(0);
            lbR.Text=Convert.ToString(0);
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
    }
}
