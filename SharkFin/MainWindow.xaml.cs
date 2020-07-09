using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace SharkFin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly String BALLOON_TIP_TITLE = "Matrix Connect";
        private static readonly int BALLOON_TIP_TIMEOUT = 5000;

        readonly NotifyIcon notifyIcon = new NotifyIcon();
        
        private readonly Openfin.Desktop.Runtime openfin;

        public MainWindow()
        {
            InitializeComponent();
            // TODO is version relevant/used?  do need to provide LicenseKey, SupportInformation?
            // http://cdn.openfin.co/docs/csharp/latest/OpenfinDesktop/html/6A71B701.htm
            var runtimeOptions = new Openfin.Desktop.RuntimeOptions
            {
                Version = "stable"
            };
            openfin = Openfin.Desktop.Runtime.GetRuntimeInstance(runtimeOptions);
            openfin.Disconnected += Openfin_RuntimeDisconnected;
            openfin.ConnectTimeout += Openfin_ConnectTimeout;
            openfin.Connect(() =>
            {
                var provider = new SerialPortProvider(openfin);
                provider.ClientConnected += SerialPortProvider_ClientConnected;
                provider.OpenAsync();
            });
        }


        private void ShowTooltip(string tipText, ToolTipIcon tipIcon)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                notifyIcon.ShowBalloonTip(BALLOON_TIP_TIMEOUT, BALLOON_TIP_TITLE, "Connected to Matrix Desktop", ToolTipIcon.Info);
            }));
        }

        private void Openfin_ConnectTimeout(object sender, EventArgs e)
        {
            ShowTooltip("Failed to connect to Matrix Desktop after timeout", ToolTipIcon.Error);
        }

        private void Openfin_RuntimeDisconnected(object sender, EventArgs e)
        {
            ShowTooltip("Disconnected from Matrix Desktop", ToolTipIcon.Error);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.notifyIcon.Icon = new Icon(SystemIcons.Question, 16, 16);
            this.notifyIcon.Visible = true;
        }

        private void SerialPortProvider_ClientConnected(object sender, Openfin.Desktop.Messaging.ChannelConnectedEventArgs e)
        {
            ShowTooltip("Connected to Matrix Desktop", ToolTipIcon.Info);
        }
    }
}
