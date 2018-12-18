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
using Newtonsoft.Json.Linq;

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
        public static string findCountHouse = "";
        public static string yjname = "全部";
        public static string ejname = "";
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
        public string GetWebClient(string url)
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
        public void getAreasAndSubway()
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
            //当前总共找到XX个房源信息
            getFindallHous(doc);
            getAllCrawlText(doc);
        }

        private void Button_area_Click(object sender, EventArgs e)
        {
            //yjname =(sender as DuiButton).Text;
            Panel_sc.DUIControls.Clear();
            Panel_xj.DUIControls.Clear();
            addSelectControls(areas);
        }

        private void BaseButton_MouseClick(object sender, DuiMouseEventArgs e)
        {
            selectChidens.Clear();
            ejname = (sender as DuiButton).Text;
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
                JObject jo = (JObject)JsonConvert.DeserializeObject(rastr);
                foreach (var item in jo)
                {
                    Console.WriteLine(item.Value["name"].ToString()+"---"+item.Value["url"].ToString());
                    Entity.SelectEntity se = new Entity.SelectEntity();
                    se.Name = item.Value["name"].ToString();
                    se.Rtag = item.Value["name"].ToString();
                    se.Url = item.Value["url"].ToString();
                    selectChidens.Add(se);
                }
            }
            //当前总共找到XX个房源信息
            getFindallHous(doc);
            getAllCrawlText(doc);
            addSelectChiendControls(selectChidens);
        }
        private void getFindallHous(HtmlAgilityPack.HtmlDocument doc)
        {
            //当前总共找到XX个房源信息
            var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[4]/div[1]/div[1]");
            if (res != null)
            {
                var astr = res.SelectSingleNode(@"h2").SelectNodes("span");
                foreach (var item in astr)
                {
                    var count = item.InnerText;
                    findCountHouse = "共找到" + count.ToString() + "套北京在售二手房源";
                }
            }
            this.label_count.Text = findCountHouse;
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
            getFindallHous(doc);
            getAllCrawlText(doc);
        }

        private void Button_subway_Click(object sender, EventArgs e)
        {
            //yjname = (sender as DuiButton).Text;
            Panel_sc.DUIControls.Clear();
            Panel_xj.DUIControls.Clear();
            addSelectControls(subways);
        }

        private void addSelectControls(List<Entity.SelectEntity> lists)
        {
            Panel_sc.DUIControls.Clear();
            DuiBaseControl baseControl = new DuiBaseControl();
            baseControl.Size = new Size(999, 78);
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
            baseControl.Size = new Size(999, 78);
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

        /// <summary>
        /// 获取房源标题信息
        /// </summary>
        /// <param name="doc"></param>
        private void getAllCrawlText(HtmlAgilityPack.HtmlDocument doc)
        {
            DuiTextBox duia = ((DuiTextBox)lp_panel.DUIControls[0]);
            duia.Dock = DockStyle.Fill;
            duia.Multiline = true;
            duia.Size = new Size(999,389);
            duia.Text = "";
            //当前总共找到XX个房源信息
            var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[4]/div[1]");
            if (res != null)
            {
                var astr = res.SelectSingleNode(@"ul[1]").SelectNodes("li");
                int i = 1;
                foreach (var item in astr)
                {
                    if (item.Attributes["class"].Value != "list_app_daoliu")
                    {
                        var itemstr = item.SelectNodes("a")[0];
                        string userinfo = getHouseDetail(itemstr.Attributes["href"].Value);
                        string astrs = i.ToString()+"----" + itemstr.Attributes["href"].Value + "----" + itemstr.SelectNodes("img")[1].Attributes["alt"].Value+"----"+ userinfo;
                        duia.Text += astrs + "\r\n";
                        i++;
                    }
                }
            }
            //当前页数、总页数
            res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[4]/div[1]/div[8]/div[2]");
            string str = res.SelectNodes("div")[0].Attributes["page-data"].Value;
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            string pcount = "";
            string npage = "";
            foreach (var item in jo)
            {
                if (item.Key.Contains("totalPage"))
                {
                    pcount = item.Value.ToString();
                }
                else
                {
                    npage = item.Value.ToString();
                }
            }
            label_pagecount.Text = "总页数：" + pcount;
            label_nowcrawlpage.Text = "当前正在采集" + yjname + ejname + "第 " + npage + " 页的信息！";
        }
        /// <summary>
        /// 获取房源详细信息
        /// </summary>
        /// <param name="url"></param>
        private string getHouseDetail(string url)
        {
            var htmlStr = GetWebClient(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlStr);
            ///html[1]/body[1]/div[5]/div[2]/div[5]/div[1]/div[1]  a  姓名
            ////html[1]/body[1]/div[5]/div[2]/div[5]/div[1]/div[3] 待处理  电话
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
            return "";
        }
    }
}
