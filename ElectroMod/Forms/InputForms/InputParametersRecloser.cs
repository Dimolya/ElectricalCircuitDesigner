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
        private string _elementName;
        private string _typeRecloser;
        private string _typeTT;
        private List<string> _typesRecloser = new List<string>() { "Таврила электрик", "БМР3" };
        private List<string> _typesTT = new List<string>() { "5/10", "5/15","5/50" };

        public InputParametersRecloser()
        {
            InitializeComponent();
            cbTypeRecloser.DataSource = _typesRecloser;
            cbTypeTT.DataSource = _typesTT;
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
