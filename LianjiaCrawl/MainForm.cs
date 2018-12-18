using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LayeredSkin.Forms;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using LayeredSkin.DirectUI;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LianjiaCrawl
{
    public partial class MainForm : LayeredForm
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public static string mainUrl = "https://bj.lianjia.com/ershoufang/";
        public static List<Entity.SelectEntity> areas = new List<Entity.SelectEntity>();
        public static List<Entity.SelectEntity> subways = new List<Entity.SelectEntity>();
        public static List<Entity.SelectEntity> selectChidens = new List<Entity.SelectEntity>();
        private void layeredButton_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void layeredButton_close_Click(object sender, EventArgs e)
        {
            this.Animation.Effect = new LayeredSkin.Animations.GradualCurtainEffect();
            this.Animation.Asc = true;
            this.Close();
        }

        private void layeredTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.Opacity = Bar_kzt.Value;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            getAreasAndSubway();
        }
        public static string GetWebClient(string url)
        {
            try
            {
                string strHTML = "";
                WebClient myWebClient = new WebClient();
                Stream myStream = myWebClient.OpenRead(url);
                StreamReader sr = new StreamReader(myStream, Encoding.UTF8);//注意编码
                strHTML = sr.ReadToEnd();
                myStream.Close();
                return strHTML;
            }
            catch (Exception)
            {
                //连接失败
                return null;
            }
            
        }
        /// <summary>
        /// 获取地区及地铁信息
        /// </summary>
        public static void getAreasAndSubway()
        {
            var htmlStr = GetWebClient(mainUrl);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlStr);
            //地区
            var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[3]/div[1]/div[1]/dl[2]/dd[1]/div[1]");
            if (res != null)
            {
                var astr = res.SelectSingleNode(@"div").SelectNodes("a");
                foreach (var item in astr)
                {
                    var aurl = item.Attributes["href"].Value;
                    var tags = item.Attributes["title"].Value;
                    var name = item.InnerText;
                    Entity.SelectEntity area = new Entity.SelectEntity();
                    string lstr = (name == "燕郊" || name == "香河" ? "" : "https://bj.lianjia.com");
                    area.Url = lstr + aurl;
                    area.Rtag = tags;
                    area.Name = name;
                    areas.Add(area);
                    Console.WriteLine("name:"+name+"---tags:"+tags+"---url:"+aurl);
                }
            }
            //地铁线
            res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[3]/div[1]/div[1]/dl[2]/dd[1]/div[2]");
            if (res != null)
            {
                var astr = res.SelectSingleNode(@"div").SelectNodes("a");
                foreach (var item in astr)
                {
                    var aurl = item.Attributes["href"].Value;
                    var tags = item.Attributes["title"].Value;
                    var name = item.InnerText;
                    Entity.SelectEntity area = new Entity.SelectEntity();
                    area.Url = "https://bj.lianjia.com" + aurl;
                    area.Rtag = tags;
                    area.Name = name;
                    subways.Add(area);
                    Console.WriteLine("name:" + name + "---tags:" + tags + "---url:" + aurl);
                }
            }
        }

        private void Button_area_Click(object sender, EventArgs e)
        {
            Panel_sc.DUIControls.Clear();
            Panel_xj.DUIControls.Clear();
            addSelectControls(areas);
        }

        private void BaseButton_MouseClick(object sender, DuiMouseEventArgs e)
        {
            selectChidens.Clear();
            string url = (sender as DuiButton).Name.Replace("baseButton_", "");
            //根据所选地区或线路获取子信息
            var htmlStr = GetWebClient(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlStr);
            //子信息
            var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[3]/div[1]/div[1]/dl[2]/dd[1]/div[1]/div[2]");
            if (res != null)
            {
                var astr = res.SelectNodes("a");
                foreach (var item in astr)
                {
                    var aurl = item.Attributes["href"].Value;
                    var tags = item.InnerText;
                    var name = item.InnerText;
                    Entity.SelectEntity area = new Entity.SelectEntity();
                    area.Url = "https://bj.lianjia.com" + aurl;
                    area.Rtag = tags;
                    area.Name = name;
                    selectChidens.Add(area);
                }
            }
            else
            {
                //res为null则取地铁信息
                var ress = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[3]/div[1]/div[1]/dl[2]/dd[1]/div[2]/div[2]/@data-d[1]");
                string rastr = ress.OuterHtml.Replace("<div class=\"sub_sub_nav sbw_sub_nav\" id=\"sbw_line\" data-d='", "").Replace("' class=\"div_relative\">", "").Replace("</div>", "").Replace(" ","");
                rastr = Unicode2String(rastr);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                List<Entity.SubwayEntity> ListSubwat = (List<Entity.SubwayEntity>)JsonConvert.DeserializeObject(rastr);
                foreach (Entity.SubwayEntity item in ListSubwat)
                {
                    Console.WriteLine(item.Id + "---" + item.Name);
                }
            }
            addSelectChiendControls(selectChidens);
        }
        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        private void ABaseButton_MouseClick(object sender, DuiMouseEventArgs e)
        {
            string url = (sender as DuiButton).Name.Replace("baseButton_", "");
            //根据所选地区或线路获取详细信息
            var htmlStr = GetWebClient(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlStr);
            //子信息
            var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[3]/div[1]/div[1]/dl[2]/dd[1]/div[1]/div[2]");
            if (res != null)
            {
                var astr = res.SelectNodes("a");
                foreach (var item in astr)
                {
                    var aurl = item.Attributes["href"].Value;
                    var tags = item.InnerText;
                    var name = item.InnerText;
                    Entity.SelectEntity area = new Entity.SelectEntity();
                    area.Url = "https://bj.lianjia.com" + aurl;
                    area.Rtag = tags;
                    area.Name = name;
                    selectChidens.Add(area);
                }
            }
            addSelectChiendControls(selectChidens);
        }

        private void Button_subway_Click(object sender, EventArgs e)
        {
            Panel_sc.DUIControls.Clear();
            Panel_xj.DUIControls.Clear();
            addSelectControls(subways);
        }

        private void addSelectControls(List<Entity.SelectEntity> lists)
        {
            Panel_sc.DUIControls.Clear();
            DuiBaseControl baseControl = new DuiBaseControl();
            baseControl.Size = new Size(735, 78);
            baseControl.Location = new Point(0, 0);
            baseControl.Dock = DockStyle.Fill;
            int i = 1;
            int row = 1;
            //循环创建控件
            foreach (Entity.SelectEntity item in lists)
            {
                DuiButton baseButton = new DuiButton();
                baseButton.Size = new Size(60, 20);
                baseButton.BackColor = Color.Transparent;
                baseButton.BaseColor = Color.Transparent;
                if (i * 2 + 60 * (i) > this.Width)
                {
                    row++;
                    i = 1;
                }
                baseButton.Location = new Point(i * 2 + 60 * (i - 1), 2 * row + 20 * (row - 1));
                baseButton.Text = item.Name;
                if (item.Name.Length > 3)
                {
                    baseButton.Width = baseButton.Width * 2;
                    i++;
                }
                baseButton.Tag = item.Rtag;
                baseButton.Name = "baseButton_" + item.Url;
                baseButton.MouseClick += BaseButton_MouseClick;
                i++;
                baseControl.Controls.Add(baseButton);
            }
            Panel_sc.DUIControls.Add(baseControl);
        }

        private void addSelectChiendControls(List<Entity.SelectEntity> sLists)
        {
            Panel_xj.DUIControls.Clear();
            DuiBaseControl baseControl = new DuiBaseControl();
            baseControl.Size = new Size(735, 78);
            baseControl.Location = new Point(0, 0);
            baseControl.Dock = DockStyle.Fill;
            int i = 1;
            int row = 1;
            //循环创建控件
            foreach (Entity.SelectEntity item in sLists)
            {
                DuiButton baseButton = new DuiButton();
                baseButton.Size = new Size(60, 20);
                baseButton.BackColor = Color.Transparent;
                baseButton.BaseColor = Color.Transparent;
                if (i * 2 + 60 * (i) > this.Width)
                {
                    row++;
                    i = 1;
                }
                baseButton.Location = new Point(i * 2 + 60 * (i - 1), 2 * row + 20 * (row - 1));
                baseButton.Text = item.Name;
                if (item.Name.Length > 3)
                {
                    baseButton.Width = baseButton.Width * 2;
                    i++;
                }
                baseButton.Tag = item.Rtag;
                baseButton.Name = "baseButton_" + item.Url;
                baseButton.MouseClick += ABaseButton_MouseClick;
                i++;
                baseControl.Controls.Add(baseButton);
            }
            Panel_xj.DUIControls.Add(baseControl);
        }
    }
}
