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
using System.IO.Pipes;
using System.Security.AccessControl;

namespace Ktos.Fa287a
{
    /// <summary>
    /// A very basic Inter-Process communication implementation
    /// </summary>
    internal class Ipc
    {        
        private NamedPipeServerStream pipeServer;

        /// <summary>
        /// Event which is raised when data from client is received
        /// </summary>
        public event Action<int> DataReceived;

        /// <summary>
        /// Sends data to the server
        /// </summary>
        /// <param name="data">One byte of data to be send to server</param>
        public static void WriteServer(string computerName, string pipeName, byte data)
        {
            PipeSecurity ps = new PipeSecurity();
            ps.AddAccessRule(new PipeAccessRule("Users", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, AccessControlType.Allow));

            using (var pipeClient = new NamedPipeClientStream(computerName, pipeName, PipeDirection.Out, PipeOptions.Asynchronous))
            {
                pipeClient.Connect();
                pipeClient.WriteByte(data);
            }
        }

        /// <summary>
        /// Creates a new server listening for IPC messages
        /// </summary>
        public Ipc(string pipeName)
        {
            PipeSecurity ps = new PipeSecurity();
            ps.AddAccessRule(new PipeAccessRule("Users", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, AccessControlType.Allow));

            pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 1024, 1024, ps);
            pipeServer.BeginWaitForConnection(pipeConnected, null);
        }

        private void pipeConnected(IAsyncResult ar)
        {
            pipeServer.EndWaitForConnection(ar);

            var data = pipeServer.ReadByte();
            DataReceived?.Invoke(data);

            pipeServer.Disconnect();
            pipeServer.BeginWaitForConnection(pipeConnected, null);
        }
    }

    /// <summary>
    /// Definition of all possible IPC messages
    /// </summary>
    internal enum IpcMessages
    {
        /// <summary>
        /// Makes server to connect to the device
        /// </summary>
        CONNECT = 1,

        /// <summary>
        /// Makes server to disconnect from the device
        /// </summary>
        DISCONNECT = 2
    }
}