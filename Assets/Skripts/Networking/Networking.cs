using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public abstract class Networking : MonoBehaviour
{
    public int port;
    public IPAddress ip;

    Server server;
    Client client;

    List<string> loglist;

    private void Start()
    {
        loglist = new List<string>();

        Init();
    }

    private void Update()
    {
        
        for (int i = 0; i < loglist.Count; i++)
        {
            GetMessage(loglist[i]);
        }

        loglist = new List<string>();

        Draw();
    }

    public void Log(string message)
    {
        // Debug.Log("message: " + message);

        string[] messages = message.Substring(1).Split('#');

        for (int i = 0; i < messages.Length; i++)
        {
            loglist.Add(messages[i]);
        }
    }

    public abstract void Init();
    public abstract void Draw();

    public abstract void GetMessage(string message);

    public void Write(string message)
    {
        if (server != null)
        {
            server.WriteToClient(message);
        }

        if (client != null)
        {
            client.Write(message);
        }
    }

    public void SetIp(string ip)
    {
        try
        {
            this.ip = IPAddress.Parse(ip);
        }
        catch
        {
            this.ip = IPAddress.Parse(GetLocalIPAddress());
            Debug.Log("Failed to set the ip address");
        }
    }

    public void SetPort(string port)
    {
        try
        {
            this.port = int.Parse(port);
        }
        catch
        {
            this.port = 27015;
            Debug.Log("Failed to set the port");
        }
    }

    public void SetPort(int port)
    {
        this.port = port;
    }

    public void StartServer()
    {
        CheckValues();

        Debug.Log("Starting Server...");

        Debug.Log("Ip  : " + ip);
        Debug.Log("Port: " + port);

        server = new Server(this, ip, port);

        server.Start();
    }

    public void StartClient()
    {
        CheckValues();

        Debug.Log("Starting Client...");

        Debug.Log("Ip  : " + ip);
        Debug.Log("Port: " + port);

        client = new Client(this, ip, port);

        client.Start();
    }
    
    public void Stop()
    {
        if (server != null)
        {
            server.Stop();
        }

        if (client != null)
        {
            client.Stop();
        }
    }

    public bool MessageEquals(string string1, string string2)
    {
        byte[] bytes1 = Encoding.ASCII.GetBytes(string1);
        byte[] bytes2 = Encoding.ASCII.GetBytes(string2);

        string1 = "";
        string2 = "";

        int smallerLen = bytes1.Length;
        if (bytes2.Length < smallerLen)
            smallerLen = bytes2.Length;

        for (int i = 0; i < smallerLen; i++)
        {
            string1 += bytes1[i];
            string2 += bytes2[i];
        }

        return string1.Equals(string2);
    }

    void CheckValues()
    {
        if (ip == null)
        {
            ip = IPAddress.Parse("127.0.0.1");
        }

        if (port == 0)
        {
            port = 27015;
        }
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
