using System;
using System.Collections.Generic;
using System.Text;
using Geister.GameInformation;
using Geister.GameSystem;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Geister.GameSystem
{
    public class GameManager
    {

        private List<AbstractPlayer> playerList = new List<AbstractPlayer>();
        private AbstractPlayer timeoutplayer1 = new DebugPlayer();
        private AbstractPlayer timeoutplayer2 = new DebugPlayer();

        #region [フィールド]
        public GameState gamestate
        {
            get;
            set;
        }

        public AbstractPlayer P1
        {
            get;
            set;
        }

        public AbstractPlayer P2
        {
            get;
            set;
        }

        public int FinalTurn
        {
            get;
            set;
        }



        public Square Good
        {
            get;
            set;
        }

        public Square Evil
        {
            get;
            set;
        }

        public int P1_GoodGhostNum
        {
            get;
            set;
        }

        public int P1_EvilGhostNum
        {
            get;
            set;
        }

        public int P2_GoodGhostNum
        {
            get;
            set;
        }

        public int P2_EvilGhostNum
        {
            get;
            set;
        }

        private int processFPS;
        #endregion


        #region [コンストラクタ]
        public GameManager(AbstractPlayer p1, AbstractPlayer p2, int finalturn, int fps)
        {
            P1 = p1;
            P2 = p2;
            FinalTurn = finalturn;

            //gamestate.NotCurrentPlayer = FieldObject.P2;

            playerList.Add(p1);
            playerList.Add(p2);

            Evil = Square.P1Evil;
            Good = Square.P1Good;

            P1_EvilGhostNum = 4;
            P1_GoodGhostNum = 4;

            P2_EvilGhostNum = 4;
            P2_GoodGhostNum = 4;

            this.processFPS = fps;

            gamestate = new GameState(P1.InitialPlacement, P2.InitialPlacement);
        }
        #endregion

        #region [メソッド]


        public void SetGhostPostionInVirtual()
        {
            foreach (Ghost g in gamestate.P1ghostList)
            {
                gamestate.M_Board[g.P.Row, g.P.Col] = FieldObject.P1;
            }

            foreach (Ghost g in gamestate.P2ghostList)
            {
                gamestate.M_Board[g.P.Row, g.P.Col] = FieldObject.P2;
            }
        }

        public void SetGhostPositionInBoard()
        {
            //P1リスト
            foreach (Ghost g in gamestate.P1ghostList)
            {
                //vb->b変換
                int x = g.P.Row - 1;
                int y = g.P.Col;

                //例外処理
                if (x < 0 || y < 0 || x > 6 || y > 6)
                {

                }
                else
                {
                    if (g.Gt.Equals(GhostAttribute.evil))
                    {
                        gamestate.Board[x, y] = GhostType.P1evil;
                    }
                    else if (g.Gt.Equals(GhostAttribute.good))
                    {
                        gamestate.Board[x, y] = GhostType.P1good;
                    }
                    else
                    {
                        gamestate.Board[x, y] = GhostType.Blank;
                    }

                }
            }

            foreach (Ghost g in gamestate.P2ghostList)
            {
                //vb->b変換
                int x = g.P.Row - 1;
                int y = g.P.Col;

                //例外処理
                if (x < 0 || y < 0 || x > 6 || y > 6)
                {

                }
                else
                {
                    if (g.Gt.Equals(GhostAttribute.evil))
                    {
                        gamestate.Board[x, y] = GhostType.P2evil;
                    }
                    else if (g.Gt.Equals(GhostAttribute.good))
                    {
                        gamestate.Board[x, y] = GhostType.P2good;
                    }
                    else
                    {
                        gamestate.Board[x, y] = GhostType.Blank;
                    }

                }
            }

        }



        public void ResetGhostPostion()
        {
            for (int i = 0; i < gamestate.M_Board.GetLength(0); i++)
            {
                for (int j = 0; j < gamestate.M_Board.GetLength(1); j++)
                {
                    gamestate.M_Board[i, j] = FieldObject.blank;
                }
            }
        }

        public void ResetGhostPositionInBoard()
        {
            for (int i = 0; i < gamestate.Board.GetLength(0); i++)
            {
                for (int j = 0; j < gamestate.Board.GetLength(1); j++)
                {
                    gamestate.Board[i, j] = GhostType.Blank;
                }
            }

        }

        #endregion

        #region [ゲーム進行メソッド]
        public void ProcessGame()
        {
            //のちに消す
            //DisplayVirtualBoard();
            //DisplayBoard();
            //

            System.Threading.Thread.Sleep(3000);


            int count = 0;
            for (int i = 0; i < FinalTurn; i++)
            {

                gamestate.CurrentPlayer = FieldObject.P1;
                gamestate.NotCurrentPlayer = FieldObject.P2;
                //NextTurn();
                Evil = Square.P1Evil;
                Good = Square.P1Good;
                ProcessTurn();
                if (VorDCheck())
                {
                    break;
                }

                //のちに消す
                //DisplayVirtualBoard();
                //DisplayBoard();
                //Console.CursorLeft = 0;
                //DisplayBoardState();
                //

                gamestate.CurrentPlayer = FieldObject.P2;
                gamestate.NotCurrentPlayer = FieldObject.P1;
                //NextTurn();
                Evil = Square.P2Evil;
                Good = Square.P2Good;
                ProcessTurn();
                if (VorDCheck())
                {
                    break;
                }
                gamestate.TurnNum++;
                //DisplayVirtualBoard();
                //DisplayBoard();
                //Console.CursorLeft = 0;
                //DisplayBoardState();

                count++;

            }

            //forループ後も行われる．
            //出口での勝利はremoveされるため
            if (count == FinalTurn)
            {
                gamestate.Flag = true;
                OverTurn();
            }
            //勝敗を決める


        }

        private void OverTurn()
        {
            int p1score = 0;
            int p2score = 0;
            int p1gnum = 0;
            int p1enum = 0;
            int p2gnum = 0;
            int p2enum = 0;

            foreach (Ghost g in gamestate.P1ghostList)
            {
                if (g.Gt == GhostAttribute.good)
                {
                    p1gnum++;
                }
                else if (g.Gt == GhostAttribute.evil)
                {
                    p1enum++;
                }
            }

            foreach (Ghost g in gamestate.P2ghostList)
            {
                if (g.Gt == GhostAttribute.good)
                {
                    p2gnum++;
                }
                else if (g.Gt == GhostAttribute.evil)
                {
                    p2enum++;
                }
            }

            p1score = 2 * (p1gnum - p2gnum) - (p1enum - p2enum);
            p2score = 2 * (p2gnum - p1gnum) - (p2enum - p1enum);

            if (p1score > p2score)
            {
                gamestate.Winner = FieldObject.P1;
            }
            else
                if (p1score < p2score)
            {
                gamestate.Winner = FieldObject.P2;
            }
            else
            {
                gamestate.Winner = FieldObject.blank;
            }
        }

        private void ProcessTurn()
        {


            if (gamestate.currentPlayer.Equals(FieldObject.P1))
            {
                P1.SetGameState(ConvertGameState(gamestate));

                timeoutplayer1.SetGameState(ConvertGameState(gamestate));
            }
            else
             if (gamestate.currentPlayer.Equals(FieldObject.P2))
            {
                //gamestateを反転して渡す
                P2.SetGameState(ConvertGameState(gamestate));
                timeoutplayer2.SetGameState(ConvertGameState(gamestate));
            }

            MovePlayer();
        }

        //与えるGameStateを変換する
        public GameState ConvertGameState(GameState gs)
        {
            //複製を作成
            GameState tmp = gs.Clone();
            //変換するプロパティ
            //ghostlistの初期位置と現在位置 (8,6になっているので)
            foreach (Ghost g in tmp.P1ghostList)
            {
                g.P.Row = g.P.Row - 1;
                g.InitPos.Row = g.InitPos.Row - 1;
            }
            foreach (Ghost g in tmp.P2ghostList)
            {
                g.P.Row = g.P.Row - 1;
                g.InitPos.Row = g.InitPos.Row - 1;
            }
            foreach (Ghost g in tmp.P1GhostGetList)
            {
                g.P.Row = g.P.Row - 1;
                g.InitPos.Row = g.InitPos.Row - 1;
            }
            foreach (Ghost g in tmp.P2GhostGetList)
            {
                g.P.Row = g.P.Row - 1;
                g.InitPos.Row = g.InitPos.Row - 1;
            }


            //currentplayerが２Pならボードを反転
            if (gamestate.currentPlayer.Equals(FieldObject.P2))
            {
                tmp.BoardState = RotateClockwise(RotateClockwise(tmp.BoardState));

                //さらにghostlistの２つも反転
                foreach (Ghost g in tmp.P1ghostList)
                {
                    g.P.Row = 5 - g.P.Row;
                    g.P.Col = 5 - g.P.Col;
                    g.InitPos.Row = 5 - g.InitPos.Row;
                    g.InitPos.Col = 5 - g.InitPos.Col;
                    //Console.WriteLine(g.P.X + " " + g.P.Y);

                }
                //Console.WriteLine();
                foreach (Ghost g in tmp.P2ghostList)
                {
                    g.P.Row = 5 - g.P.Row;
                    g.P.Col = 5 - g.P.Col;
                    g.InitPos.Row = 5 - g.InitPos.Row;
                    g.InitPos.Col = 5 - g.InitPos.Col;
                    //Console.WriteLine(g.P.X + " " + g.P.Y);
                }
                foreach (Ghost g in tmp.P1GhostGetList)
                {
                    g.P.Row = 5 - g.P.Row;
                    g.P.Col = 5 - g.P.Col;
                    g.InitPos.Row = 5 - g.InitPos.Row;
                    g.InitPos.Col = 5 - g.InitPos.Col;
                    //Console.WriteLine(g.P.X + " " + g.P.Y);
                }
                foreach (Ghost g in tmp.P2GhostGetList)
                {
                    g.P.Row = 5 - g.P.Row;
                    g.P.Col = 5 - g.P.Col;
                    g.InitPos.Row = 5 - g.InitPos.Row;
                    g.InitPos.Col = 5 - g.InitPos.Col;
                    //Console.WriteLine(g.P.X + " " + g.P.Y);
                }

            }
            return tmp;
        }

        FieldObject[,] RotateClockwise(FieldObject[,] g)
        {
            // 引数の2次元配列 g を時計回りに回転させたものを返す
            int rows = g.GetLength(0);
            int cols = g.GetLength(1);
            var t = new FieldObject[cols, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    t[j, rows - i - 1] = g[i, j];
                }
            }
            return t;
        }

        /// <summary>
        /// Gets the player move.
        /// GamestateにplayerMoveを代入する
        /// </summary>
        private void GetPlayerMove(CancellationToken cancelToken)
        {
            try
            {
                //
                // もし、外部でキャンセルされていた場合
                // このメソッドはOperationCanceledExceptionを発生させる。
                //
                cancelToken.ThrowIfCancellationRequested();

                if (gamestate.currentPlayer.Equals(FieldObject.P1))
                {
                    gamestate.CurrentPlayerMove = P1.GetMove();

                }
                else
                if (gamestate.currentPlayer.Equals(FieldObject.P2))
                {
                    gamestate.CurrentPlayerMove = ConvertPosition(P2.GetMove());
                }
            }
            catch (OperationCanceledException ex)
            {
                //
                // キャンセルされた.
                //
                Debug.WriteLine(">>> {0}", ex.Message);
            }

        }

        /// <summary>
        /// 2P のポジションを反転する
        /// Converts the position.
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name="player2move">Player2move.</param>
        private Move ConvertPosition(Move player2move)
        {
            if (player2move != null)
            {
                GhostMove gm = GhostMove.Down;
                if (player2move.GhostM == GhostMove.Down)
                {
                    gm = GhostMove.Up;
                }
                else
                if (player2move.GhostM == GhostMove.Left)
                {
                    gm = GhostMove.Right;
                }
                else
                if (player2move.GhostM == GhostMove.Right)
                {
                    gm = GhostMove.Left;
                }
                else
                if (player2move.GhostM == GhostMove.Up)
                {
                    gm = GhostMove.Down;
                }

                Move m = new Move(new Position(5 - player2move.Pos.Row, 5 - player2move.Pos.Col), gm);
                return m;
            }
            return null;
        }


        private void MovePlayer()
        {

            Task task = null;
            var cts = new CancellationTokenSource();
            Boolean isTaskRunning = false;
            Boolean isTaskTimeOut;

            TimeSpan timeSpan;
            DateTime endTime;
            DateTime startTime = DateTime.Now;

            while (true)
            {
                if (!isTaskRunning)
                {
                    isTaskRunning = true;

                    Debug.WriteLine(">>> Task start");
                    task = new Task(() => GetPlayerMove(cts.Token));
                    task.Start();

                }
                else
                {

                    endTime = DateTime.Now;
                    timeSpan = endTime - startTime;
                    if (timeSpan.TotalMilliseconds > gamestate.ThinkTime)
                    {
                        //スレッドを強制終了させる
                        cts.Cancel();
                        Debug.WriteLine("Task Cancel {0}", timeSpan.TotalMilliseconds);

                        isTaskTimeOut = true;
                        break;
                    }

                    // スレッドが終了した時
                    //Debug.WriteLine(task.Status.ToString());
                    if (task.IsCanceled || task.IsCompleted)
                    {
                        Debug.WriteLine(">>> Task end");
                        Debug.WriteLine(">>> {0}Turn {1}", gamestate.TurnNum, gamestate.currentPlayer);
                        if (gamestate.CurrentPlayerMove != null)
                        {
                            Debug.WriteLine(">>> {0} {1} {2}", gamestate.CurrentPlayerMove.Pos.Row, gamestate.CurrentPlayerMove.Pos.Col, gamestate.CurrentPlayerMove.GhostM);
                        }
                        isTaskTimeOut = false;
                        break;
                    }



                }
            }

            if (!isTaskTimeOut)
            {
                JudgeMove(gamestate.currentPlayerMove);
                if (timeSpan.TotalMilliseconds + processFPS < gamestate.ThinkTime)
                    //Debug.WriteLine("{0} < {1}", timeSpan.TotalMilliseconds + processFPS, gamestate.ThinkTime);
                    Thread.Sleep(processFPS);
            }

            else
            {
                ///タイムアウト用のプログラムを書く

                //if (gamestate.currentPlayer.Equals(FieldObject.P1))
                //{
                //    gamestate.CurrentPlayerMove = timeoutplayer1.GetMove();

                //}
                //else
                //if (gamestate.currentPlayer.Equals(FieldObject.P2))
                //{

                //    gamestate.CurrentPlayerMove = ConvertPosition(timeoutplayer2.GetMove());
                //}
                //JudgeMove(gamestate.currentPlayerMove);
                Debug.WriteLine(">>>Time OVER");
            }
        }

        public Boolean VorDCheck()
        {

            //ゲームの終了条件を確認
            //ゴースト数での終了条件
            //Console.WriteLine("good_{0} {1}", gamestate.GetGhostCount(gamestate.NotCurrentPlayer, GhostAttribute.good), gamestate.NotCurrentPlayer);
            if (gamestate.GetGhostCount(gamestate.NotCurrentPlayer, GhostAttribute.good).Equals(0))
            {
                //Console.WriteLine("{0} {1}", gamestate.NotCurrentPlayer, gamestate.GetGhostCount(gamestate.NotCurrentPlayer, GhostAttribute.good));
                Console.WriteLine("{0} Win! (Capture AllGood Ghosts)", gamestate.currentPlayer);
                gamestate.Winner = gamestate.currentPlayer;
                return true;
            }

            //Console.WriteLine("evil_{0} {1}", gamestate.GetGhostCount(gamestate.NotCurrentPlayer, GhostAttribute.evil), gamestate.NotCurrentPlayer);
            if (gamestate.GetGhostCount(gamestate.NotCurrentPlayer, GhostAttribute.evil).Equals(0))
            {
                Console.WriteLine("{0} Win! (Evil Ghosts doesn't exist)", gamestate.NotCurrentPlayer);
                gamestate.Winner = gamestate.NotCurrentPlayer;
                return true;
            }

            //ゴーストの位置での終了条件

            if (gamestate.IsGhostAtExit(gamestate.currentPlayer))
            {
                Console.WriteLine("{0} Win! (Good Ghost Exits)", gamestate.currentPlayer);
                gamestate.Winner = gamestate.currentPlayer;
                return true;
            }
            return false;
        }

        public void NextTurn()
        {
            gamestate.TurnNum++;
            if ((gamestate.TurnNum % 2).Equals(1))
            {
                Evil = Square.P1Evil;
                Good = Square.P1Good;
            }
            else
            {
                Evil = Square.P2Evil;
                Good = Square.P2Good;
            }

            //Console.WriteLine("Turn{0} {1}", gamestate.TurnNum, gamestate.currentPlayer);

        }
        #endregion

        public void JudgeMove(Move m)
        {
            //書き換え用変数
            if (m != null)
            {
                Move _m = new Move(new Position(m.Pos.Row, m.Pos.Col), m.GhostM);

                if (gamestate.currentPlayer.Equals(FieldObject.P1))
                {
                    //board変換
                    _m.Pos.Row = m.Pos.Row + 1;
                    _m.Pos.Col = m.Pos.Col;


                    //移動可能性
                    if (IsGhostMovable(_m))
                    {

                        //移動(ゴーストリストのpositionを書き換える)
                        //移動先に相手のゴーストがいるときはそのゴーストをリストから消す
                        int _tmp;
                        switch (_m.GhostM)
                        {

                            case GhostMove.Down:
                                //xとｙがm.posと同じリストの中の要素のインデックスを一つ取得する
                                gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.Row + 1, _m.Pos.Col);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.Row + 1, _m.Pos.Col));
                                if (_tmp >= 0)
                                {
                                    gamestate.P1GhostGetList.Add(gamestate.P2ghostList[_tmp]);
                                    gamestate.P2ghostList.RemoveAt(_tmp);
                                }
                                //Vb.M_Board[m.Pos.X + 1, m.Pos.Y] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;

                            case GhostMove.Left:
                                gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.Row, _m.Pos.Col - 1);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.Row, _m.Pos.Col - 1));
                                if (_tmp >= 0)
                                {
                                    gamestate.P1GhostGetList.Add(gamestate.P2ghostList[_tmp]);
                                    gamestate.P2ghostList.RemoveAt(_tmp);
                                }
                                //Vb.M_Board[m.Pos.X, m.Pos.Y - 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;

                            case GhostMove.Right:
                                gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.Row, _m.Pos.Col + 1);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.Row, _m.Pos.Col + 1));
                                if (_tmp >= 0)
                                {
                                    gamestate.P1GhostGetList.Add(gamestate.P2ghostList[_tmp]);
                                    gamestate.P2ghostList.RemoveAt(_tmp);
                                }
                                //Vb.M_Board[m.Pos.X, m.Pos.Y + 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;
                            case GhostMove.Up:
                                gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.Row - 1, _m.Pos.Col);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.Row - 1, _m.Pos.Col));
                                if (_tmp >= 0)
                                {
                                    gamestate.P1GhostGetList.Add(gamestate.P2ghostList[_tmp]);
                                    gamestate.P2ghostList.RemoveAt(_tmp);
                                }
                                //Vb.M_Board[m.Pos.X - 1, m.Pos.Y] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;
                            default:
                                break;
                        }

                    }

                }
                else
                {
                    //Borad変換
                    _m.Pos.Row = m.Pos.Row + 1;
                    _m.Pos.Col = m.Pos.Col;

                    //移動可能性
                    if (IsGhostMovable(_m))
                    {

                        //移動(ゴーストリストのpositionを書き換える)
                        //移動先に相手のゴーストがいるときはそのゴーストをリストから消す
                        int _tmp;
                        switch (_m.GhostM)
                        {

                            case GhostMove.Down:
                                //xとｙがm.posと同じリストの中の要素のインデックスを一つ取得する
                                gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.Row + 1, _m.Pos.Col);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.Row + 1, _m.Pos.Col));
                                if (_tmp >= 0)
                                {
                                    gamestate.P2GhostGetList.Add(gamestate.P1ghostList[_tmp]);
                                    gamestate.P1ghostList.RemoveAt(_tmp);

                                }
                                //Vb.M_Board[m.Pos.X + 1, m.Pos.Y] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;

                            case GhostMove.Left:
                                gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.Row, _m.Pos.Col - 1);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.Row, _m.Pos.Col - 1));
                                if (_tmp >= 0)
                                {
                                    gamestate.P2GhostGetList.Add(gamestate.P1ghostList[_tmp]);
                                    gamestate.P1ghostList.RemoveAt(_tmp);
                                }
                                //Vb.M_Board[m.Pos.X, m.Pos.Y - 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;

                            case GhostMove.Right:
                                gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.Row, _m.Pos.Col + 1);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.Row, _m.Pos.Col + 1));
                                if (_tmp >= 0)
                                {
                                    gamestate.P2GhostGetList.Add(gamestate.P1ghostList[_tmp]);
                                    gamestate.P1ghostList.RemoveAt(_tmp);
                                }
                                //Vb.M_Board[m.Pos.X, m.Pos.Y + 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;
                            case GhostMove.Up:
                                gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.Row - 1, _m.Pos.Col);
                                _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.Row - 1, _m.Pos.Col));
                                if (_tmp >= 0)
                                {
                                    gamestate.P2GhostGetList.Add(gamestate.P1ghostList[_tmp]);
                                    gamestate.P1ghostList.RemoveAt(_tmp);
                                }
                                //Vb.M_Board[m.Pos.X - 1, m.Pos.Y] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                                break;
                            default:
                                break;
                        }
                    }

                }

                gamestate.ResetGhostPostion();
                gamestate.ResetGhostPositionInBoard();
                gamestate.SetGhostPostionInVirtual();
                gamestate.SetGhostPositionInBoard();
                gamestate.SetBoardState();
            }
        }


        public int GetSamePosGhostIndex(List<Ghost> glist, Position p)
        {
            int index = -1;
            foreach (Ghost g in glist)
            {
                if (g.P.Row == p.Row && g.P.Col == p.Col)
                {
                    index = glist.IndexOf(g);
                    break;
                }
            }
            return index;

        }

        public int GetGhostCount(FieldObject o, GhostAttribute gt)
        {
            int count = 0;

            if (o.Equals(FieldObject.P1))
            {
                foreach (Ghost g in gamestate.P1ghostList)
                {
                    if (g.Gt.Equals(gt))
                    {
                        count++;
                    }
                }
            }
            else if (o.Equals(FieldObject.P2))
            {
                foreach (Ghost g in gamestate.P2ghostList)
                {
                    if (g.Gt.Equals(gt))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public Boolean IsGhostAtExit(FieldObject o)
        {
            //Ghostlistを検索し出口にいないかチェック
            if (o.Equals(FieldObject.P1))
            {
                foreach (Ghost g in gamestate.P1ghostList)
                {
                    if (g.P.Row == 0 && g.P.Col == 0 || g.P.Row == 0 && g.P.Col == 5)
                    {
                        gamestate.P2GhostGetList.Add(g);
                        gamestate.P1ghostList.Remove(g);

                        if (g.Gt.Equals((GhostAttribute.good)))
                        {
                            return true;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
            else if (o.Equals(FieldObject.P2))
            {
                foreach (Ghost g in gamestate.P2ghostList)
                {
                    if (g.P.Row == 7 && g.P.Col == 0 || g.P.Row == 7 && g.P.Col == 5)
                    {
                        gamestate.P1GhostGetList.Add(g);
                        gamestate.P2ghostList.Remove(g);

                        if (g.Gt.Equals((GhostAttribute.good)))
                        {
                            return true;
                        }
                        else
                        {
                            break;
                        }
                    }

                }

            }

            return false;
        }

        /// <summary>
        /// ゴーストが移動できるか判定
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Boolean IsGhostMovable(Move m)
        {
            ///ゴーストがいるか判定
            if (GhostExists(m.Pos))
            {
                //下に移動するとき
                if (m.GhostM.Equals(GhostMove.Down))
                {
                    //移動先が盤面内か
                    if (GhostIsInBoard(new Position(m.Pos.Row + 1, m.Pos.Col)) || IsExit(m.Pos))
                    {
                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.Row + 1, m.Pos.Col)))
                        {
                            //Debug.WriteLine("Can move");
                            return true;
                        }
                    }

                }

                //左に移動するとき
                if (m.GhostM.Equals(GhostMove.Left))
                {
                    //移動先が盤面内か
                    if (GhostIsInBoard(new Position(m.Pos.Row, m.Pos.Col - 1)) || IsExit(m.Pos))
                    {
                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.Row, m.Pos.Col - 1)))
                        {
                            //Debug.WriteLine("Can move");
                            return true;
                        }
                    }

                }

                //右に移動するとき
                if (m.GhostM.Equals(GhostMove.Right))
                {
                    //移動先が盤面内か
                    if (GhostIsInBoard(new Position(m.Pos.Row, m.Pos.Col + 1)) || IsExit(m.Pos))
                    {
                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.Row, m.Pos.Col + 1)))
                        {
                            //Debug.WriteLine("Can move");
                            return true;
                        }
                    }

                }

                //上に移動するとき
                if (m.GhostM.Equals(GhostMove.Up))
                {
                    //移動先が盤面内か
                    if (GhostIsInBoard(new Position(m.Pos.Row - 1, m.Pos.Col)) || IsExit(m.Pos))
                    {
                        //行き先が出口の時
                        if (IsExit(new Position(m.Pos.Row - 1, m.Pos.Col)))
                        {
                            //さらに自分のゴーストが良いやつであるか
                            if (CheckGhostAttribute(m.Pos))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }

                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.Row - 1, m.Pos.Col)))
                        {
                            return true;
                        }
                    }
                }

            }

            Debug.WriteLine("already exist:{0} {1}", m.Pos.Row, m.Pos.Col);
            return false;
        }

        /// <summary>
        /// 引数のpositionにターンプレイヤーのゴーストがいるか判定
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Boolean GhostExists(Position p)
        {
            //pが不正な値でないか判定
            if (GhostIsInBoard(p))
            {
                //位置pにゴーストが存在するか判定
                if (gamestate.M_Board[p.Row, p.Col].Equals(gamestate.currentPlayer))
                {
                    return true;
                }
            }
            //Debug.WriteLine("Ghost not exist:{0} {1}", p.X, p.Y);
            return false;
        }

        public Boolean CheckGhostAttribute(Position p)
        {
            Boolean result = false;
            //pが不正な値でないか判定
            if (GhostIsInBoard(p))
            {
                //位置pにゴーストが存在するか判定
                if (gamestate.M_Board[p.Row, p.Col].Equals(gamestate.currentPlayer))
                {
                    //カレントプレイヤーのゴーストリスト取得
                    List<Ghost> tmp = null;
                    if (gamestate.currentPlayer == FieldObject.P1)
                    {
                        tmp = gamestate.P1ghostList;
                    }
                    else if (gamestate.currentPlayer == FieldObject.P2)
                    {
                        tmp = gamestate.P2ghostList;
                    }

                    //一致するゴーストを見つける
                    foreach (Ghost g in tmp)
                    {
                        if (g.P.Row == p.Row && g.P.Col == p.Col)
                        {
                            //属性が正しいかチェック
                            if (g.Gt == GhostAttribute.good)
                            {
                                result = true;
                                break;
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// pが不正な値でないか判定
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Boolean GhostIsInBoard(Position p)
        {

            //盤面内にあるとき
            //test
            if ((0 < p.Row && p.Row < 7) && (0 <= p.Col && p.Col <= 5))
            {
                return true;
            }

            if (gamestate.currentPlayer.Equals(FieldObject.P1))
            {


                if (p.Row == 0 && p.Col == 0 || p.Row == 0 && p.Col == 5)
                {
                    return true;
                }

            }
            else if (gamestate.currentPlayer.Equals(FieldObject.P2))
            {
                if (p.Row == 7 && p.Col == 0 || p.Row == 7 && p.Col == 5)
                {
                    return true;
                }


            }

            Debug.WriteLine("out of Board:{0} {1}", p.Row, p.Col);
            return false;
        }
        public Boolean IsExit(Position p)
        {
            ////出口にあるとき
            if ((p.Row.Equals(0) && p.Col.Equals(0)) || (p.Row.Equals(0) && p.Col.Equals(5)) || (p.Row.Equals(7) && p.Col.Equals(5)) || (p.Row.Equals(7) && p.Col.Equals(0)))
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// コンソール画面にボード情報を出力
        /// </summary>
        public void DisplayVirtualBoard()
        {
            for (int i = 0; i < gamestate.M_Board.GetLength(1); i++)
            {
                Console.Write("{0,11} ", i);
            }
            Console.WriteLine();
            for (int i = 0; i < gamestate.M_Board.GetLength(0); i++)
            {
                Console.Write("{0} ", i);
                for (int j = 0; j < gamestate.M_Board.GetLength(1); j++)
                {
                    Console.Write("{0,11} ", gamestate.M_Board[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void DisplayBoard()
        {
            for (int i = 0; i < gamestate.Board.GetLength(1); i++)
            {
                Console.Write("{0,11} ", i);
            }
            Console.WriteLine();
            for (int i = 0; i < gamestate.Board.GetLength(0); i++)
            {
                Console.Write("{0} ", i);
                for (int j = 0; j < gamestate.Board.GetLength(1); j++)
                {
                    Console.Write("{0,11} ", gamestate.Board[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void DisplayBoardState()
        {
            for (int i = 0; i < gamestate.BoardState.GetLength(1); i++)
            {
                Console.Write("{0,11} ", i);
            }
            Console.WriteLine();
            for (int i = 0; i < gamestate.BoardState.GetLength(0); i++)
            {
                Console.Write("{0} ", i);
                for (int j = 0; j < gamestate.BoardState.GetLength(1); j++)
                {
                    Console.Write("{0,11} ", gamestate.BoardState[i, j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// テスト用Move入力関数(コンソールから)
        /// </summary>
        public Move TmpMove()
        {
            for (int i = 0; i < gamestate.BoardState.GetLength(0); i++)
            {
                for (int j = 0; j < gamestate.BoardState.GetLength(1); j++)
                {
                    Console.Write("{0}  ", gamestate.BoardState[i, j]);
                }
                Console.WriteLine();

            }


            GhostMove gm = new GhostMove();

            Console.WriteLine("x");
            int x = int.Parse(Console.ReadLine());
            Console.WriteLine("y");

            int y = int.Parse(Console.ReadLine());
            Console.WriteLine("gm");
            string gmString = Console.ReadLine();

            switch (gmString)
            {
                case "u":
                    gm = GhostMove.Up;
                    break;

                case "d":
                    gm = GhostMove.Down;
                    break;

                case "l":
                    gm = GhostMove.Left;
                    break;

                case "r":
                    gm = GhostMove.Right;
                    break;
                default:
                    gm = GhostMove.Up;
                    break;
            }

            return new Move(new Position(x, y), gm);
        }



    }
}
