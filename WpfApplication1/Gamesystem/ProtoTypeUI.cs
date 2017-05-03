using System;
using System.Collections.Generic;
using System.Text;
using Prototype.GameSystem;

namespace Prototype.GameSystem
{
    public class ProtoTypeUI
    {
        #region [フィールド]

        #endregion

        /// <summary>
        /// ゲームの処理を書く
        /// </summary>
        /// <param name="gameManager"></param>
        #region [コンストラタ]
        public ProtoTypeUI(GameManager gameManager)
        {
            gameManager.ProcessGame();
            Console.ReadLine();
        }


        #endregion
    }
}
