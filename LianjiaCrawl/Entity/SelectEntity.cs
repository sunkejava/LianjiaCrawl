using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LianjiaCrawl.Entity
{
    /// <summary>
    /// 类    名: SelectEntity.cs
    /// CLR 版本: 4.0.30319.42000
    /// 作    者: sunkejava 
    /// 邮    箱：declineaberdeen@foxmail.com
    /// 创建时间: 2018/12/18 14:19:37
    /// 说    明：
    /// </summary>
    public class SelectEntity
    {
        private string url;
        private string name;
        private string rtag;

        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Rtag
        {
            get
            {
                return rtag;
            }

            set
            {
                rtag = value;
            }
        }
    }
}
