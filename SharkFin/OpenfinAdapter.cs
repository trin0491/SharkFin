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
                var provider = new SerialPortProvider(runtime);
            });
        }

    }
}
