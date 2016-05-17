using System;
using System.IO.Ports;

namespace ConsoleApplication1
{
    public delegate void KeyHandler(object sender, KeyHandlerEventArgs e);

    public class KeyHandlerEventArgs
    {
        public KeyHandlerEventArgs(Key key)
        {
            Key = key;
        }

        public Key Key { get; private set; }
    }

    public enum Key
    {
        Escape = 8,
        Accent = 14,
        N_1 = 22,
        N_2 = 30,
        N_3 = 38,
        N_4 = 37,
        N_5 = 46,
        N_6 = 54,
        N_7 = 61,
        N_8 = 62,
        N_9 = 70,
        N_0 = 69,
        Minus = 78,
        Plus = 85,
        Backspace = 113,
        Tab = 13,
        Q = 21,
        W = 29,
        E = 36,
        R = 45,
        T = 44,
        Y = 53,
        U = 60,
        I = 67,
        O = 68,
        P = 77,
        LBracket = 84,
        RBracket = 91,
        Backslash = 93,
        CapsLock = 88,
        A = 28,
        S = 27,
        D = 35,
        F = 43,
        G = 52,
        H = 51,
        J = 59,
        K = 66,
        L = 75,
        Semicolon = 76,
        Apostrophe = 82,
        Enter = 90,
        LShift = 18,
        Z = 26,
        X = 34,
        C = 33,
        V = 42,
        B = 50,
        N = 49,
        M = 58,
        Comma = 65,
        Dot = 73,
        ArrowUp = 40,
        RShift = 89,
        Slash = 74,
        LCtrl = 20,
        Fn = 2,
        LWin = 7,
        Alt = 16,
        Space = 92,
        AltGr = 19,
        ArrowLeft = 94,
        ArrowDown = 96,
        ArrowRight = 47,
        Del = 102,
        Battery = 98
    }

    public class FA287ADriver
    {
        private SerialPort sp;

        public FA287ADriver(string portName)
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