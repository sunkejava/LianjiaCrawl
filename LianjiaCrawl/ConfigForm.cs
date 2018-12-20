using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LayeredSkin.Forms;
using LayeredSkin.Controls;
using LayeredSkin.DirectUI;

namespace LianjiaCrawl
{
    public partial class ConfigForm : LayeredForm
    {

        #region 窗体事件
        DuiTextBox softNameTextBox = new DuiTextBox();
        DuiTextBox imgTextBox = new DuiTextBox();
        DuiTextBox fileTextBox = new DuiTextBox();
        DuiComboBox animation = new DuiComboBox();
        PropertsUtils pes = new PropertsUtils();
        MainForm mainForm = new MainForm();
        public ConfigForm(MainForm tmainForm)
        {
            InitializeComponent();
            mainForm = tmainForm;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            softNameTextBox.Text = pes.SoftName;
            imgTextBox.Text = pes.BackImg;
            fileTextBox.Text = pes.FilePath;
            tb_radius.Value = int.Parse(pes.Radius);
            tb_kzt.Value = int.Parse(pes.Opacity)/100;
            animation.Text =  pes.Animation;
            animation.Size = new Size(267,20);
            softNameTextBox.Size = new Size(267,20);
            imgTextBox.Size = new Size(237,20);
            fileTextBox.Size = imgTextBox.Size;
            softNameTextBox.Location = new Point(107,17);
            imgTextBox.Location = new Point(107,48);
            fileTextBox.Location = new Point(107,185);
            animation.Location = new Point(107,148);
            panel_xc.DUIControls.Add(softNameTextBox);
            panel_xc.DUIControls.Add(imgTextBox);
            panel_xc.DUIControls.Add(fileTextBox);
            panel_xc.DUIControls.Add(animation);
        }
        #endregion

        #region 控件事件
        private void btn_changeImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (!string.IsNullOrEmpty(imgTextBox.Text))
            {
                string filePath = System.IO.Path.GetDirectoryName(imgTextBox.Text);
                fileDialog.InitialDirectory = filePath;
            }
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择背景图片";
            fileDialog.Filter = "图片文件(*.jpg,*.png,*.jpeg,*.bmp)|*.jpg;*.png;*.jpeg;*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] names = fileDialog.FileNames;
                foreach (string file in names)
                {
                    imgTextBox.Text = file;
                }
            }
            this.BackgroundImage = Image.FromFile(imgTextBox.Text);
            mainForm.BackgroundImage = this.BackgroundImage;
            changeMainform();
        }
        private void layeredTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            LayeredTrackBar ltb = sender as LayeredTrackBar;
            if (ltb.Name == "tb_kzt")
            {
                this.Opacity = ltb.Value;
                mainForm.Opacity = this.Opacity;
                pes.Opacity = ltb.Value.ToString();
            }
            else
            {
                this.Radius = (int)(ltb.Value * this.Width);
                mainForm.Radius = this.Radius;
                pes.Radius = ltb.Value.ToString();
            }
            changeMainform();
        }
        #endregion

        #region 自定义事件
        private void changeMainform()
        {
            pes.SoftName = softNameTextBox.Text;
            pes.BackImg = imgTextBox.Text;
            pes.Opacity = tb_kzt.Value.ToString();
            pes.Radius = tb_radius.Value.ToString();
            pes.FilePath = fileTextBox.Text;
            pes.Animation = animation.Text;
            mainForm.perUtils = pes;
        }
        #endregion

        private void layeredButton_close_Click(object sender, EventArgs e)
        {
            pes.saveConfig();
            this.Close();
        }
    }
}
