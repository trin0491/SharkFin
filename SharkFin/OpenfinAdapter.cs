using Openfin.Desktop;
using Openfin.Desktop.Messaging;
using System;
using System.IO.Ports;

namespace SharkFin
{
    class OpenfinAdapter
    {

        private readonly Runtime runtime;

        public OpenfinAdapter()
        {
            var runtimeOptions = new RuntimeOptions
            {
                Version = "stable"
            };
            runtime = Runtime.GetRuntimeInstance(runtimeOptions);
            runtime.Disconnected += Runtime_Disconnected;
        }

        private void Runtime_Disconnected(object sender, EventArgs e) => RuntimeDisconnected?.Invoke(this, EventArgs.Empty);

        public event EventHandler RuntimeDisconnected;

        public void Connect()
        {
            runtime.Connect(() => 
            {
                var provider = runtime.InterApplicationBus.Channel.CreateProvider("SerialPort");
                provider.RegisterTopic<string[]>("getPorts", onGetPorts);
                provider.Opened += Provider_Opened;
                provider.ClientConnected += Provider_ClientConnected;
                provider.Closed += Provider_Closed;
                provider.OpenAsync();

            });
        }

        private void Provider_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Closed");
        }

        private void Provider_ClientConnected(object sender, ChannelConnectedEventArgs e)
        {
            Console.WriteLine("Connected");
        }

        private void Provider_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Opened");
        }

        private string[] onGetPorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}
