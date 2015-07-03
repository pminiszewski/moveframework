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
        List<byte> ReceivedByte = new List<byte>();
    }
    public class ProxyClient
    {
        private Socket _Sock;
        public void Init()
        {
            _Sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _Sock.Connect(new IPEndPoint(IPAddress.Loopback, ProxyServer.LISTEN_PORT));
            Listen();
        }

        private void Listen()
        {
            StateObject state = new StateObject();
            state.Socket = _Sock;
            _Sock.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, OnDataReceived, state);

        }
        StringBuilder d;
        private void OnDataReceived(IAsyncResult result)
        {

            
        
        }
        
           
    
    }


    public class ProxyServer
    {
        
        public const int LISTEN_PORT = 11111;

        private Socket _Sock;
        private Timer _Timer;

        

        public bool IsConnected { get; private set; }
        public float SendDataTimeout = 0.02f;

        public void Init()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            throw new Exception("Proxy server cannot run from Unity.");
#endif
            _Sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _Sock.Bind(new IPEndPoint(IPAddress.Loopback, LISTEN_PORT));
            _Sock.Listen(1);
            _Sock.BeginDisconnect(true, OnDisconnected, new object());
                
            Debug.Log("Started listen server");
            _Sock.Accept();
            IsConnected = true;
            Debug.Log("Client conencted.");
            _Timer.Elapsed += SendData;
            _Timer.Start();

            Debug.Log("Client disconnected. ");
        }

        private void SendData(object sender, ElapsedEventArgs e)
        {
            Debug.Log("Send data.");
        }

        private void OnDisconnected(IAsyncResult result)
        {
            Debug.Log("Client disconnected");
        }

    }
}
