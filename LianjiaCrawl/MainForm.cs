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
using System.Threading;

namespace LianjiaCrawl
{
    public partial class MainForm : LayeredForm
    {
        public MainForm()
        {
            InitializeComponent();
        }
        #region 公共变量
        public static string mainUrl = "https://bj.lianjia.com/ershoufang/";//默认首页地址
        public static List<Entity.SelectEntity> areas = new List<Entity.SelectEntity>();//地区集合
        public static List<Entity.SelectEntity> subways = new List<Entity.SelectEntity>();//地铁线路集合
        public static List<Entity.SelectEntity> selectChidens = new List<Entity.SelectEntity>();//末级集合，如：海淀区下的集合 或 一号线的站点
        public static string findCountHouse = "";
        public static string yjname = "全部";
        public static string ejname = "";
        public delegate void UpdateUI(string type,string value);//声明一个更新控件信息的委托
        public UpdateUI UpdateUIDelegate;
        public delegate void AccomplishTask();//声明一个在完成任务时通知主线程的委托
        public AccomplishTask TaskCallBack;
        public Thread t = null;
        delegate void AsynUpdateUI(string type, string value);
        #endregion


        #region 控件事件
        private void layeredButton_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void layeredButton_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void layeredTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.Opacity = Bar_kzt.Value;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Thread at = new Thread(new ThreadStart(getAreasAndSubway));
            at.Start();
            //getAreasAndSubway();
        }

        private void Button_area_Click(object sender, EventArgs e)
        {
            Button_area.BackColor = Color.FromArgb(174, 102, 1);
            Button_subway.BackColor = Color.Transparent;
            Panel_sc.DUIControls.Clear();
            Panel_xj.DUIControls.Clear();
            addSelectControls(areas);
        }

        private void BaseButton_MouseClick(object sender, DuiMouseEventArgs e)
        {
            selectedButtonSkin(sender as DuiButton);
            selectChidens.Clear();
            yjname = (sender as DuiButton).Text;
            string url = (sender as DuiButton).Name.Replace("yjbaseButton_", "").Replace("ejbaseButton_", "");
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
                string rastr = ress.OuterHtml.Replace("<div class=\"sub_sub_nav sbw_sub_nav\" id=\"sbw_line\" data-d='", "").Replace("' class=\"div_relative\">", "").Replace("</div>", "").Replace(" ", "");
                rastr = Unicode2String(rastr);
                JObject jo = (JObject)JsonConvert.DeserializeObject(rastr);
                foreach (var item in jo)
                {
                    Console.WriteLine(item.Value["name"].ToString() + "---" + item.Value["url"].ToString());
                    Entity.SelectEntity se = new Entity.SelectEntity();
                    se.Name = item.Value["name"].ToString();
                    se.Rtag = item.Value["name"].ToString();
                    se.Url = item.Value["url"].ToString();
                    selectChidens.Add(se);
                }
            }
            //当前总共找到XX个房源信息
            //getAllCrawlText(doc);
            addSelectChiendControls(selectChidens);
            if (t != null && t.ThreadState != ThreadState.Stopped && t.ThreadState != ThreadState.Aborted)
            {
                t.Abort();
            }
            t = new Thread(new ParameterizedThreadStart(getAllCrawlText));
            docStruct ds = new docStruct();
            UpdateUIDelegate += updateLabelText;
            ds.doc = doc;
            t.Start(ds);
        }

        private void ABaseButton_MouseClick(object sender, DuiMouseEventArgs e)
        {
            selectedButtonSkin(sender as DuiButton);
            ejname = (sender as DuiButton).Text;
            string url = (sender as DuiButton).Name.Replace("yjbaseButton_", "").Replace("ejbaseButton_", "");
            //根据所选地区或线路获取详细信息
            var htmlStr = GetWebClient(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlStr);
            //getAllCrawlText(doc);
            if (t != null && t.ThreadState != ThreadState.Stopped && t.ThreadState != ThreadState.Aborted)
            {
                t.Abort();
            }
            t = new Thread(new ParameterizedThreadStart(getAllCrawlText));
            docStruct ds = new docStruct();
            UpdateUIDelegate += updateLabelText;
            ds.doc = doc;
            t.Start(ds);
        }

        private void Button_subway_Click(object sender, EventArgs e)
        {
            Button_subway.BackColor = Color.FromArgb(174, 102, 1);
            Button_area.BackColor = Color.Transparent;
            Panel_sc.DUIControls.Clear();
            Panel_xj.DUIControls.Clear();
            addSelectControls(subways);
        }

        #endregion

        #region 自定义事件
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
                    Console.WriteLine("name:" + name + "---tags:" + tags + "---url:" + aurl);
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
            t = new Thread(new ParameterizedThreadStart(getAllCrawlText));
            docStruct ds = new docStruct();
            ds.doc = doc;
            UpdateUIDelegate += updateLabelText;
            TaskCallBack += ThisTaskCallBack;
            t.Start(ds);
            //getAllCrawlText(doc);
        }
        struct docStruct{
            public HtmlAgilityPack.HtmlDocument doc;
        }
        private void selectedButtonSkin(DuiButton db)
        {
            if (db.Name.Contains("yjbaseButton"))
            {
                foreach (DuiBaseControl itemDb in Panel_sc.DUIControls)
                {
                    itemDb.BackColor = Color.Transparent;
                }
            }
            else
            {
                foreach (DuiBaseControl itemDb in Panel_xj.DUIControls)
                {
                    itemDb.BackColor = Color.Transparent;
                }
            }
            db.BackColor = Color.FromArgb(174,102,1);
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
        /// <summary>
        /// 添加一级菜单按钮
        /// </summary>
        /// <param name="lists"></param>
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
                baseButton.Name = "yjbaseButton_" + item.Url;
                baseButton.MouseClick += BaseButton_MouseClick;
                i++;
                baseControl.Controls.Add(baseButton);
            }
            Panel_sc.DUIControls.Add(baseControl);
        }
        /// <summary>
        /// 添加二级菜单按钮
        /// </summary>
        /// <param name="sLists"></param>
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
                baseButton.Name = "ejbaseButton_" + item.Url;
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
        private void getAllCrawlText(Object ob)
        {
            docStruct ds = (docStruct)ob;
            HtmlAgilityPack.HtmlDocument doc = ds.doc; 
            DuiTextBox duia = ((DuiTextBox)lp_panel.DUIControls[0]);
            duia.Dock = DockStyle.Fill;
            duia.Multiline = true;
            duia.Size = new Size(999, 389);
            duia.Text = "";
            //当前页数、总页数
            var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[4]/div[1]/div[8]/div[2]");
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
            UpdateUIDelegate("label_pagecount", "总页数：" + pcount);
            //label_pagecount.Text = "总页数：" + pcount;
            UpdateUIDelegate("label_nowcrawlpage", "当前正在采集" + yjname + ejname + "第 " + npage + " 页的房源信息！");
            //label_nowcrawlpage.Text = "当前正在采集" + yjname + ejname + "第 " + npage + " 页的房源信息！";
            //当前总共找到XX个房源信息
            res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[4]/div[1]/div[1]");
            if (res != null)
            {
                var astr = res.SelectSingleNode(@"h2").SelectNodes("span");
                foreach (var item in astr)
                {
                    var count = item.InnerText;
                    findCountHouse = "共找到" + count.ToString() + "套北京在售二手房源";
                }
            }
            UpdateUIDelegate("label_count", findCountHouse);
            //this.label_count.Text = findCountHouse;
            //获取所有房源信息
            res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[4]/div[1]");
            if (res != null)
            {
                var astr = res.SelectSingleNode(@"ul[1]").SelectNodes("li");
                int i = 1;
                foreach (var item in astr)
                {
                    if (item.Attributes["class"].Value != "list_app_daoliu")
                    {
                        var itemstr = item.SelectNodes("a")[0];
                        UpdateUIDelegate("label_nowcrawlpage", "当前正在采集" + yjname + ejname + "第 " + npage + " 页第" + i.ToString() + "条的房源销售信息！");
                        //label_nowcrawlpage.Text = "当前正在采集" + yjname + ejname + "第 " + npage + " 页第"+i.ToString()+"条的房源销售信息！";
                        string userinfo = getHouseDetail(itemstr.Attributes["href"].Value);
                        string astrs = i.ToString() + "----" + itemstr.Attributes["href"].Value + "----" + itemstr.SelectNodes("img")[1].Attributes["alt"].Value + "----" + userinfo;
                        UpdateUIDelegate("duia", astrs + "\r\n");
                        //duia.Text += astrs + "\r\n";
                        i++;
                    }
                }
            }
            UpdateUIDelegate("label_nowcrawlpage", yjname + ejname + "第 " + npage + " 页的房源信息采集完毕！");
            //label_nowcrawlpage.Text = yjname + ejname + "第 " + npage + " 页的房源信息采集完毕！";
            TaskCallBack();
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
            //获取姓名
            string userInfo = "";
            var res = doc.DocumentNode.SelectSingleNode(@"html[1]/body[1]/div[5]/div[2]/div[5]/div[1]/div[1]");
            if (res != null)
            {
                var astr = res.SelectNodes("a");
                foreach (var item in astr)
                {
                    userInfo = item.InnerText;
                }
            }
            //获取电话
            res = doc.DocumentNode.SelectSingleNode(@"html[1]/body[1]/div[5]/div[2]/div[5]/div[1]/div[3]");
            if (res != null)
            {
                userInfo = userInfo+"----"+ res.InnerText.Replace("微信扫码拨号","");
            }
            return userInfo;
        }
        /// <summary>
        /// 更新控件信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        private void updateLabelText(string type,string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new AsynUpdateUI(delegate (string atype,string avalue)
                {
                    switch (atype)
                    {
                        case "label_pagecount":
                            label_pagecount.Text = avalue;
                            break;
                        case "label_nowcrawlpage":
                            label_nowcrawlpage.Text = avalue;
                            break;
                        case "label_count":
                            label_count.Text = avalue;
                            break;
                        case "duia":
                            DuiTextBox duia = ((DuiTextBox)lp_panel.DUIControls[0]);
                            duia.Text = duia.Text + value;
                            break;
                        default:
                            break;
                    }
                }), type,value);
            }
            else
            {
                switch (type)
                {
                    case "label_pagecount":
                        label_pagecount.Text = value;
                        break;
                    case "label_nowcrawlpage":
                        label_nowcrawlpage.Text = value;
                        break;
                    case "label_count":
                        label_count.Text = value;
                        break;
                    case "duia":
                        DuiTextBox duia = ((DuiTextBox)lp_panel.DUIControls[0]);
                        duia.Text = duia.Text + value;
                        break;
                    default:
                        break;
                }
            }
            
        }
        /// <summary>
        /// 线程执行完毕后的事件
        /// </summary>
        /// <param name="t"></param>
        private void ThisTaskCallBack()
        {
            //t.Abort();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"线程执行完毕！");
        }
        #endregion
    }
}
