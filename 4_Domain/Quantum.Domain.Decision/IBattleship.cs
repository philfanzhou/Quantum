namespace Quantum.Domain.Decision
{
    /// <summary>
    /// Nebuchadnezzar号飞船
    /// </summary>
    public interface IBattleship
    {
        /// <summary>
        /// 获取Link操作员
        /// </summary>
        Link Link { get; }

        /// <summary>
        /// 根据Neo对数据的要求，升级飞船上的接线员
        /// </summary>
        /// <param name="neo"></param>
        /// <returns></returns>
        void UpgradeOperator(Neo neo);
    }
}
