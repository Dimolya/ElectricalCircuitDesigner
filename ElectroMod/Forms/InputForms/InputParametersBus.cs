using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroMod.Forms.InputForms
{
    public partial class InputParametersBus : Form
    {
        private string _elementName;
        private string _voltage;
        private string _dataType;
        private List<string> _dataTypes = new List<string>() { "Ток", "Сопротивление" };
        public InputParametersBus()
        {
            InitializeComponent();
            cbDataTypes.DataSource = _dataTypes;
        }

        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public string Voltage
        {
            get { return _voltage; }
            set { _voltage = value; }
        }
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ElementName = tbElementName.Text;
            Voltage = tbVoltage.Text;
            DataType = cbDataTypes.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
