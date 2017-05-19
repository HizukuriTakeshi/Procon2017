using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Timers;
using System.Windows.Threading;
using Geister.GameInformation;
using Geister.GameSystem;

// hoppin参考
/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
*/

namespace Geister
{
    /// <summary>
    /// GeisterUI.xaml の相互作用ロジック
    /// </summary>
    public partial class GeisterUI : Window
    {
        int modeflag = new int();

        // gameManager とりあえずコメントアウト
        private GameManager gameManager;
        private delegate void GameProcessDeligate();

        // 時間のカウント用？
        private int count;

        DispatcherTimer dispatcherTimer;

        /*
        public GeisterUI()
        {
            // gameが始まるとここにやってくる？

            modeflag = 0;
            InitializeComponent();
        }
        */

        // gameManager を引数にする場合のMainWindow?

        public GeisterUI(GameManager gameManager)
        {
            modeflag = 0;

            InitializeComponent();
            this.gameManager = gameManager;

            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);

            // Timespan 10ミリ秒
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();

            GameProcessDeligate gameDelegate = new GameProcessDeligate(gameManager.ProcessGame);
            IAsyncResult result = gameDelegate.BeginInvoke(null, null);

        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            count++;

            if (count == 10000)
                count = 0;

            // 要素の描画を無効にして、新しい完全なレイアウト パスを強制します。 レイアウト サイクルが完了した後に、OnRender が呼び出されます。
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // 画面の描画を行う
            if (gameManager != null)
            {
                display(gameManager.gamestate.Board);
            }
            // tick確認用 使うときはコメントアウトすること
            //count1.Text = "count" + count;
            count1.Text = "Player1: " + gameManager.P1.Name;
            count4.Text = "Player2: " + gameManager.P2.Name;
            if (gameManager.gamestate.Winner != FieldObject.blank)
            {
                Result.Text = gameManager.gamestate.Winner.ToString() + " Win!";

            }
            else if (gameManager.gamestate.Flag)
            {
                Result.Text = " Draw...";
            }
        }

        // テスト用ボタン1(モード切替)のクリック時処理の記述
        // 開発途中用
        private void bt_test1_Click(object sender, RoutedEventArgs e)
        {
            if (modeflag == 0)
            {
                mode(1);
                modeflag = 1;
            }
            else
            {
                mode(0);
                modeflag = 0;
            }
        }

        // テスト用ボタン2(表示テスト)のクリック時処理の記述
        // 開発途中用
        private void bt_test2_Click(object sender, RoutedEventArgs e)
        {
            GhostType[,] test = new GhostType[6, 6]
            {{GhostType.Blank,GhostType.P2evil,GhostType.Blank,GhostType.P2evil,GhostType.Blank,GhostType.Blank},
            {GhostType.Blank,GhostType.Blank,GhostType.P2good,GhostType.Blank,GhostType.P2good,GhostType.Blank},
            {GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank},
            {GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank},
            {GhostType.Blank,GhostType.P1good,GhostType.Blank,GhostType.P1good,GhostType.P1good,GhostType.Blank},
            {GhostType.Blank,GhostType.P1evil,GhostType.Blank,GhostType.P1evil,GhostType.P1evil,GhostType.Blank}};

            display(test);
        }

        // テスト用ボタン3(配置リセット)のクリック時処理の記述
        // 開発途中用
        private void bt_test3_Click(object sender, RoutedEventArgs e)
        {
            //配置のリセットを行う
            reset();
        }

        // 初期配置に盤面をリセットする関数reset
        // 開発途中用
        private void reset()
        {
            // サイズのリセットを行う
            ghost11.Width = 50; ghost11.Height = 50;
            ghost12.Width = 50; ghost12.Height = 50;
            ghost13.Width = 50; ghost13.Height = 50;
            ghost14.Width = 50; ghost14.Height = 50;
            ghost15.Width = 50; ghost15.Height = 50;
            ghost16.Width = 50; ghost16.Height = 50;
            ghost17.Width = 50; ghost17.Height = 50;
            ghost18.Width = 50; ghost18.Height = 50;

            ghost21.Width = 50; ghost21.Height = 50;
            ghost22.Width = 50; ghost22.Height = 50;
            ghost23.Width = 50; ghost23.Height = 50;
            ghost24.Width = 50; ghost24.Height = 50;
            ghost25.Width = 50; ghost25.Height = 50;
            ghost26.Width = 50; ghost26.Height = 50;
            ghost27.Width = 50; ghost27.Height = 50;
            ghost28.Width = 50; ghost28.Height = 50;

            // 配置のリセットを行う
            GhostType[,] reset = new GhostType[6, 6]
            {{GhostType.Blank,GhostType.P2evil,GhostType.P2evil,GhostType.P2evil,GhostType.P2evil,GhostType.Blank},
            {GhostType.Blank,GhostType.P2good,GhostType.P2good,GhostType.P2good,GhostType.P2good,GhostType.Blank},
            {GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank},
            {GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank},
            {GhostType.Blank,GhostType.P1good,GhostType.P1good,GhostType.P1good,GhostType.P1good,GhostType.Blank},
            {GhostType.Blank,GhostType.P1evil,GhostType.P1evil,GhostType.P1evil,GhostType.P1evil,GhostType.Blank}};

            display(reset);
        }

        // 盤面の配列を受け取って、その内容を表示する関数display
        private void display(GhostType[,] input)
        //private void display()
        {
            // 入力配列：例
            // 開発途中用(最終的にコメントアウトすること)
            /*
            GhostType[,] input = new GhostType[6,6] 
            {{GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank},
            {GhostType.P2good,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.P2evil},
            {GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.P1good,GhostType.P2evil},
            {GhostType.P2good,GhostType.Blank,GhostType.Blank,GhostType.P1good,GhostType.Blank,GhostType.P2evil},
            {GhostType.P2good,GhostType.Blank,GhostType.P1good,GhostType.Blank,GhostType.Blank,GhostType.Blank},
            {GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank,GhostType.Blank}};
            */

            // ループ用変数
            int i = new int();
            int j = new int();

            // 配置済みゴースト数の確認
            int p1evil = new int();
            int p1good = new int();
            int p2evil = new int();
            int p2good = new int();

            p1evil = 0;
            p1good = 0;
            p2evil = 0;
            p2good = 0;

            // 確認用
            //set11(0, 0);
            /*
            if (array2D[0, 0] == GhostType.P1evil)
            {
                statustext.Text = "p1evil OK";
            }
             */

            // 配置確認ループ
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    // 座標(i,j)マスがp1goodゴーストだった場合
                    if (input[i, j] == GhostType.P1good)
                    {
                        if (p1good == 0)
                        {
                            set11(j * 60 + 15, i * 120 - 300);
                        }
                        if (p1good == 1)
                        {
                            set12(j * 60 + 15, i * 120 - 300);
                        }
                        if (p1good == 2)
                        {
                            set13(j * 60 + 15, i * 120 - 300);
                        }
                        if (p1good == 3)
                        {
                            set14(j * 60 + 15, i * 120 - 300);
                        }
                        p1good++;
                    }
                    // 座標(i,j)マスがp1evilゴーストだった場合
                    if (input[i, j] == GhostType.P1evil)
                    {
                        if (p1evil == 0)
                        {
                            set15(j * 60 + 15, i * 120 - 300);
                        }
                        if (p1evil == 1)
                        {
                            set16(j * 60 + 15, i * 120 - 300);
                        }
                        if (p1evil == 2)
                        {
                            set17(j * 60 + 15, i * 120 - 300);
                        }
                        if (p1evil == 3)
                        {
                            set18(j * 60 + 15, i * 120 - 300);
                        }
                        p1evil++;
                    }
                    // 座標(i,j)マスがp2goodゴーストだった場合
                    if (input[i, j] == GhostType.P2good)
                    {
                        if (p2good == 0)
                        {
                            set21(j * 60 + 15, i * 120 - 300);
                        }
                        if (p2good == 1)
                        {
                            set22(j * 60 + 15, i * 120 - 300);
                        }
                        if (p2good == 2)
                        {
                            set23(j * 60 + 15, i * 120 - 300);
                        }
                        if (p2good == 3)
                        {
                            set24(j * 60 + 15, i * 120 - 300);
                        }
                        p2good++;
                    }
                    // 座標(i,j)マスがp2evilゴーストだった場合
                    if (input[i, j] == GhostType.P2evil)
                    {
                        if (p2evil == 0)
                        {
                            set25(j * 60 + 15, i * 120 - 300);
                        }
                        if (p2evil == 1)
                        {
                            set26(j * 60 + 15, i * 120 - 300);
                        }
                        if (p2evil == 2)
                        {
                            set27(j * 60 + 15, i * 120 - 300);
                        }
                        if (p2evil == 3)
                        {
                            set28(j * 60 + 15, i * 120 - 300);
                        }
                        p2evil++;
                    }

                }
            }

            set31(GhostType.P1good, 4 - p1good);
            set31(GhostType.P1evil, 4 - p1evil);
            set31(GhostType.P2good, 4 - p2good);
            set31(GhostType.P2evil, 4 - p2evil);

        }

        //---以下ghost00を配置する為の関数set00(00 = 11~18, 21~28)---
        // マージン値x,yを受け取り、対応する座標に配置

        // ghost11(good)を配置する関数 set11
        private void set11(int x, int y)
        {
            Thickness margin11 = new Thickness(x, y, 0, 0);
            ghost11.Margin = margin11;
        }

        // ghost12(good)を配置する関数 set12
        private void set12(int x, int y)
        {
            Thickness margin12 = new Thickness(x, y, 0, 0);
            ghost12.Margin = margin12;
        }

        // ghost13(good)を配置する関数 set13
        private void set13(int x, int y)
        {
            Thickness margin13 = new Thickness(x, y, 0, 0);
            ghost13.Margin = margin13;
        }

        // ghost14(good)を配置する関数 set14
        private void set14(int x, int y)
        {
            Thickness margin14 = new Thickness(x, y, 0, 0);
            ghost14.Margin = margin14;
        }

        // ghost15(evil)を配置する関数 set15
        private void set15(int x, int y)
        {
            Thickness margin15 = new Thickness(x, y, 0, 0);
            ghost15.Margin = margin15;
        }

        // ghost16(evil)を配置する関数 set16
        private void set16(int x, int y)
        {
            Thickness margin16 = new Thickness(x, y, 0, 0);
            ghost16.Margin = margin16;
        }

        // ghost17(evil)を配置する関数 set17
        private void set17(int x, int y)
        {
            Thickness margin17 = new Thickness(x, y, 0, 0);
            ghost17.Margin = margin17;
        }

        // ghost18(evil)を配置する関数 set18
        private void set18(int x, int y)
        {
            Thickness margin18 = new Thickness(x, y, 0, 0);
            ghost18.Margin = margin18;
        }

        // ghost21(good)を配置する関数 set21
        private void set21(int x, int y)
        {
            Thickness margin21 = new Thickness(x, y, 0, 0);
            ghost21.Margin = margin21;
        }

        // ghost22(good)を配置する関数 set22
        private void set22(int x, int y)
        {
            Thickness margin22 = new Thickness(x, y, 0, 0);
            ghost22.Margin = margin22;
        }

        // ghost23(good)を配置する関数 set23
        private void set23(int x, int y)
        {
            Thickness margin23 = new Thickness(x, y, 0, 0);
            ghost23.Margin = margin23;
        }

        // ghost24(good)を配置する関数 set24
        private void set24(int x, int y)
        {
            Thickness margin24 = new Thickness(x, y, 0, 0);
            ghost24.Margin = margin24;
        }

        // ghost25(evil)を配置する関数 set25
        private void set25(int x, int y)
        {
            Thickness margin25 = new Thickness(x, y, 0, 0);
            ghost25.Margin = margin25;
        }

        // ghost26(evil)を配置する関数 set26
        private void set26(int x, int y)
        {
            Thickness margin26 = new Thickness(x, y, 0, 0);
            ghost26.Margin = margin26;
        }

        // ghost27(evil)を配置する関数 set27
        private void set27(int x, int y)
        {
            Thickness margin27 = new Thickness(x, y, 0, 0);
            ghost27.Margin = margin27;
        }

        // ghost28(evil)を配置する関数 set28
        private void set28(int x, int y)
        {
            Thickness margin28 = new Thickness(x, y, 0, 0);
            ghost28.Margin = margin28;
        }

        // --- 以下、余ったゴースト(GhostType:gt) n体を盤外に配置する関数 ---
        private void set31(GhostType gt, int n)
        {
            if (n > 0)
            {
                if (gt == GhostType.P1good)
                {
                    set14(460, -120);
                }
                if (gt == GhostType.P1evil)
                {
                    set18(460, -60);
                }
                if (gt == GhostType.P2good)
                {
                    set24(460, 40);
                }
                if (gt == GhostType.P2evil)
                {
                    set28(460, 100);
                }
            }
            if (n > 1)
            {
                if (gt == GhostType.P1good)
                {
                    set13(480, -120);
                }
                if (gt == GhostType.P1evil)
                {
                    set17(480, -60);
                }
                if (gt == GhostType.P2good)
                {
                    set23(480, 40);
                }
                if (gt == GhostType.P2evil)
                {
                    set27(480, 100);
                }
            }
            if (n > 2)
            {
                if (gt == GhostType.P1good)
                {
                    set12(500, -120);
                }
                if (gt == GhostType.P1evil)
                {
                    set16(500, -60);
                }
                if (gt == GhostType.P2good)
                {
                    set22(500, 40);
                }
                if (gt == GhostType.P2evil)
                {
                    set26(500, 100);
                }
            }
            if (n > 3)
            {
                if (gt == GhostType.P1good)
                {
                    set11(520, -120);
                }
                if (gt == GhostType.P1evil)
                {
                    set15(520, -60);
                }
                if (gt == GhostType.P2good)
                {
                    set21(520, 40);
                }
                if (gt == GhostType.P2evil)
                {
                    set25(520, 100);
                }
            }
        }

        // --- 以下、表示モードを切り替える関数mode ---
        // 引数flagに0を指定で共通表示モードに,1を指定で判別モードに
        private void mode(int flag)
        {
            if (flag == 1)
            {
                ghost15.Source = p1evilmaster.Source;
                ghost16.Source = p1evilmaster.Source;
                ghost17.Source = p1evilmaster.Source;
                ghost18.Source = p1evilmaster.Source;

                ghost25.Source = p2evilmaster.Source;
                ghost26.Source = p2evilmaster.Source;
                ghost27.Source = p2evilmaster.Source;
                ghost28.Source = p2evilmaster.Source;
            }
            if (flag == 0)
            {
                ghost15.Source = p1goodmaster.Source;
                ghost16.Source = p1goodmaster.Source;
                ghost17.Source = p1goodmaster.Source;
                ghost18.Source = p1goodmaster.Source;

                ghost25.Source = p2goodmaster.Source;
                ghost26.Source = p2goodmaster.Source;
                ghost27.Source = p2goodmaster.Source;
                ghost28.Source = p2goodmaster.Source;
            }
        }

    }
}
