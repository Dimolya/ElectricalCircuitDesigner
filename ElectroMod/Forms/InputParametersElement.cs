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
    public partial class InputParametersElement : Form
    {
        public string ElementName { get; set; }
        public InputParametersElement()
        {
            InitializeComponent();            
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ElementName = tbElementName.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
