using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using Loveii.Cache;
using System.Data;

namespace Loveii.Repositories
{
    public class LinkRepository
    {
        public static readonly string LINKLIST_KEY = "/Link/List";

        public List<Link> GetList()
        {
            string sql = "SELECT * FROM li_links ORDER BY `order`";

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);

            List<Link> list = CacheHelper.Get<List<Link>>(LINKLIST_KEY);
            if (list == null)
            {
                list = context.ExecuteList<Link>(LoadModle);
                CacheHelper.Set(LINKLIST_KEY, list, 86400);
            }

            return list;
        }

        #region ------Load Model------
        /// <summary>
        /// 加载下载基本信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Link LoadModle(IDataReader dr)
        {
            Link model = new Link();
            model.id = dr.GetInt32("id");
            model.name = dr.GetString("name");
            model.logo = dr.GetString("logo");
            model.url = dr.GetString("url");
            model.typeId = Convert.ToByte(dr["typeId"]);
            model.order = Convert.ToByte(dr["order"]);
            return model;
        }
        #endregion
    }
}
