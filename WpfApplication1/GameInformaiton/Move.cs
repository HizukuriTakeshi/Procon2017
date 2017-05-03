using System;
using System.Collections.Generic;
using System.Text;

namespace Geister.GameInformation
{
    /// <summary>
    /// 動かすゴーストと方向を示すクラス
    /// </summary>
    public class Move
    {
        
        #region　[アクセサ]
        /// <summary>
        /// Position
        /// </summary>
        public Position Pos
        {
            set;
            get;
        }

        /// <summary>
        /// GhostMove
        /// </summary>
        public GhostMove GhostM
        {
            set;
            get;
        }
        #endregion 

        #region [コンストラクタ]
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        /// <remarks>
        /// ゴーストの位置と移動方向を設定する
        /// </remarks>
        /// <param name="p">
        /// 
        /// </param>
        /// <param name="gm">
        /// 
        /// </param>
        public Move(Position p, GhostMove gm)
        {
            Pos = p;
            GhostM = gm;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// ゴーストの位置と移動方向を設定する
        /// </remarks>
        /// <param name="x">
        /// X座標
        /// </param>
        /// <param name="y">
        /// Y座標
        /// </param>
        /// <param name="gm">
        /// ゴーストの移動方向
        /// </param>
        public Move(int x, int y, GhostMove gm)
        {
            Pos = new Position(x, y);
            GhostM = gm;
        }
        #endregion

    }
}
