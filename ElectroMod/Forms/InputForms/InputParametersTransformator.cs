using ElectroMod.DataBase.Dtos;
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

namespace ElectroMod.Forms
{
    public partial class InputParametersTransformator : Form
    {
        private List<TransformatorDataTypeDto> _dto;
        private string _elementName;
        private string _typeKTP;
        private string _shemeConnectingWinding;

        public InputParametersTransformator()
        {
            InitializeComponent();
            LoadDataToComboBox();
        }

        private void LoadDataToComboBox()
        {
            _dto = JsonProvider.LoadData<TransformatorDataTypeDto>("..\\..\\DataBase\\TransformatorDataTypeDB.json");
            cbTypeKTP.DataSource = _dto[0].TypeKTP;
            cbTypeKTP.DisplayMember = "Name";
            cbTypeKTP.ValueMember = "Id";

            cbShemesConnectWinding.DataSource = _dto[0].ShemesConnectingWinding;
            cbShemesConnectWinding.DisplayMember = "Name";
            cbShemesConnectWinding.ValueMember = "Id";
        }

        public string ElementName 
        { 
            get { return _elementName; } 
            set { _elementName = value; } 
        }
        public string TypeKTP
        {
            get { return _typeKTP; }
            set { _typeKTP = value; }
        }
        public string ShemeConnectingWinding
        {
            get { return _shemeConnectingWinding; }
            set { _shemeConnectingWinding = value; }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ElementName = tbElementName.Text;
            TypeKTP = cbTypeKTP.SelectedItem.ToString();
            ShemeConnectingWinding = cbShemesConnectWinding.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
