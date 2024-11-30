using ElectroMod.DataBase;
using ElectroMod.DataBase.Dtos;
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
    public partial class InputParametersRecloser : Form
    {
        private List<RecloserDataTypeDto> _dto;
        private string _elementName;
        private string _typeRecloser;
        private string _typeTT;

        public InputParametersRecloser()
        {
            InitializeComponent();
            LoadDataToComboBox();
        }

        private void LoadDataToComboBox()
        {
            _dto = JsonProvider.LoadData<RecloserDataTypeDto>("..\\..\\DataBase\\RecloserDataTypeDB.json");
            cbTypeRecloser.DataSource = _dto[0].TypeRecloser;
            cbTypeRecloser.DisplayMember = "Name";
            cbTypeRecloser.ValueMember = "Id";

            cbTypeTT.DataSource = _dto[0].TypeRecloser[0].TypeTT;
            cbTypeTT.DisplayMember = "Name";
            cbTypeTT.ValueMember = "Id";
        }

        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public string TypeRecloser
        {
            get { return _typeRecloser; }
            set { _typeRecloser = value; }
        }
        public string TypeTT
        {
            get { return _typeTT; }
            set { _typeTT = value; }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ElementName = tbElementName.Text;
            TypeRecloser = cbTypeRecloser.SelectedItem.ToString();
            TypeTT = cbTypeTT.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
