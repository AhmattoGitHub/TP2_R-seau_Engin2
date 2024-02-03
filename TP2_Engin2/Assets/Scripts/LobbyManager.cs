using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    private static LobbyManager _instance;
    [SerializeField]
    private GameObject[] m_uiTextSlots;
    [SerializeField]
    private GameObject m_conigureMenu;
    [SerializeField]
    private List<NetworkConnectionToClient> m_connectedPlayers = new List<NetworkConnectionToClient>();
    [SerializeField]
    private List<NetworkConnectionToClient> m_players = new List<NetworkConnectionToClient>();
    [SerializeField]
    private List<NetworkConnectionToClient> m_enemies = new List<NetworkConnectionToClient>();

    private bool m_selectingTeam;
    private bool m_selectedPlayer = false;
    private bool m_selectedEnemy = false;

    public static LobbyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("There is no Instance");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    [ClientRpc]
    public void UpdateUI(int numberOfPlayers)
    {
        for (int i = 0; i < m_uiTextSlots.Length; i++)
        {
            if(i < numberOfPlayers)
                m_uiTextSlots[i].gameObject.SetActive(true);
            else
                m_uiTextSlots[i].gameObject.SetActive(false);
        }
    }

    public void AddPlayerToList(NetworkConnectionToClient player)
    {
        m_connectedPlayers.Add(player);
    }

    public void RemovePlayerFromList(NetworkConnectionToClient player)
    {
        m_connectedPlayers.Remove(player);
        m_players.Remove(player);
        m_enemies.Remove(player);
    }

    public void AddPlayerToPlayerTeam(NetworkConnectionToClient player)
    {
        m_players.Add(player);
    }

    public void AddPlayerToLevelTeam(NetworkConnectionToClient player)
    {
        m_enemies.Add(player);
    }

    public void IsAPlayer()
    {
        m_selectedPlayer = true;
    }

    public void IsAnEnemy()
    {
        m_selectedEnemy = true;
    }

    public void WaitForTeamSelection(NetworkConnectionToClient player)
    {

    }
}
