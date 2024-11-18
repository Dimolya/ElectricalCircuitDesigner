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
        private string _elementName;
        private string _typeKTP;
        private string _shemeConnectingWinding;
        private List<string> _typesKTP = new List<string>() { "Тип 1", "Тип 2" };
        private List<string> _shemesConnectingWinding = new List<string>() { "Звезда", "Треугольник" };

        public InputParametersTransformator()
        {
            InitializeComponent();
            cbTypeKTP.DataSource = _typesKTP;
            cbShemeConnectWinding.DataSource = _shemesConnectingWinding;
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
            ShemeConnectingWinding = cbShemeConnectWinding.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
