using Openfin.Desktop;
using Openfin.Desktop.Messaging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace SharkFin
{
    class SerialPortProvider
    {

        private readonly ChannelProvider provider;
 
        public event EventHandler<ChannelConnectedEventArgs> ClientConnected;

        public SerialPortProvider(Runtime runtime)
        {
            provider = runtime.InterApplicationBus.Channel.CreateProvider("SerialPort");
            provider.RegisterTopic<string[]>("getPorts", onGetPorts);
            provider.ClientConnected += Provider_ClientConnected;
        }

        private void Provider_ClientConnected(object sender, ChannelConnectedEventArgs e)
        {
            ClientConnected?.Invoke(this, e);
        }

        private string[] onGetPorts()
        {
            return SerialPort.GetPortNames();
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
