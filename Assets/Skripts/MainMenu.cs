using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour
{
    public GameObject hostPanel;
    public InputField hostPort;
    public InputField hostIp;

    public GameObject joinPanel;
    public InputField joinPort;
    public InputField joinIp;

    void Start()
    {

    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("FreeGame"))
        {
            Main.mode = Mode.FreeGame;
            Main.bot = false;

            Application.LoadLevel(1);
        }

        if (CrossPlatformInputManager.GetButtonDown("Bot"))
        {
            Main.mode = Mode.FreeGame;
            Main.bot = true;

            Application.LoadLevel(1);
        }

        if (CrossPlatformInputManager.GetButtonDown("Casual"))
        {
            Main.mode = Mode.Casual;

            Application.LoadLevel(1);
        }

        if (CrossPlatformInputManager.GetButtonDown("Host"))
        {
            ResetAll();

            hostPanel.SetActive(true);

            hostPort.text = "27015";
            hostIp.text = Networking.GetLocalIPAddress();
        }

        if (CrossPlatformInputManager.GetButtonDown("Join"))
        {
            ResetAll();

            joinPanel.SetActive(true);

            joinPort.text = "27015";
            joinIp.text = "127.0.0.1";
        }

        if (CrossPlatformInputManager.GetButtonDown("Host-Host"))
        {
            Main.mode = Mode.Host;

            Main.port = int.Parse(hostPort.text);
            Main.ip = hostIp.text;

            Application.LoadLevel(1);
        }

        if (CrossPlatformInputManager.GetButtonDown("Join-Join"))
        {
            Main.mode = Mode.Join;

            Main.port = int.Parse(joinPort.text);
            Main.ip = joinIp.text;

            Application.LoadLevel(1);
        }
    }

    void ResetAll()
    {
        hostPanel.SetActive(false);
        joinPanel.SetActive(false);
    }
}
