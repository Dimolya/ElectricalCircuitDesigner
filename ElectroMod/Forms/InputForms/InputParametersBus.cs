using ElectroMod.DataBase;
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
        //private List<BusDataTypeDto> _dto;

        private string _elementName;
        private string _voltage;
        private string _dataType;
        public InputParametersBus()
        {
            InitializeComponent();
            LoadDataToComboBox();
        }

        private void LoadDataToComboBox()
        {
            //_dto = JsonProvider.LoadData<BusDataTypeDto>("..\\..\\DataBase\\BusDataTypesDB.json");
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
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void rbCurrent_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                panelForCurrent.Visible = true;
                panelForResistance.Visible = false;
            }
        }

        private void rbResistance_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                panelForResistance.Visible = true;
                panelForCurrent.Visible = false;
            }
        }
    }
}
