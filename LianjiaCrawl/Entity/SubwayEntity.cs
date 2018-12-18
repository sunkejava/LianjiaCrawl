using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LianjiaCrawl.Entity
{
    /// <summary>
    /// 类    名: SubwayEntity.cs
    /// CLR 版本: 4.0.30319.42000
    /// 作    者: sunkejava 
    /// 邮    箱：declineaberdeen@foxmail.com
    /// 创建时间: 2018/12/18 16:44:19
    /// 说    明：地铁类
    /// </summary>
    public class SubwayEntity
    {
        private string id;
        private string name;
        private string url;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
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
    }
}
