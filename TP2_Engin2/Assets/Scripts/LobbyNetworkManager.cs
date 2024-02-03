using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LobbyNetworkManager : NetworkManager
{
    private int m_playerCount = 0;
    private string ipAdress;

    private void Start()
    {
        
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        m_playerCount++;
        base.OnServerAddPlayer(conn);
        LobbyManager.Instance.WaitForTeamSelection(conn);
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        m_playerCount--;
        base.OnServerDisconnect(conn);
    }
}
