using System.Collections.Generic;
using System;

namespace Loveii
{
    /// <summary>
    /// 分页结果实体类
    /// </summary>
    public class PageResult<T> : IResult
    {
        /// <summary>
        /// 默认构造函数
        /// PageIndex 默认为 1
        /// PageSize  默认为 10
        /// </summary>
        public PageResult()
            : base()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
            this.TotalCount = 0;

            this.Successed = false;
            this.Message = "";
            this.Item = new List<T>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalCount">总记录行数</param>
        public PageResult(int pageIndex, int pageSize, int totalCount) : base()
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
        }

        /// <summary>
        /// 泛型实体
        /// </summary>
        public List<T> Item { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        public int PageCount
        {
            get
            {
                if (TotalCount > 0 && PageSize > 0)
                {
                    return ((TotalCount - 1) / PageSize) + 1; //List.Count实际记录数
                }

                return 0;
            }
        }

        private int _pageIndex;

        /// <summary>
        /// 索引页
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (_pageIndex == 0)
                    _pageIndex = 1;

                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 上一页
        /// </summary>
        public int PrevPage
        {
            get
            {
                if (PageIndex == 1 || PageCount == 0)
                    return 0;

                return PageIndex - 1;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public int NextPage
        {
            get
            {
                if (PageCount == 0)
                    return 0;

                if (PageIndex < PageCount)
                    return PageIndex + 1;

                return PageIndex;
            }
        }

        /// <summary>
        /// 最大页数
        /// </summary>
        public int MaxPage
        {
            get
            {
                if (TotalCount > 0 && PageSize > 0)
                {
                    return ((TotalCount - 1) / PageSize) + 1;
                }

                return 0;
            }
        }

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
