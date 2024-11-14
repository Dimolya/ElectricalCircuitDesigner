using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroMod
{
    [Serializable]
    public partial class PowerSupplyCharacterization : Form
    {
        public delegate void PowerSupplyCharacterizationChangedHandler(object sender, ElementsEvenAtgs e);
        public event PowerSupplyCharacterizationChangedHandler PowerSupplyCharacterizationChanged;

        public PowerSupplyCharacterization()
        {
            InitializeComponent();
        }
        public class ElementsEvenAtgs
        {
            public double CurrentStrenghtPS { get; set; }
            public double VoltagePS { get; set; }
            public double ResistancePS { get; set; }
            public ElementsEvenAtgs(double currentStrenght, double voltage, double resistance)
            {
                CurrentStrenghtPS = currentStrenght;
                VoltagePS = voltage;
                ResistancePS = resistance;
            }
        }
        private void bPSSave_Click(object sender, EventArgs e)
        {
            if (PowerSupplyCharacterizationChanged != null) PowerSupplyCharacterizationChanged(this, new ElementsEvenAtgs(Convert.ToDouble(tbAPS.Text), Convert.ToDouble(tbVPS.Text), Convert.ToDouble(tbRPS.Text)));
            
            this.Close();
        }
    }
}
