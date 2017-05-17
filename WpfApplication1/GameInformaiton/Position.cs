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
        /// 行番号
        /// </summary>
        public int Row;
        /// <summary>
        /// 列番号
        /// </summary>
        public int Col;
        #endregion

        #region　[コンストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="row">
        /// 盤面上の行番号
        /// </param>
        /// <param name="col">
        /// 盤面上の列番号
        /// </param>
        public Position(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
        #endregion

        #region [パブリックメソッド]
        /// <summary>
        /// 行,列番号を設定する
        /// </summary>
        /// <param name="row">
        /// 盤面上の行番号
        /// </param>
        /// <param name="col">
        /// 盤面上の列番号
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
