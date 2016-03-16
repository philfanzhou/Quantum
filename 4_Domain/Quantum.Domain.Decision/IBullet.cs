namespace Quantum.Domain.Decision
{
    /// <summary>
    /// Neo打出的子弹
    /// </summary>
    public interface IBullet
    {
        /// <summary>
        /// 是否是向上打子弹
        /// 向上意味着买入
        /// </summary>
        bool IsUp { get; }

        /// <summary>
        /// 子弹的数量
        /// </summary>
        int Quantity { get; }
    }

    internal class Bullet : IBullet
    {
        public bool IsUp
        {
            get; internal set;
        }

        public int Quantity
        {
            get; internal set;
        }
    }
}
