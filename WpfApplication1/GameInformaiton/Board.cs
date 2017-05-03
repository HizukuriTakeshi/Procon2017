using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.GameInformation
{
    /// <summary>
    /// 表示のための盤面を表すクラス
    /// </summary>
    public class Board
    {

        #region  [パブリック変数]
        /// <summary>
        /// ボード上のオブジェクト情報
        /// </summary>
        private Square[,] m_object = new Square[6, 6];
        #endregion


        #region [アクセサ]
        /// <summary>
        /// Objectのアクセサ
        /// </summary>
        public Square[,] M_object
        {
            set
            {
                this.m_object = value;
            }

            get
            {
                return this.m_object;
            }
        }
        #endregion

        #region [コンストラクタ]
        /// <summary>
        /// Boardコンストラクタ
        /// </summary>
        public Board()
        {

        }
        #endregion

        #region [パブリックメソッド]

        #endregion
    }

}

