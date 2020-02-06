using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System;

public class TCPServer : MonoBehaviour {

    // Use this for initialization
    Thread tcpRequest;
    Thread tcpListenerThread;
    TcpListener tcpListener;

	void Start () {
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();

        tcpRequest = new Thread(new ThreadStart(Request));
        tcpRequest.IsBackground = true;
        tcpRequest.Start();

        /*Thread ov = new Thread(new ThreadStart(Request));
        ov.IsBackground = true;
        ov.Start();*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Request()
    {
        Socket reqSocket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream,
                                        ProtocolType.Tcp);

        // bind the listening socket to the port
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
        Debug.Log("Client started");
        reqSocket.Connect(ep);

        try
        {
            Debug.Log("Client connection = " + reqSocket.LocalEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        //Debug.LogFormat("Client Connected {0}", reqSocket.RemoteEndPoint);
    }

    void ListenForIncommingRequests()
    {

        Socket listenSocket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream,
                                        ProtocolType.Tcp);

        // bind the listening socket to the port
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
        
        listenSocket.Bind(ep);
        Debug.Log(listenSocket.IsBound);
        listenSocket.Listen(5);
        Debug.Log("Socket Listening");

        while (true)
        {
            Socket test = listenSocket.Accept();
            Debug.Log("Called once");
            Debug.Log("Script connected at " +  test.LocalEndPoint);
            try
            {
                Debug.Log("Test listenSocket" + test.RemoteEndPoint);
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
