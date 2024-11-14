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
    public partial class ResistanceChar : Form
    {
        public delegate void ResistanceCharacterizationChangedHandler(object sender, ElementsEvenAtgsAll e);
        public event ResistanceCharacterizationChangedHandler ResistanceCharacterizationChanged;
        public ResistanceChar()
        {
            InitializeComponent();
        }
        public class ElementsEvenAtgsAll
        {
            public double Resistance { get; set; }
            public ElementsEvenAtgsAll(double resistance)
            {
                Resistance = resistance;
            }
        }
        private void bAllSave_Click(object sender, EventArgs e)
        {
            if (ResistanceCharacterizationChanged != null) ResistanceCharacterizationChanged(this, new ElementsEvenAtgsAll(Convert.ToDouble(tbRAll.Text)));
            this.Close();
        }
    }
}
