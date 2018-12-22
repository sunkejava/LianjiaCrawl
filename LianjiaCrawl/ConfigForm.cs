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
using System.IO;

namespace LianjiaCrawl
{
    public partial class ConfigForm : LayeredForm
    {

        #region 窗体事件
        DuiTextBox softNameTextBox = new DuiTextBox();
        DuiTextBox imgTextBox = new DuiTextBox();
        DuiTextBox fileTextBox = new DuiTextBox();
        DuiTextBox timeLengthTextBox = new DuiTextBox();
        DuiComboBox animation = new DuiComboBox();
        PropertsUtils pes = new PropertsUtils();
        MainForm mainForm = new MainForm();
        public ConfigForm(MainForm tmainForm)
        {
            mainForm = tmainForm;
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            softNameTextBox.Text = pes.SoftName;
            imgTextBox.Text = (String.IsNullOrEmpty(pes.BackImg) ? "" : pes.BackImg);
            fileTextBox.Text = (String.IsNullOrEmpty(pes.FilePath) ? Application.StartupPath : pes.FilePath);
            timeLengthTextBox.Text = pes.StopTimeLength;
            tb_radius.Value = Double.Parse(pes.Radius) / mainForm.Width;
            tb_kzt.Value = Double.Parse(pes.Opacity);
            animation.Text =  pes.Animation;
            animation.AutoDrawSelecedItem = true;
            animation.InnerListBox.ItemSize = new System.Drawing.Size(265, 18);
            animation.InnerListBox.Orientation = LayeredSkin.Controls.ListOrientation.Vertical;
            animation.InnerListBox.ShowScrollBar = true;
            animation.InnerListBox.BackColor = Color.Transparent;
            animation.IsMoveParentPaint = true;
            animation.InnerListBox.Size = new System.Drawing.Size(265, 75);
            animation.Size = new Size(267,17);
            DuiLabel dlb = new DuiLabel();
            dlb.Size = new Size(265,15);
            dlb.Location = new Point(0, 0);
            dlb.Text = "测试账号一";
            DuiLabel dlb2 = new DuiLabel();
            dlb2.Size = new Size(265, 15);
            dlb2.Location = new Point(0, 15);
            dlb2.Text = "测试账号二";
            DuiLabel dlb3 = new DuiLabel();
            dlb3.Size = new Size(265, 15);
            dlb3.Location = new Point(0, 30);
            dlb3.Text = "测试账号三";
            DuiLabel dlb4 = new DuiLabel();
            dlb4.Size = new Size(265, 15);
            dlb4.Location = new Point(0, 45);
            dlb4.Text = "测试账号四";
            DuiLabel dlb5 = new DuiLabel();
            dlb5.Size = new Size(265, 15);
            dlb5.Location = new Point(0, 60);
            dlb5.Text = "测试账号五";
            animation.Items.AddRange(new LayeredSkin.DirectUI.DuiBaseControl[] {
            dlb,
            dlb2,
            dlb3,
            dlb4,
            dlb5});
            softNameTextBox.Size = new Size(267,17);
            timeLengthTextBox.Size = softNameTextBox.Size;
            imgTextBox.Size = new Size(237,17);
            fileTextBox.Size = imgTextBox.Size;
            softNameTextBox.Location = new Point(107,17);
            imgTextBox.Location = new Point(107,48);
            fileTextBox.Location = new Point(107,185);
            animation.Location = new Point(107,146);
            timeLengthTextBox.Location = new Point(107,219);
            panel_xc.DUIControls.Add(softNameTextBox);
            panel_xc.DUIControls.Add(imgTextBox);
            panel_xc.DUIControls.Add(fileTextBox);
            panel_xc.DUIControls.Add(animation);
            panel_xc.DUIControls.Add(timeLengthTextBox);
        }

        private void layeredButton_close_Click(object sender, EventArgs e)
        {
            pes.saveConfig();
            this.Close();
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
                this.BackgroundImage = Image.FromFile(imgTextBox.Text);
                mainForm.BackgroundImage = this.BackgroundImage;
                changeMainform();
            }
        }
        private void layeredTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            LayeredTrackBar ltb = sender as LayeredTrackBar;
            if (ltb.Name == "tb_kzt")
            {
                this.Opacity = ltb.Value;
                mainForm.Opacity = ltb.Value;
                pes.Opacity = ltb.Value.ToString();
            }
            else
            {
                this.Radius = (int)(ltb.Value * mainForm.Width);
                mainForm.Radius = this.Radius;
                pes.Radius = ltb.Value.ToString();
            }
            changeMainform();
        }

        private void btn_filePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (!string.IsNullOrEmpty(fileTextBox.Text))
            {
                string filePath = System.IO.Path.GetDirectoryName(fileTextBox.Text);
                fileDialog.InitialDirectory = filePath;
            }
            else
            {
                fileDialog.InitialDirectory = Application.StartupPath;
            }
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择目录";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileTextBox.Text = Path.GetDirectoryName(fileDialog.FileName);
                changeMainform();
            }
        }
        #endregion

        #region 自定义事件
        private void changeMainform()
        {
            pes.SoftName = softNameTextBox.Text;
            pes.BackImg = imgTextBox.Text;
            pes.Opacity = tb_kzt.Value.ToString();
            pes.Radius = (tb_radius.Value*mainForm.Width).ToString();
            pes.FilePath = fileTextBox.Text;
            pes.Animation = animation.Text;
            pes.StopTimeLength = timeLengthTextBox.Text;
            mainForm.perUtils = pes;
        }
        #endregion

        
    }
}
