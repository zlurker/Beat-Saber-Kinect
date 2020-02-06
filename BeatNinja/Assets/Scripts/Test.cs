using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Test : MonoBehaviour
{
    Thread tcpListenerThread;
    TcpClient socketConnection;
    TcpListener tcpListener;
    // Use this for initialization
    void Start()
    {
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();

        socketConnection = new TcpClient("localhost", 8052);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ListenForIncommingRequests()
    {
        // Create listener on localhost port 8052. 			
        tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052);
        tcpListener.Start();
        tcpListener.AcceptTcpClient();
        Debug.Log("Connected to client");

    }
}
