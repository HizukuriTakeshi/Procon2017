﻿using System;
using System.Collections.Generic;
using System.Text;
using Prototype.GameSystem;
using System.Diagnostics;

namespace Prototype.GameInformation
{
    //test
    public abstract class AbstractPlayer
    {
        #region [フィールド]
        /// <summary>
        /// プレイヤーの名前
        /// </summary>
        protected string name;
        /// <summary>
        /// プレイヤーの番号
        /// </summary>
        public int playerNumber;
        /// <summary>
        /// ゴーストの初期配置
        /// </summary>
        protected GhostAttribute[,] initialPlacement = new GhostAttribute[2, 4];

        private GameState gameState;

        #endregion

        #region [ アクセサ ]
        /// <summary>
        /// プレイヤー名を取得します
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public int PlayerNumber
        {
            get
            {
                return playerNumber;
            }

            set
            {
                playerNumber = value;
            }
        }

        public GhostAttribute[,] InitialPlacement
        {
            set
            {
                this.initialPlacement = value;
            }

            get
            {
                return this.initialPlacement;
            }
        }

        #endregion

        #region [コンストラクタ]
        /// <summary>
        /// コンストラクタ 名前表示がNonameになります
        /// </summary>
        public AbstractPlayer()
        {
            Name = "Noname";
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">表示したい名前</param>
        public AbstractPlayer(string name)
        {
            Name = name;
        }
        #endregion

        #region [パブリックメソッド]

        public void SetGameState(GameState gameState)
        {
            this.gameState = gameState.Clone();
        }

        /// <summary>
        /// 盤面の状態を取得する
        /// </summary>
        /// <returns></returns>
        public FieldObject[,] GetBoardState()
        {
            return gameState.BoardState;
        }

        /// <summary>
        /// 自身のFieldObjectにおける値を取得する
        /// </summary>
        /// <returns>
        /// Type:FieldObject
        /// P1 or P2
        /// </returns>
        public FieldObject GetMyPlayerID()
        {
            return gameState.currentPlayer;
        }

        /// <summary>
        /// 自身のGhostのリストを取得する
        /// </summary>
        /// <returns>
        /// Type:List<Ghost>
        /// </returns>
        public List<Ghost> GetMyGhostList()
        {
            if (gameState.currentPlayer.Equals(FieldObject.P1))
            {
                return gameState.P1ghostList;
            }
            else
            if (gameState.currentPlayer.Equals(FieldObject.P2))
            {
                return gameState.P2ghostList;
            }

            return null;
        }

        /// <summary>
        /// 指定した属性のGhostのリストを取得
        /// </summary>
        /// <param name="ga"></param>
        /// <returns></returns>
        public List<Ghost> GetMyGhostList(GhostAttribute ga)
        {
            List<Ghost> glist = new List<Ghost>();

            //getmyghostlistをループ
            foreach (Ghost g in GetMyGhostList())
            {
                if (g.Gt.Equals(ga))
                {
                    glist.Add(g);
                }
            }
            return glist;
        }

       /// <summary>
       /// 指定した属性のゴーストの数を取得する
       /// </summary>
       /// <param name="ga"></param>
       /// <returns></returns>
       public int GetGhostNum(FieldObject player, GhostAttribute ga)
        {
            int num = 0;
            List<Ghost> glist = null;

            //
            if(player.Equals(FieldObject.P1))
            {
                glist = gameState.P1ghostList;
            }
            else if(player.Equals(FieldObject.P2))
            {
                glist = gameState.P2ghostList;
            }

            foreach (Ghost g in glist)
            {
                if (g.Gt.Equals(ga))
                {
                    num++;
                }
            }
            return num;
        }

        /// <summary>
        /// 指定したプレイヤーのゴーストのポジションのリストを取得する
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public List<Position> GetGhostPositionList(FieldObject player)
        {
            List<Position> plist = new List<Position>();
            List<Ghost> glist = new List<Ghost>();
            if(player.Equals(FieldObject.P1))
            {
                glist = gameState.P1ghostList;
            }
            else if(player.Equals(FieldObject.P2))
            {
                glist = gameState.P2ghostList;
            }



                foreach (Ghost g in glist)
                {
                    plist.Add(g.P.Clone());
                }
            return plist;

        }

        public List<Ghost> GetCaputuredGhostList()
        {
            if (gameState.currentPlayer.Equals(FieldObject.P1))
            {
                Debug.WriteLine("Capture P1");
                return gameState.P1GhostGetList;
            }
            else
           if (gameState.currentPlayer.Equals(FieldObject.P2))
            {
                Debug.WriteLine("Capture P2");
                return gameState.P2GhostGetList;
            }

            return null;

        }

        /// <summary>
        /// ゴーストの初期配置を設定するメソッド
        /// </summary>
        /// <param name="init">ゴーストの2次元配列</param>
        public void SetInitialPlacement(GhostAttribute[,] init)
        {
            this.initialPlacement = init;
        }

        /// <summary>
        /// 移動させるゴーストと方向を決定するメソッド
        /// </summary>
        /// <returns>移動させるゴーストの位置と方向のメンバを持つクラス Move</returns>
        public abstract Move GetMove();


        #endregion
    }


}
