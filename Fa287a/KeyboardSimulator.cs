#region License

/*
 * FA287A for Windows
 *
 * Copyright (C) Marcin Badurowicz <m at badurowicz dot net> 2016
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion License

using System;

namespace Ktos.Fa287a
{
    /// <summary>
    /// Handles simulating real keyboard by signals from the FA287A device
    /// </summary>
    public class KeyboardSimulator
    {
        private Fa287aDriver sp;
        private WindowsInput.IKeyboardSimulator ks;
        private bool keyFnActive;

        /// <summary>
        /// Gets if keyboard simulator is in active state
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Initializes a new instance of KeyboardSimulator and starts
        /// the connection (but not opens it) over the desired COM port.
        /// </summary>
        /// <param name="portName">Bluetooth device COM port</param>
        public KeyboardSimulator(string portName)
        {
            sp = new Fa287aDriver(portName);
            ks = new WindowsInput.InputSimulator().Keyboard;

            sp.KeyDown += Sp_KeyDown;
            sp.KeyUp += Sp_KeyUp;
        }

        /// <summary>
        /// Starts the connection, starts listening for keypresses
        /// and simulating them respectively
        /// </summary>
        public void Open()
        {
            sp.Open();
            IsOpen = true;
        }

        /// <summary>
        /// Stops the connection and simulation
        /// </summary>
        public void Close()
        {
            sp.Close();
            IsOpen = false;

            // turning off any special keys which may be active
            ks.KeyUp(WindowsInput.Native.VirtualKeyCode.CAPITAL);
            ks.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
            ks.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
            ks.KeyUp(WindowsInput.Native.VirtualKeyCode.LWIN);
            ks.KeyUp(WindowsInput.Native.VirtualKeyCode.LMENU);
            ks.KeyUp(WindowsInput.Native.VirtualKeyCode.MENU);
        }

        /// <summary>
        /// Converts between Virtual Key Codes for Windows and codes
        /// from the FA287A device
        /// </summary>
        /// <param name="k">A key code from the device</param>
        /// <returns>Windows API compatible Virtual Key Code</returns>
        private WindowsInput.Native.VirtualKeyCode ConvertKey(Key k)
        {
            if (keyFnActive)
            {
                switch (k)
                {
                    case Key.Semicolon: return WindowsInput.Native.VirtualKeyCode.PRIOR;
                    case Key.Apostrophe: return WindowsInput.Native.VirtualKeyCode.NEXT;
                    case Key.ArrowLeft: return WindowsInput.Native.VirtualKeyCode.HOME;
                    case Key.ArrowRight: return WindowsInput.Native.VirtualKeyCode.END;
                    
                    // Fn+1-0 and Minus and Plus = F1 to F12
                    case Key.N_1: return WindowsInput.Native.VirtualKeyCode.F1;
                    case Key.N_2: return WindowsInput.Native.VirtualKeyCode.F2;
                    case Key.N_3: return WindowsInput.Native.VirtualKeyCode.F3;
                    case Key.N_4: return WindowsInput.Native.VirtualKeyCode.F4;
                    case Key.N_5: return WindowsInput.Native.VirtualKeyCode.F5;
                    case Key.N_6: return WindowsInput.Native.VirtualKeyCode.F6;
                    case Key.N_7: return WindowsInput.Native.VirtualKeyCode.F7;
                    case Key.N_8: return WindowsInput.Native.VirtualKeyCode.F8;
                    case Key.N_9: return WindowsInput.Native.VirtualKeyCode.F9;
                    case Key.N_0: return WindowsInput.Native.VirtualKeyCode.F10;
                    case Key.Minus: return WindowsInput.Native.VirtualKeyCode.F11;
                    case Key.Plus: return WindowsInput.Native.VirtualKeyCode.F12;
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
                default: return WindowsInput.Native.VirtualKeyCode.NONAME;
            }
        }

        private void Sp_KeyUp(object sender, KeyHandlerEventArgs e)
        {
            if (e.Key == Key.Fn)
                keyFnActive = false;

            ks.KeyUp(ConvertKey(e.Key));
        }

        private void Sp_KeyDown(object sender, KeyHandlerEventArgs e)
        {
            if (e.Key == Key.Fn)
                keyFnActive = true;

            ks.KeyDown(ConvertKey(e.Key));
        }
    }
}