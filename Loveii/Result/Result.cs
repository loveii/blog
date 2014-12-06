
namespace Loveii
{
    /// <summary>
    /// 简单型结果实体类
    /// </summary>
    public class Result : IResult
    {
        /// <summary>
        /// 返回结果类
        /// </summary>
        public Result()
        {
            this.Successed = false;
            this.Message = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="successed"></param>
        public Result(bool successed, string message)
        {
            this.Message = message;
            this.Successed = successed;
        }

        /// <summary>
        /// 返回是否成功
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

    }
}
