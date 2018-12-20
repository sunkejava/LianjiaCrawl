using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LayeredSkin.Forms;

namespace LianjiaCrawl
{
    public partial class MenuStrip : LayeredForm
    {
        Point np = new Point(0,0);
        MainForm mf = null;
        public MenuStrip(Point ps,MainForm mainf)
        {
            InitializeComponent();
            np = ps;
            mf = mainf;
        }

        private void MenuStrip_Load(object sender, EventArgs e)
        {
            this.Location = new Point(np.X-this.Width,np.Y-this.Height);
            this.Width = Button_About.Width;
            this.TopLevel = true;
        }

        private void Button_About_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Button_Set_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.TopLevel = false;
            ConfigForm cf = new ConfigForm(mf);
            cf.Show();
        }

        private void Button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            GC.Collect();
        }
    }
}
