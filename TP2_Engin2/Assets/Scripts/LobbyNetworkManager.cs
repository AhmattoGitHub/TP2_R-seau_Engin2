using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Mirror.Discovery;
using UnityEngine.UI;

public class LobbyNetworkManager : NetworkManager
{
    private int m_playerCount = 0;

    private void Start()
    {

    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        LobbyManager.Instance.WaitForConfig();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        conn.identity.name = "";
        LobbyManager.Instance.AddToConnections(conn);
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        LobbyManager.Instance.RemoveFromConnections(conn);
        base.OnServerDisconnect(conn);
    }
}
