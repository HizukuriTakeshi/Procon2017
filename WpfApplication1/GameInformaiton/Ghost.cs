using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.GameInformation
{


    public class Ghost
    {
        #region [フィールド]
        /// <summary>
        /// ゴーストのタイプ
        /// </summary>
        private GhostAttribute gt;

        private Position initP;
        private Position p;

        private int playerNumber;

        #endregion

        #region [アクセサ]
        /// <summary>
        /// ゴーストのタイプのアクセサ
        /// </summary>
        public GhostAttribute Gt
        {
            set
            {
                gt = value;
            }

            get
            {
                return gt;
            }
        }

        public int PlayerNumber
        {
            set
            {
                playerNumber = value;
            }
            get
            {
                return playerNumber;
            }
        }

        public Position P
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
            }
        }

        public Position InitPos
        {
            get
            {
                return initP;
            }

            set
            {
                initP = value;
            }
        }
        #endregion

        #region [コンストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gt"></param>
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
