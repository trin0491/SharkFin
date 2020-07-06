using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharkFin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly NotifyIcon notifyIcon = new NotifyIcon();
        readonly OpenfinAdapter openfin;

        public MainWindow()
        {
            InitializeComponent();
            openfin = new OpenfinAdapter();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            openfin.Connect();
            this.WindowState = WindowState.Minimized;
            this.notifyIcon.Icon = new Icon(SystemIcons.Question, 16, 16);
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(5000, "Title", "Some Text", ToolTipIcon.Info);
        }
    }
}
