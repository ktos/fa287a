using System.IO.Ports;

namespace Ktos.Fa287a
{
    public class Fa287aDriver
    {
        private SerialPort sp;

        public Fa287aDriver(string portName)
        {
            sp = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
            sp.DataReceived += Sp_DataReceived;
            sp.ReceivedBytesThreshold = 1;
        }

        public void Open()
        {
            sp.Open();
            sp.ReadExisting();
        }

        public void Close()
        {
            sp.Close();
        }

        public event KeyHandler KeyUp;

        public event KeyHandler KeyDown;

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var b = (sender as SerialPort).ReadByte();

            if (b <= 128)
            {
                KeyDown?.Invoke(this, new KeyHandlerEventArgs((Key)b));
            }
            else
            {
                KeyUp?.Invoke(this, new KeyHandlerEventArgs((Key)(b - 128)));
            }
        }
    }
}