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
        public string NumberTY { get; set; }
        public double PowerKBT { get; set; }
        public double PowerKBA { get; set; }
        public double CapacityKBT { get; set; }
        public double CapacityKBA { get; set; }

        public PreCalculationForm()
        {
            InitializeComponent();
        }

        private void btnFormCalculate_Click(object sender, EventArgs e)
        {
            double powerKBT;
            double powerKBA;
            double capacityKBT;
            double capacityKBA;

            NumberTY = tbNumberTY.Text;
            if (double.TryParse(tbCapacityKBT.Text, out capacityKBT))
                CapacityKBT = capacityKBT;
            if (double.TryParse(tbCapacityKBA.Text, out capacityKBA))
                CapacityKBA = capacityKBA;
            if (double.TryParse(tbPowerKBT.Text, out powerKBT))
                PowerKBT = powerKBT;
            if (double.TryParse(tbPowerKBA.Text, out powerKBA))
                PowerKBA = powerKBA;
            DialogResult = DialogResult.OK;

        }

        private void cbReconnect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if(cb.Text == "Расчет по мощности")
            {
                panelOptionOneReconnect.Visible = true;
                panelOptionTwoReconnect.Visible = false;
            }
            else
            {
                panelOptionOneReconnect.Visible = false;
                panelOptionTwoReconnect.Visible = true;
            }
        }

        private void cbEarlyConnect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb.Text == "Расчет по мощности кВт")
            {
                panelOptionOneEarlyConnect.Visible = true;
                panelOptionTwoEarlyConnect.Visible = false;
            }
            else
            {
                panelOptionOneEarlyConnect.Visible = false;
                panelOptionTwoEarlyConnect.Visible = true;
            }
        }
    }
}
