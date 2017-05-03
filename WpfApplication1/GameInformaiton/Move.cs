using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.GameInformation
{
    /// <summary>
    /// 動かすゴーストと方向を示すクラス
    /// </summary>
    public class Move
    {
        #region [フィールド]
        /// <summary>
        /// 移動させるゴーストの盤面の位置
        /// </summary>
        public Position pos;
        /// <summary>
        /// 移動させるゴーストの方向
        /// </summary>
        public GhostMove ghostM;
        #endregion

        #region　[アクセサ]
        /// <summary>
        /// Positionのアクセサ
        /// </summary>
        public Position Pos
        {
            set
            {
                this.pos = value;
            }
            get
            {
                return this.pos;
            }
        }

        /// <summary>
        /// GhostMoveのアクセサ
        /// </summary>
        public GhostMove GhostM
        {
            set
            {
                this.ghostM = value;
            }

            get
            {
                return this.ghostM;
            }
        }
        #endregion 

        #region [コンストラクタ]
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        /// <remarks>
        /// ゴーストの位置と移動先を設定する
        /// </remarks>
        /// <param name="p"></param>
        /// <param name="gm"></param>
        public Move(Position p, GhostMove gm)
        {
            Pos = p;
            GhostM = gm;
        }
        #endregion

    }
}
