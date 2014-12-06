
namespace Loveii
{
    /// <summary>
    /// 结果接口
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// 返回是否成功
        /// </summary>
        bool Successed { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        string Message { get; set; }
    }
}
