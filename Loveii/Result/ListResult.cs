using System.Collections.Generic;
using System;

namespace Loveii
{
    /// <summary>
    /// List结果实体类
    /// </summary>
    public class ListResult<T> : IResult
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ListResult()
        {
            this.Successed = false;
            this.Message = "";
            this.Item = new List<T>();
        }

        /// <summary>
        /// 泛型实体
        /// </summary>
        public List<T> Item { get; set; }

        /// <summary>
        /// 返回是否成功
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 添加Result结果
        /// </summary>
        /// <param name="result">Result类型</param>
        public void AddResult(Result result)
        {
            this.Message = result.Message;
            this.Successed = result.Successed;
        }

    }
}
