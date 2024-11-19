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
        private string _elementName;
        private string _elementLength;
        private string _mark;
        private List<string> _marks = new List<string>() { "АС35", "АС50", "АС70" };

        public InputParametersLine()
        {
            InitializeComponent();
            cbMarks.DataSource = _marks;
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
        public string Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            ElementName = tbElementName.Text;
            ElementLength = tbElementName.Text;
            Mark = cbMarks.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
