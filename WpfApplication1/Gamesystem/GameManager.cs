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
                gamestate.M_Board[g.P.X, g.P.Y] = FieldObject.P1;
            }

            foreach (Ghost g in gamestate.P2ghostList)
            {
                gamestate.M_Board[g.P.X, g.P.Y] = FieldObject.P2;
            }
        }

        public void SetGhostPositionInBoard()
        {
            //P1リスト
            foreach (Ghost g in gamestate.P1ghostList)
            {
                //vb->b変換
                int x = g.P.X - 1;
                int y = g.P.Y;

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
                int x = g.P.X - 1;
                int y = g.P.Y;

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

            for (int i = 0; i < FinalTurn; i++)
            {

                gamestate.CurrentPlayer = FieldObject.P1;
                gamestate.NotCurrentPlayer = FieldObject.P2;
                NextTurn();
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
                NextTurn();
                ProcessTurn();
                if (VorDCheck())
                {
                    break;
                }
                //DisplayVirtualBoard();
                //DisplayBoard();
                //Console.CursorLeft = 0;
                //DisplayBoardState();



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
                g.P.X = g.P.X - 1;
                g.InitPos.X = g.InitPos.X - 1;
            }
            foreach (Ghost g in tmp.P2ghostList)
            {
                g.P.X = g.P.X - 1;
                g.InitPos.X = g.InitPos.X - 1;
            }
            foreach (Ghost g in tmp.P1GhostGetList)
            {
                g.P.X = g.P.X - 1;
                g.InitPos.X = g.InitPos.X - 1;
            }
            foreach (Ghost g in tmp.P2GhostGetList)
            {
                g.P.X = g.P.X - 1;
                g.InitPos.X = g.InitPos.X - 1;
            }


            //currentplayerが２Pならボードを反転
            if (gamestate.currentPlayer.Equals(FieldObject.P2))
            {
                tmp.BoardState = RotateClockwise(RotateClockwise(tmp.BoardState));

                //さらにghostlistの２つも反転
                foreach (Ghost g in tmp.P1ghostList)
                {
                    g.P.X = 5 - g.P.X;
                    g.P.Y = 5 - g.P.Y;
                    g.InitPos.X = 5 - g.InitPos.X;
                    g.InitPos.X = 5 - g.InitPos.X;
                    //Console.WriteLine(g.P.X + " " + g.P.Y);

                }
                //Console.WriteLine();
                foreach (Ghost g in tmp.P2ghostList)
                {
                    g.P.X = 5 - g.P.X;
                    g.P.Y = 5 - g.P.Y;
                    g.InitPos.X = 5 - g.InitPos.X;
                    g.InitPos.X = 5 - g.InitPos.X;
                    //Console.WriteLine(g.P.X + " " + g.P.Y);
                }
                foreach (Ghost g in tmp.P1GhostGetList)
                {
                    g.P.X = 5 - g.P.X;
                    g.P.Y = 5 - g.P.Y;
                    g.InitPos.X = 5 - g.InitPos.X;
                    g.InitPos.X = 5 - g.InitPos.X;
                    //Console.WriteLine(g.P.X + " " + g.P.Y);
                }
                foreach (Ghost g in tmp.P2GhostGetList)
                {
                    g.P.X = 5 - g.P.X;
                    g.P.Y = 5 - g.P.Y;
                    g.InitPos.X = 5 - g.InitPos.X;
                    g.InitPos.X = 5 - g.InitPos.X;
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

            Move m = new Move(new Position(5 - player2move.Pos.X, 5 - player2move.Pos.Y), gm);
            return m;
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
                        Debug.WriteLine(">>> {0} {1} {2}", gamestate.CurrentPlayerMove.Pos.X, gamestate.CurrentPlayerMove.Pos.Y, gamestate.CurrentPlayerMove.GhostM);
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

                if (gamestate.currentPlayer.Equals(FieldObject.P1))
                {
                    gamestate.CurrentPlayerMove = timeoutplayer1.GetMove();

                }
                else
                if (gamestate.currentPlayer.Equals(FieldObject.P2))
                {

                    gamestate.CurrentPlayerMove = ConvertPosition(timeoutplayer2.GetMove());
                }
                JudgeMove(gamestate.currentPlayerMove);
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
                Console.WriteLine("{0} Good Ghost Get Win!", gamestate.currentPlayer);
                gamestate.Winner = gamestate.currentPlayer;
                return true;
            }

            //Console.WriteLine("evil_{0} {1}", gamestate.GetGhostCount(gamestate.NotCurrentPlayer, GhostAttribute.evil), gamestate.NotCurrentPlayer);
            if (gamestate.GetGhostCount(gamestate.NotCurrentPlayer, GhostAttribute.evil).Equals(0))
            {
                Console.WriteLine("{0} Evil Ghost dosnt exis Win!", gamestate.NotCurrentPlayer);
                gamestate.Winner = gamestate.NotCurrentPlayer;
                return true;
            }

            //ゴーストの位置での終了条件

            if (gamestate.IsGhostAtExit(gamestate.currentPlayer))
            {
                Console.WriteLine("{0} Ghost Exits Win!", gamestate.currentPlayer);
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
                //TurnPlayer = FieldObject.P1;
                //gamestate.NotCurrentPlayer = FieldObject.P2;
            }
            else
            {
                Evil = Square.P2Evil;
                Good = Square.P2Good;
                //TurnPlayer = FieldObject.P2;
                //gamestate.NotCurrentPlayer = FieldObject.P1;
            }
            //Console.WriteLine("Turn{0} {1}", gamestate.TurnNum, gamestate.currentPlayer);

        }
        #endregion

        public void JudgeMove(Move m)
        {
            //書き換え用変数
            Move _m = new Move(new Position(m.Pos.X, m.Pos.Y), m.GhostM);

            if (gamestate.currentPlayer.Equals(FieldObject.P1))
            {
                //board変換
                _m.Pos.X = m.Pos.X + 1;
                _m.Pos.Y = m.Pos.Y;


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
                            gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.X + 1, _m.Pos.Y);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.X + 1, _m.Pos.Y));
                            if (_tmp >= 0)
                            {
                                gamestate.P1GhostGetList.Add(gamestate.P2ghostList[_tmp]);
                                gamestate.P2ghostList.RemoveAt(_tmp);
                            }
                            //Vb.M_Board[m.Pos.X + 1, m.Pos.Y] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                            break;

                        case GhostMove.Left:
                            gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.X, _m.Pos.Y - 1);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.X, _m.Pos.Y - 1));
                            if (_tmp >= 0)
                            {
                                gamestate.P1GhostGetList.Add(gamestate.P2ghostList[_tmp]);
                                gamestate.P2ghostList.RemoveAt(_tmp);
                            }
                            //Vb.M_Board[m.Pos.X, m.Pos.Y - 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                            break;

                        case GhostMove.Right:
                            gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.X, _m.Pos.Y + 1);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.X, _m.Pos.Y + 1));
                            if (_tmp >= 0)
                            {
                                gamestate.P1GhostGetList.Add(gamestate.P2ghostList[_tmp]);
                                gamestate.P2ghostList.RemoveAt(_tmp);
                            }
                            //Vb.M_Board[m.Pos.X, m.Pos.Y + 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                            break;
                        case GhostMove.Up:
                            gamestate.P1ghostList[gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, _m.Pos)].P = new Position(_m.Pos.X - 1, _m.Pos.Y);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, new Position(_m.Pos.X - 1, _m.Pos.Y));
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
                _m.Pos.X = m.Pos.X + 1;
                _m.Pos.Y = m.Pos.Y;

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
                            gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.X + 1, _m.Pos.Y);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.X + 1, _m.Pos.Y));
                            if (_tmp >= 0)
                            {
                                gamestate.P2GhostGetList.Add(gamestate.P1ghostList[_tmp]);
                                gamestate.P1ghostList.RemoveAt(_tmp);

                            }
                            //Vb.M_Board[m.Pos.X + 1, m.Pos.Y] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                            break;

                        case GhostMove.Left:
                            gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.X, _m.Pos.Y - 1);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.X, _m.Pos.Y - 1));
                            if (_tmp >= 0)
                            {
                                gamestate.P2GhostGetList.Add(gamestate.P1ghostList[_tmp]);
                                gamestate.P1ghostList.RemoveAt(_tmp);
                            }
                            //Vb.M_Board[m.Pos.X, m.Pos.Y - 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                            break;

                        case GhostMove.Right:
                            gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.X, _m.Pos.Y + 1);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.X, _m.Pos.Y + 1));
                            if (_tmp >= 0)
                            {
                                gamestate.P2GhostGetList.Add(gamestate.P1ghostList[_tmp]);
                                gamestate.P1ghostList.RemoveAt(_tmp);
                            }
                            //Vb.M_Board[m.Pos.X, m.Pos.Y + 1] = Vb.M_Board[m.Pos.X, m.Pos.Y];
                            break;
                        case GhostMove.Up:
                            gamestate.P2ghostList[gamestate.GetSamePosGhostIndex(gamestate.P2ghostList, _m.Pos)].P = new Position(_m.Pos.X - 1, _m.Pos.Y);
                            _tmp = gamestate.GetSamePosGhostIndex(gamestate.P1ghostList, new Position(_m.Pos.X - 1, _m.Pos.Y));
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


        public int GetSamePosGhostIndex(List<Ghost> glist, Position p)
        {
            int index = -1;
            foreach (Ghost g in glist)
            {
                if (g.P.X == p.X && g.P.Y == p.Y)
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
                Debug.WriteLine("P1");
                foreach (Ghost g in gamestate.P1ghostList)
                {
                    Debug.WriteLine("roop");
                    if (g.P.X == 0 && g.P.Y == 0 || g.P.X == 0 && g.P.Y == 5)
                    {
                        Debug.WriteLine("0005");
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
                    if (g.P.X == 7 && g.P.Y == 0 || g.P.X == 7 && g.P.Y == 5)
                    {
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
                    if (GhostIsInBoard(new Position(m.Pos.X + 1, m.Pos.Y)) || IsExit(m.Pos))
                    {
                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.X + 1, m.Pos.Y)))
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
                    if (GhostIsInBoard(new Position(m.Pos.X, m.Pos.Y - 1)) || IsExit(m.Pos))
                    {
                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.X, m.Pos.Y - 1)))
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
                    if (GhostIsInBoard(new Position(m.Pos.X, m.Pos.Y + 1)) || IsExit(m.Pos))
                    {
                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.X, m.Pos.Y + 1)))
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
                    if (GhostIsInBoard(new Position(m.Pos.X - 1, m.Pos.Y)) || IsExit(m.Pos))
                    {
                        //移動先に自分のゴーストがいないか
                        if (!GhostExists(new Position(m.Pos.X - 1, m.Pos.Y)))
                        {
                            //Debug.WriteLine("Can move");
                            return true;
                        }
                    }

                }

            }

            Debug.WriteLine("already exist:{0} {1}", m.Pos.X, m.Pos.Y);
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
                if (gamestate.M_Board[p.X, p.Y].Equals(gamestate.currentPlayer))
                {
                    return true;
                }
            }
            //Debug.WriteLine("Ghost not exist:{0} {1}", p.X, p.Y);
            return false;
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
            if ((0 < p.X && p.X < 7) && (0 <= p.Y && p.Y <= 5))
            {
                return true;
            }

            if (gamestate.currentPlayer.Equals(FieldObject.P1))
            {


                if (p.X == 0 && p.Y == 0 || p.X == 0 && p.Y == 5)
                {
                    return true;
                }

            }
            else if (gamestate.currentPlayer.Equals(FieldObject.P2))
            {
                if (p.X == 7 && p.Y == 0 || p.X == 7 && p.Y == 5)
                {
                    return true;
                }


            }

            Debug.WriteLine("out of Board:{0} {1}", p.X, p.Y);
            return false;
        }
        public Boolean IsExit(Position p)
        {
            ////出口にあるとき
            if ((p.X.Equals(0) && p.Y.Equals(0)) || (p.X.Equals(0) && p.Y.Equals(5)) || (p.X.Equals(7) && p.Y.Equals(5)) || (p.X.Equals(7) && p.Y.Equals(0)))
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
