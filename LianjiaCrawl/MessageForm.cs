using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LayeredSkin.DirectUI;
using LayeredSkin.Forms;

namespace LianjiaCrawl
{
    public partial class MessageForm : LayeredForm
    {
        private static StringBuilder MessageStr = null;
        public MessageForm(StringBuilder MessageStrs)
        {
            InitializeComponent();
            MessageStr = MessageStrs;
            setMessageStr();
        }

        private void setMessageStr()
        {
            DuiTextBox duia = ((DuiTextBox)lp_panel.DUIControls[0]);
            duia.Dock = DockStyle.Fill;
            duia.Multiline = true;
            duia.Size = new Size(395, 123);
            duia.Text = MessageStr.ToString();
        }

        private void Button_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void layeredButton_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
