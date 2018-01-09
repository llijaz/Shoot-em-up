using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Client
{
    public IPAddress ip;
    public int port;

    Networking networking;

    TcpClient client;

    public Client(Networking networking, IPAddress ip, int port)
    {
        this.networking = networking;

        this.ip = ip;
        this.port = port;
    }

    public void Start()
    {
        client = new TcpClient();

        client.Connect(ip, port);

        Debug.Log("Connecting to server...");

        new Thread(delegate ()
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                byte[] receivedBuffer = new byte[1024];

                stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                string message = Encoding.ASCII.GetString(receivedBuffer, 0, receivedBuffer.Length);

                networking.Log(message);
            }
        }).Start();
    }

    public void Write(string message)
    {
        message = "#" + message;

        NetworkStream stream = client.GetStream();

        byte[] sendData = Encoding.ASCII.GetBytes(message);

        stream.Write(sendData, 0, message.Length);
    }

    public void Stop()
    {
        client.Close();
    }

}
