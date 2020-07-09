using Openfin.Desktop;
using Openfin.Desktop.Messaging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace SharkFin
{
    class SerialPortProvider
    {

        private readonly ChannelProvider provider;
 
        public event EventHandler<ChannelConnectedEventArgs> ClientConnected;

        public SerialPortProvider(Runtime runtime)
        {
            provider = runtime.InterApplicationBus.Channel.CreateProvider("SerialPort");
            provider.RegisterTopic<string[]>("getPorts", OnGetPorts);
            provider.RegisterTopic<string>("sendKey", OnSendKey);
            provider.ClientConnected += Provider_ClientConnected;

            /* TODO where is the ability to dispatch from the provider to a specific client?
             * https://cdn.openfin.co/docs/javascript/stable/Channel_ChannelProvider.html#dispatch
             * http://cdn.openfin.co/docs/csharp/latest/OpenfinDesktop/html/D9E92A51.htm
             * 
             * Only the ChannelClient can DispatchAsync?
             * http://cdn.openfin.co/docs/csharp/latest/OpenfinDesktop/html/861D6A97.htm
             */

        }

        private void Provider_ClientConnected(object sender, ChannelConnectedEventArgs e)
        {
            // TODO security check and reject
            ClientConnected?.Invoke(this, e);
        }

        private string[] OnGetPorts()
        {
            return SerialPort.GetPortNames();
        }

        private void OnSendKey(string key)
        {
            SendKeys.SendWait(key);
        }

        public void OpenAsync()
        {
            provider.OpenAsync();
        }

        public void CloseAsync()
        {
            provider.CloseAsync();
        }
    }
}
