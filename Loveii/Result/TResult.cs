using System;

namespace Loveii
{
    /// <summary>
    /// 泛型结果实体类
    /// </summary>
    public class TResult<T> : Result
    {
        /// <summary>
        /// 泛型T相关项
        /// </summary>
        private T _item;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TResult() : this(false,"") { }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="successed">是否成功</param>
        /// <param name="message"></param>
        public TResult(bool successed, string message)
        {
            this.Successed = successed;
            this.Message = message;

            if (typeof(T) == typeof(int) || typeof(T) == typeof(string))
                this.Item = default(T);
            else
                this.Item = (T)Activator.CreateInstance(typeof(T));
        }

        /// <summary>
        /// 泛型对象
        /// </summary>
        public T Item
        {
            get { return _item; }
            set { _item = value; }
        }

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
