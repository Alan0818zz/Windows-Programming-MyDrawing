using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.control
{
    public partial class EditText : Form
    {
        private string originText;

        public string TextBoxContent
        {
            get { return textContent.Text; }
        }

        public EditText(string originText)
        {
            this.originText = originText;
            InitializeComponent();
            InitButtons();
            InitTextBox();
        }

        private void InitTextBox()
        {
            this.textContent.TextChanged += (s, e) =>
            {
                this.btnConfirm.Enabled = textContent.Text != originText;
            };
            this.textContent.Text = originText;
        }

        private void InitButtons()
        {
            btnConfirm.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }
        private void textContent_TextChanged(object sender, EventArgs e)
        {

        }

        private void EditText_Load(object sender, EventArgs e)
        {

        }
    }
}
