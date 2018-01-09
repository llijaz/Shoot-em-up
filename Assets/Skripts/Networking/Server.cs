using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    public int port;

    Networking networking;

    IPAddress ip;
    TcpListener server;

    List<TcpClient> clients;

    public Server(Networking networking, IPAddress _ip, int port)
    {
        this.networking = networking;

        this.port = port;

        // IPAddress ip = IPAddress.Parse(Networking.GetLocalIPAddress());
        IPAddress ip = _ip;

        server = new TcpListener(ip, port);

        clients = new List<TcpClient>();
    }

    public void Start()
    {
        new Thread(Run).Start();
    }

    void Run()
    {
        server.Start();

        while (true)
        {
            Debug.Log("Wait for clients...");

            TcpClient client = server.AcceptTcpClient();

            clients.Add(client);

            WriteToClient(client, "a");
            Game.instance.SetCountDown(5);

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

            Debug.Log("Client connected!");
        }
    }

    public void WriteToClient(string message)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            WriteToClient(clients[i], message);
        }
    }

    public void WriteToClient(TcpClient client, string message)
    {
        message = "#" + message;

        NetworkStream stream = client.GetStream();

        byte[] sendData = Encoding.ASCII.GetBytes(message);

        stream.Write(sendData, 0, message.Length);
        // stream.Write(sendData, 0, sendData.Length);
    }

    public void Stop()
    {
        server.Stop();
    }
}
