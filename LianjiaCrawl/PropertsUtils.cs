using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LianjiaCrawl
{
    /// <summary>
    /// 类    名: PropertsUtils.cs
    /// CLR 版本: 4.0.30319.42000
    /// 作    者: sunkejava 
    /// 邮    箱：declineaberdeen@foxmail.com
    /// 创建时间: 2018/12/20 10:23:22
    /// 说    明：配置文件使用说明
    /// </summary>
    public class PropertsUtils
    {
        private string softName = "链家二手房信息销售信息采集";
        private string backImg = "";
        private string radius = "15";
        private string opacity = "1";
        private string animation = "GradualCurtainEffect";
        private string filePath = "";
        private string stopTimeLength = "";
        public PropertsUtils()
        {
            this.softName = GetAppConfig("softName");
            this.backImg = GetAppConfig("backImg");
            this.radius = GetAppConfig("Radius");
            this.opacity = GetAppConfig("Opacity");
            this.animation = GetAppConfig("Animation");
            this.filePath = GetAppConfig("filePath");
            this.stopTimeLength = GetAppConfig("stopTimeLength");
        }
        /// <summary>
        /// 软件名称
        /// </summary>
        public string SoftName
        {
            get
            {
                return softName;
            }

            set
            {
                softName = value;
            }
        }
        /// <summary>
        /// 背景图片
        /// </summary>
        public string BackImg
        {
            get
            {
                return backImg;
            }

            set
            {
                backImg = value;
            }
        }
        /// <summary>
        /// 圆角度数
        /// </summary>
        public string Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }
        /// <summary>
        /// 透明度
        /// </summary>
        public string Opacity
        {
            get
            {
                return opacity;
            }

            set
            {
                opacity = value;
            }
        }
        /// <summary>
        /// 窗体动画
        /// </summary>
        public string Animation
        {
            get
            {
                return animation;
            }

            set
            {
                animation = value;
            }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
            }
        }
        /// <summary>
        /// 暂停时间长度，单位为秒
        /// </summary>
        public string StopTimeLength {
            get {
                return stopTimeLength;
                 }
            set {
                stopTimeLength = value;
            }
        }

        /// <summary>
        /// 根据Key值获取value值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private static string GetAppConfig(string strKey)
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == strKey)
                {
                    return config.AppSettings.Settings[strKey].Value.ToString();
                }
            }
            return null;
        }
        /// <summary>
        /// 更新key,values
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="newValue"></param>
        private static void UpdateAppConfig(string newKey, string newValue)
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            bool exist = false;
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == newKey)
                {
                    exist = true;
                }
            }
            if (exist)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void saveConfig()
        {
            UpdateAppConfig("softName", softName);
            UpdateAppConfig("backImg", backImg);
            UpdateAppConfig("Radius", radius);
            UpdateAppConfig("Opacity", opacity);
            UpdateAppConfig("Animation", animation);
            UpdateAppConfig("filePath", filePath);
            UpdateAppConfig("stopTimeLength", stopTimeLength);
        }
    }
}
