using System;
using System.Collections.Generic;
using System.Text;

namespace Geister.GameInformation
{
    /// <summary>
    /// 盤面上の位置を表すクラス
    /// </summary>
    public class Position
    {
        #region [フィールド]
        /// <summary>
        /// 行
        /// </summary>
        public int Row;
        /// <summary>
        /// 列
        /// </summary>
        public int Col;
        #endregion

        #region　[コンストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="row">
        /// 盤面上の行
        /// </param>
        /// <param name="col">
        /// 盤面上の列
        /// </param>
        public Position(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
        #endregion

        #region [パブリックメソッド]
        /// <summary>
        /// 行,列を設定する
        /// </summary>
        /// <param name="row">
        /// 盤面上の行
        /// </param>
        /// <param name="col">
        /// 盤面上の列
        /// </param>
        public void SetPosition(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
        #endregion

        public Position Clone()
        {
			Position cloned = (Position)MemberwiseClone();
            return cloned;
		}

    }

}
