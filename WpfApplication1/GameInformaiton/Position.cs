using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.GameInformation
{
    /// <summary>
    /// 盤面上の位置を表すクラス
    /// </summary>
    public class Position
    {

        #region [フィールド]
        /// <summary>
        /// X座標
        /// </summary>
        public int X;
        /// <summary>
        /// Y座標
        /// </summary>
        public int Y;
        #endregion

        #region　[コンストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">盤面のx座標</param>
        /// <param name="y">盤面のy座標</param>
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion

        #region [パブリックメソッド]
        /// <summary>
        /// x,y座標を設定する
        /// </summary>
        /// <param name="x">盤面のx座標</param>
        /// <param name="y">盤面のy座標</param>
        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion

        public Position Clone()
        {
			Position cloned = (Position)MemberwiseClone();
            return cloned;
		}

    }

}
