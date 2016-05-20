using Ktos.Fa287a;
using System;

namespace Ktos.Fa287a
{
    internal class KeyboardSimulator
    {
        private Fa287aDriver sp;
        private WindowsInput.IKeyboardSimulator ks;
        private bool Fn;

        public KeyboardSimulator(string portName)
        {
            sp = new Fa287aDriver(portName);
            ks = new WindowsInput.InputSimulator().Keyboard;

            sp.KeyDown += Sp_KeyDown;
            sp.KeyUp += Sp_KeyUp;
        }

        public void Open()
        {
            sp.Open();
        }

        public void Close()
        {
            sp.Close();
        }

        private WindowsInput.Native.VirtualKeyCode ConvertKey(Key k)
        {
            if (Fn)
            {
                switch (k)
                {
                    case Key.Semicolon: return WindowsInput.Native.VirtualKeyCode.PRIOR;
                    case Key.Apostrophe: return WindowsInput.Native.VirtualKeyCode.NEXT;
                    case Key.ArrowLeft: return WindowsInput.Native.VirtualKeyCode.HOME;
                    case Key.ArrowRight: return WindowsInput.Native.VirtualKeyCode.END;                    
                }
            }

            switch (k)
            {
                case Key.Accent: return WindowsInput.Native.VirtualKeyCode.OEM_3;
                case Key.N_1: return WindowsInput.Native.VirtualKeyCode.VK_1;
                case Key.N_2: return WindowsInput.Native.VirtualKeyCode.VK_2;
                case Key.N_3: return WindowsInput.Native.VirtualKeyCode.VK_3;
                case Key.N_4: return WindowsInput.Native.VirtualKeyCode.VK_4;
                case Key.N_5: return WindowsInput.Native.VirtualKeyCode.VK_5;
                case Key.N_6: return WindowsInput.Native.VirtualKeyCode.VK_6;
                case Key.N_7: return WindowsInput.Native.VirtualKeyCode.VK_7;
                case Key.N_8: return WindowsInput.Native.VirtualKeyCode.VK_8;
                case Key.N_9: return WindowsInput.Native.VirtualKeyCode.VK_9;
                case Key.N_0: return WindowsInput.Native.VirtualKeyCode.VK_0;
                case Key.Minus: return WindowsInput.Native.VirtualKeyCode.OEM_MINUS;
                case Key.Plus: return WindowsInput.Native.VirtualKeyCode.OEM_PLUS;
                case Key.Backspace: return WindowsInput.Native.VirtualKeyCode.BACK;
                case Key.Tab: return WindowsInput.Native.VirtualKeyCode.TAB;
                case Key.Q: return WindowsInput.Native.VirtualKeyCode.VK_Q;
                case Key.W: return WindowsInput.Native.VirtualKeyCode.VK_W;
                case Key.E: return WindowsInput.Native.VirtualKeyCode.VK_E;
                case Key.R: return WindowsInput.Native.VirtualKeyCode.VK_R;
                case Key.T: return WindowsInput.Native.VirtualKeyCode.VK_T;
                case Key.Y: return WindowsInput.Native.VirtualKeyCode.VK_Y;
                case Key.U: return WindowsInput.Native.VirtualKeyCode.VK_U;
                case Key.I: return WindowsInput.Native.VirtualKeyCode.VK_I;
                case Key.O: return WindowsInput.Native.VirtualKeyCode.VK_O;
                case Key.P: return WindowsInput.Native.VirtualKeyCode.VK_P;
                case Key.LBracket: return WindowsInput.Native.VirtualKeyCode.OEM_4;
                case Key.RBracket: return WindowsInput.Native.VirtualKeyCode.OEM_6;
                case Key.Backslash: return WindowsInput.Native.VirtualKeyCode.OEM_5;
                case Key.A: return WindowsInput.Native.VirtualKeyCode.VK_A;
                case Key.S: return WindowsInput.Native.VirtualKeyCode.VK_S;
                case Key.D: return WindowsInput.Native.VirtualKeyCode.VK_D;
                case Key.F: return WindowsInput.Native.VirtualKeyCode.VK_F;
                case Key.G: return WindowsInput.Native.VirtualKeyCode.VK_G;
                case Key.H: return WindowsInput.Native.VirtualKeyCode.VK_H;
                case Key.J: return WindowsInput.Native.VirtualKeyCode.VK_J;
                case Key.K: return WindowsInput.Native.VirtualKeyCode.VK_K;
                case Key.L: return WindowsInput.Native.VirtualKeyCode.VK_L;
                case Key.Semicolon: return WindowsInput.Native.VirtualKeyCode.OEM_1;
                case Key.CapsLock: return WindowsInput.Native.VirtualKeyCode.CAPITAL;
                case Key.Apostrophe: return WindowsInput.Native.VirtualKeyCode.OEM_7;
                case Key.Enter: return WindowsInput.Native.VirtualKeyCode.RETURN;
                case Key.LShift: return WindowsInput.Native.VirtualKeyCode.SHIFT;
                case Key.Z: return WindowsInput.Native.VirtualKeyCode.VK_Z;
                case Key.X: return WindowsInput.Native.VirtualKeyCode.VK_X;
                case Key.C: return WindowsInput.Native.VirtualKeyCode.VK_C;
                case Key.V: return WindowsInput.Native.VirtualKeyCode.VK_V;
                case Key.B: return WindowsInput.Native.VirtualKeyCode.VK_B;
                case Key.N: return WindowsInput.Native.VirtualKeyCode.VK_N;
                case Key.M: return WindowsInput.Native.VirtualKeyCode.VK_M;
                case Key.Comma: return WindowsInput.Native.VirtualKeyCode.OEM_COMMA;
                case Key.Dot: return WindowsInput.Native.VirtualKeyCode.OEM_PERIOD;
                case Key.ArrowUp: return WindowsInput.Native.VirtualKeyCode.UP;
                case Key.RShift: return WindowsInput.Native.VirtualKeyCode.SHIFT;
                case Key.Slash: return WindowsInput.Native.VirtualKeyCode.OEM_2;
                case Key.LCtrl: return WindowsInput.Native.VirtualKeyCode.CONTROL;
                case Key.Fn: return WindowsInput.Native.VirtualKeyCode.NONAME;
                case Key.LWin: return WindowsInput.Native.VirtualKeyCode.LWIN;
                case Key.Alt: return WindowsInput.Native.VirtualKeyCode.LMENU;
                case Key.Space: return WindowsInput.Native.VirtualKeyCode.SPACE;
                case Key.AltGr: return WindowsInput.Native.VirtualKeyCode.MENU;
                case Key.ArrowLeft: return WindowsInput.Native.VirtualKeyCode.LEFT;
                case Key.ArrowDown: return WindowsInput.Native.VirtualKeyCode.DOWN;
                case Key.ArrowRight: return WindowsInput.Native.VirtualKeyCode.RIGHT;
                case Key.Del: return WindowsInput.Native.VirtualKeyCode.DELETE;
                case Key.Escape: return WindowsInput.Native.VirtualKeyCode.ESCAPE;
                default: Console.WriteLine((byte)k); return WindowsInput.Native.VirtualKeyCode.NONAME;
            }
        }

        private void Sp_KeyUp(object sender, KeyHandlerEventArgs e)
        {
            if (e.Key == Key.Fn)
                Fn = false;

            ks.KeyUp(ConvertKey(e.Key));
        }

        private void Sp_KeyDown(object sender, KeyHandlerEventArgs e)
        {
            if (e.Key == Key.Fn)
                Fn = true;

            ks.KeyDown(ConvertKey(e.Key));
        }
    }
}