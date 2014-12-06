using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using Loveii.Services;

namespace Loveii
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            this.Error = new Dictionary<string, string>();
            this.Item = new Dictionary<string, object>();
            this.Result = new Result(false, "");
            this.NewsPostList = PostSrv.GetList();
            this.TermList = TermSrv.GetList();
            this.LinkList = LinkSrv.GetList(); 
        }

        public List<Post> NewsPostList { get; set; }

        public List<Term> TermList { get; set; }

        public List<Link> LinkList { get; set; } 

        /// <summary>
        /// 错误信息
        /// </summary>
        private Dictionary<string, string> Error { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        private Dictionary<string, object> Item { get; set; }

        /// <summary>
        /// 显示结果
        /// </summary>
        public Result Result { get; set; }


        #region ------- 自定义数据项操作 -------

        /// <summary>
        /// 添加自定义数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddItem(string key, object value)
        {
            if (this.Item.ContainsKey(key))
                this.Item[key] = value;
            else
                this.Item.Add(key, value);
        }

        /// <summary>
        /// 获取自定义数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetItem(string key)
        {
            if (this.Item.ContainsKey(key))
                return this.Item[key].ToString();

            return "";
        }

        /// <summary>
        /// 获取自定义数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetItem<T>(string key)
        {
            if (this.Item.ContainsKey(key))
                return (T)this.Item[key];

            return default(T);
        }

        /// <summary>
        /// 删除自定义数据
        /// </summary>
        /// <param name="key"></param>
        public void DelItem(string key)
        {
            if (this.Item.ContainsKey(key))
                this.Item.Remove(key);
        }

        #endregion


        #region ------- 错误消息操作 -------

        /// <summary>
        /// 添加错误
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddError(string key, string value)
        {
            if (this.Error.ContainsKey(key))
                this.Error[key] = value;
            else
                this.Error.Add(key, value);
        }

        /// <summary>
        /// 获取错误
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetError(string key)
        {
            if (this.Error.ContainsKey(key))
                return this.Error[key].ToString();

            return "";
        }

        /// <summary>
        /// 获取错误列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetErrorList()
        {
            return this.Error.Values.ToList();
        }

        #endregion


    }
}
