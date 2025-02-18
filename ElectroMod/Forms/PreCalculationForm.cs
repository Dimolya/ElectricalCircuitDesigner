using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroMod.Forms
{
    public partial class PreCalculationForm : Form
    {
        public string Reconnect { get; set; }
        public string NumberTY { get; set; }
        public double PowerSuchKBT { get; set; }
        public double PowerKBT { get; set; }

        public double PowerSuchKBA { get; set; }
        public double PowerKBA { get; set; }

        public PreCalculationForm(List<Transormator> transformators)
        {
            InitializeComponent();
            tbPowerKBA.Text = transformators.Sum(x => x.S).ToString();
        }

        private void btnFormCalculate_Click(object sender, EventArgs e)
        {
            double powerKBT;
            double powerKBA;
            double powerSuchKBT;
            double powerSuchKBA;
            NumberTY = tbNumberTY.Text;
            Reconnect = cbReconnect.Text;

            if (double.TryParse(tbPowerSuchKBT.Text, out powerSuchKBT))
                PowerSuchKBT = powerSuchKBT;
            if (double.TryParse(tbPowerSuchKBA.Text, out powerSuchKBA))
                PowerSuchKBA = powerSuchKBA;
            if (double.TryParse(tbPowerKBT.Text, out powerKBT))
                PowerKBT = powerKBT;
            if (double.TryParse(tbPowerKBA.Text, out powerKBA))
                PowerKBA = powerKBA;
            DialogResult = DialogResult.OK;
        }

        private void cbReconnect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if(cb.Text == "Расчет по мощности ТУ")
            {
                panelHasTY.Visible = true;
                panelNotTY.Visible = false;
            }
            else
            {
                panelHasTY.Visible = false;
                panelNotTY.Visible = true;
            }
        }

    }
}
