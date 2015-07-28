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
        public bool IsDebug = false;
        protected Socket _Sock { get; private set; }
        
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

        

        public void SendData(byte[] data)
        {
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
            Log("Packet received.");
            StateObject state = result.AsyncState as StateObject;
            Socket soc = state.Socket;

            int bytesRead = soc.EndReceive(result);
            
            if(bytesRead >= 0)
            {
                // TODO: Code below does not always manage to add byte before calling OnDataReceived. Thread sync needed, but how?
                //state.ReceivedBytes.AddRange( new List<byte>( state.buffer).GetRange(0, bytesRead));
                for (int i = 0; i < bytesRead;i++ )
                {
                    state.ReceivedBytes.Add(state.buffer[i]);
                }
                    soc.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, OnPacketReceived, state);
                if (bytesRead == StateObject.BufferSize)
                {    
                    
                    return;
                }
                byte[] finalBytes = state.ReceivedBytes.ToArray();
                OnDataReceived(state.buffer); //HACK: If needed synchronize thread to allow sending more than 256 bytes of data.
                //OnDataReceived(finalBytes); 
                state.ReceivedBytes.Clear();
            }
        }
        public virtual void OnDataReceived(byte[] data) { }

        protected void Accept()
        {
            _Sock = _Sock.Accept();
        }
        protected void Log(object logObject)
        {
            if(IsDebug)
            {
                Debug.Log(logObject);
            }
        }
    }

    public class ProxyClient  : ProxyBase
    {

        protected override void InitProtected()
        {
            Log("Init client");
            _Sock.Connect(new IPEndPoint(IPAddress.Loopback, LISTEN_PORT));
            SendAck();
        }

        private void SendAck()
        {
            byte[] ack = { 97, 99, 107 }; // a,c,k
            _Sock.Send(ack);
            Log("Send ACK");
        }

        public override void OnDataReceived(byte[] data)
        {
            Log("Received "+data.Length+" bytes");
            SendAck();
            PSMoveSerializer.Deserialize(data);
        }
    }


    public class ProxyServer : ProxyBase
    {
        //private System.Threading.Semaphore _AckLock = new System.Threading.Semaphore(0, 1);
        
        public float SendDataTimeout = 0.02f;

        protected override void InitProtected()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            throw new Exception("Proxy server cannot run from Unity.");
#endif
            Log("Init server");
            _Sock.Bind(new IPEndPoint(IPAddress.Loopback, LISTEN_PORT));
            _Sock.Listen(1);
            PSMove.InitDevice();
            Log("Started listen server");
            Accept();
            Log("Connected.");
            
        }

        private void SendData()
        {
            Log("Send data.");
            byte[] data = PSMoveSerializer.Serialize();
            if(data.Length > StateObject.BufferSize)
            {
                throw new Exception("Data chunk is bigger than " + StateObject.BufferSize + "bytes. Check OnPackedReceived for details.");
            }
            SendData(data);
        }

        public override void OnDataReceived(byte[] data)
        {

            string ack = Encoding.Default.GetString(data, 0, 3);
            
            if(ack == "ack")
            {
                Log("Received ack.");
                SendData();
            }
        }
        private void OnDisconnected(IAsyncResult result)
        {
            Log("Client disconnected");
        }

    }
}
