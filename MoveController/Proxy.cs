using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Timers;

namespace MoveController
{
    internal class StateObject
    {
        public Socket Socket;
        public const int BufferSize = 256;
        public byte[] buffer = new byte[BufferSize];
        public List<byte> ReceivedBytes = new List<byte>();
    }

    abstract public class ProxyBase
    {
        public const int LISTEN_PORT = 11000;

        protected Socket _Sock { get; private set; }
        private System.Threading.Semaphore _AckLock = new System.Threading.Semaphore(0, 1);
        public bool IsConnected
        {
            get
            {
                if(_Sock != null)
                {
                    return _Sock.Connected;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Init()
        {
            _Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            InitProtected();
            Listen();
        }
        protected abstract void InitProtected();

        private void SendAck()
        {
            byte[] ack = { 97, 99, 107 }; // a,c,k
            _Sock.Send(ack);
        }

        public void SendData(byte[] data)
        {
            _AckLock.WaitOne();
            _Sock.Send(data);
            
        }

        private void Listen()
        {
            StateObject state = new StateObject();
            state.Socket = _Sock;
            _Sock.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, OnPacketReceived, state);
        }

        private void OnPacketReceived(IAsyncResult result)
        {
            Debug.Log("Data received.");
            StateObject state = result.AsyncState as StateObject;
            Socket soc = state.Socket;

            int bytesRead = soc.EndReceive(result);
            if (bytesRead > 0)
            {
                state.ReceivedBytes.AddRange(state.buffer);
                soc.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, OnPacketReceived, state);
            }
            else
            {
                OnDataReceived(state.ReceivedBytes);
                SendAck();
            }
        }
        public virtual void OnDataReceived(List<byte> data) { }

        protected void Accept()
        {
            _Sock = _Sock.Accept();
        }
    }

    public class ProxyClient  : ProxyBase
    {

        protected override void InitProtected()
        {
            Debug.Log("Init client");
            _Sock.Connect(new IPEndPoint(IPAddress.Loopback, LISTEN_PORT));
        }

        public override void OnDataReceived(List<byte> data)
        {
            Debug.Log("Received "+data.Count+" bytes");
            long time = BitConverter.ToInt64(data.ToArray(), 0);
            PSMove.SendTime = time;
        }
    }


    public class ProxyServer : ProxyBase
    {
        private Timer _Timer;

        
        public float SendDataTimeout = 0.02f;

        protected override void InitProtected()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            throw new Exception("Proxy server cannot run from Unity.");
#endif
            Debug.Log("Init server");
            _Sock.Bind(new IPEndPoint(IPAddress.Loopback, LISTEN_PORT));
            _Sock.Listen(1);

            Debug.Log("Started listen server");
            Accept();
            Debug.Log("Connected.");
            _Timer = new Timer();
            _Timer.Elapsed += SendData;
            _Timer.Start();
            Debug.Log("Client disconnected. ");
        }

        private void SendData(object sender, ElapsedEventArgs e) 
        {
            Debug.Log("Send data.");
            byte[] data = BitConverter.GetBytes(DateTime.Now.Ticks);
            SendData(data);
        }

        public override void OnDataReceived(List<byte> data)
        {
            string ack = BitConverter.ToString(data.ToArray());
            if(ack == "ack")
            {
                
            }
        }
        private void OnDisconnected(IAsyncResult result)
        {
            Debug.Log("Client disconnected");
        }

    }
}
