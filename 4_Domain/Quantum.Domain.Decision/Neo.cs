﻿using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision.Keys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.Decision
{
    /// <summary>
    /// Matrix中的救世主，依靠他的能力来做出判断
    /// 组成Neo（尼奥）的这三个字母掉转顺序后就可以组成“one”，表示他就是那个拯救人类的救世主“The One”。
    /// 而“基督”一词在希伯来语中的本意就是“被指定的那个人”——The One
    /// </summary>
    [Serializable]
    public class Neo
    {
        #region Field
        private ISecurity _security;

        private List<IKey> _keys;

        private Trinity _trinity;

        [NonSerialized]
        private bool _logined;
        [NonSerialized]
        private IBattleship _battleship;
        #endregion

        #region Constructor
        public Neo(ISecurity security, IEnumerable<IKey> keys)
        {
            _security = security;
            _keys = keys.ToList();
        }
        #endregion

        #region Property
        /// <summary>
        /// 获取Neo关注的标的
        /// </summary>
        public ISecurity Security
        {
            get { return _security; }
        }
        /// <summary>
        /// 获取Neo持有的所有能打开Matrix后门的钥匙
        /// </summary>
        public IEnumerable<IKey> Keys
        {
            get { return _keys; }
        }
        #endregion

        #region Event
        public EventHandler<IBullet> Shootted;

        private void OnShootted(IBullet e)
        {
            EventHandler<IBullet> handler = Shootted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 通过战舰联系到操作员登陆到Matrix
        /// </summary>
        /// <param name="battleship"></param>
        public void Login(IBattleship battleship)
        {
            _battleship = battleship;
            _battleship.UpgradeOperator(this);
            _logined = true;
        }

        /// <summary>
        /// 当新数据到来的时候，调用此方法
        /// </summary>
        /// <param name="kLineType"></param>
        public void OnKLineComing(KLineType kLineType)
        {
            // 未登陆和没有飞船，都无法处理新数据
            if (!_logined || _battleship == null) return;
            
            Decide(kLineType);
        }
        #endregion

        #region Private Method
        private void Decide(KLineType kLineType)
        {
            bool isBuy = false;
            bool isSell = false;

            // 运算所有买指标的结果
            var buyResult = _keys
                .Where(p => p.KeyType == KeyType.Buy)
                .Select(p => p.Match(_battleship.Link))
                .Distinct().ToList();
            if(buyResult.Count == 1 &&
                buyResult[0] == true)
            {
                isBuy = true;
            }

            if(isBuy == false) // 如果已经决定要买了，就不用判断卖了
            {
                var sellResult = _keys
                    .Where(p => p.KeyType == KeyType.Sell)
                    .Select(p => p.Match(_battleship.Link))
                    .Distinct().ToList();

                if (sellResult.Count == 1 &&
                sellResult[0] == true)
                {
                    isSell = true;
                }
            }
            
            if(!isBuy && !isSell)
            {
                return; // 不买不卖就不进行操作了
            }

            Bullet bullet = new Bullet
            {
                Quantity = GetQuantity(isBuy),
                IsUp = isBuy
            };

            // todo: 内部系统的交易
            // Trinity

            OnShootted(bullet);
        }

        private int GetQuantity(bool isBuy)
        {
            return 100;
        }
        #endregion
    }
}
