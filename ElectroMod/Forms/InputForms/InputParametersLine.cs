using ElectroMod.DataBase;
using ElectroMod.DataBase.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroMod.Forms.InputForms
{
    public partial class InputParametersLine : Form
    {
        private List<LineDataTypeDto> _dto;
        private string _elementName;
        private string _elementLength;
        private LineDataTypeDto _mark;

        public InputParametersLine()
        {
            InitializeComponent();
            LoadDataToComboBox();
        }

        private void LoadDataToComboBox()
        {
            _dto = JsonProvider.LoadData<LineDataTypeDto>("..\\..\\DataBase\\LineDataTypesDB.json");
            cbMarks.DataSource = _dto;
            cbMarks.DisplayMember = "Mark";
            cbMarks.ValueMember = "Id";
        }

        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public string ElementLength
        {
            get { return _elementLength; }
            set { _elementLength = value; }
        }
        public LineDataTypeDto Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ElementName = tbElementName.Text;
            ElementLength = tbElementName.Text;
            Mark = cbMarks.SelectedItem as LineDataTypeDto;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
