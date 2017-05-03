using System;
using System.Collections.Generic;
using System.Text;

namespace Geister.GameInformation
{

    /// <summary>
    /// ゴーストの位置と属性を保持するクラス
    /// </summary>
    public class Ghost
    {

        #region [アクセサ]

        /// <summary>
        /// ゴーストの属性
        /// </summary>
        public GhostAttribute Gt
        {
            set;

            get;
        }

        /// <summary>
        /// ゴーストの現在位置
        /// </summary>
        public Position P
        {
            get;
            set;
        }

        /// <summary>
        /// ゴーストの初期位置
        /// </summary>
        public Position InitPos
        {
            get;
            set;
        }
        #endregion

        #region [コンストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gt">
        /// ゴーストの属性
        /// </param>
        /// <param name="initPos">
        /// ゴーストの初期位置
        /// </param>
        public Ghost(GhostAttribute gt, Position initPos)
        {
            Gt = gt;
            InitPos = initPos;
            P = initPos;
        }

        public Ghost Clone()
        {
            Ghost cloned = (Ghost)MemberwiseClone();

            if (this.InitPos != null)
            {
                cloned.InitPos = (Position)this.InitPos.Clone();
            }

            if (this.P != null)
            {
                cloned.P = (Position)this.P.Clone();
            }

            return cloned;
        }

        #endregion
    }
}
