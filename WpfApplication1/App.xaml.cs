using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Geister.GameInformation;
using Geister.GameSystem;

namespace Geister
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            
            // プログラム開始時にyes no cancelを問う
            // cancelならプログラム終了
            /*
            var result = MessageBox.Show("second window を起動しますか？", ".NET TIPS #1119", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Cancel)
                return;
            */
             
            App app = new App();
            //app.StartupUri = new Uri("GeisterUI.xaml", UriKind.Relative);

            // 上記の選択肢によってwindowを切り替える場合、app.StartupUriを以下に書き換える。
            /*
            if (result == MessageBoxResult.Yes)
            {
                app.StartupUri = new Uri("GeisterUI.xaml", UriKind.Relative);
            }
            else
            {
                app.StartupUri = new Uri("GeisterUI.xaml", UriKind.Relative);
            }
            */


            GameManager gameManager = new GameManager(new DebugPlayer("a"), new DebugPlayer("b"), 100, 100);

            app.InitializeComponent();
            app.Run(new GeisterUI(gameManager));
        }
    }
}
