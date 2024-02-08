using JetBrains.Annotations;
using Mirror;
using Mirror.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    private static LobbyManager _instance;
    [SerializeField]
    private TMP_InputField m_nameField;
    [SerializeField]
    private GameObject m_configureMenu;
    [SerializeField]
    private List<NetworkConnectionToClient> m_connectedPlayers = new List<NetworkConnectionToClient>();
    [SerializeField]
    private GameObject m_slotsMenu;
    [SerializeField]
    private TMP_Text[] m_nameSection;
    [SerializeField]
    private TMP_Text[] m_readyStatus;
    [SerializeField]
    private Button m_runnerButton;
    [SerializeField]
    private Button m_shooterButton;
    [SerializeField]
    private LobbyNetworkManager m_networkManager;
    [SyncVar]
    private int m_numberOfRunners = 0;
    [SyncVar]
    private int m_numberOfShooters = 0;

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

    [Server]
    public void AddToConnections(NetworkConnectionToClient player)
    {
        m_connectedPlayers.Add(player);
    }

    [Server]
    public void RemoveFromConnections(NetworkConnectionToClient player)
    {
        m_connectedPlayers.Remove(player);
        RemovePlayerSlotUI(player);
        UpdateTeamCount(player);
        SetOfflineStatus(player.m_uiSlotIndex);
        UpdateSlotsNameUI();
    }

    [Client]
    public void WaitForConfig()
    {
        GoToConfigurationMenu();
        PlayerConfig();
    }

    [Client]
    private void PlayerConfig()
    {
        m_nameField.onEndEdit.AddListener((text) => ChangeName(text));
        m_runnerButton.onClick.AddListener(() => JoinRunnerTeam(m_nameField.text)); 
        m_shooterButton.onClick.AddListener(() => JoinShooterTeam(m_nameField.text));
        m_runnerButton.onClick.AddListener(() => GoToSlotsMenu());
        m_shooterButton.onClick.AddListener(() => GoToSlotsMenu());
    }

    [Command(requiresAuthority = false)]
    private void JoinRunnerTeam(string newName, NetworkConnectionToClient player = null)
    {
        if (m_numberOfRunners < 2)
        {
            player.identity.tag = "Runner";
            player.m_uiSlotIndex = m_numberOfRunners;
            player.m_isOnline = true;
            m_numberOfRunners++;
            UpdateSlotsNameUI();
            Debug.Log(player.identity.name + " joined Runner team");
        }
    }

    [Command(requiresAuthority = false)]
    private void JoinShooterTeam(string newName, NetworkConnectionToClient player = null)
    {
        if (m_numberOfShooters < 2)
        {
            player.identity.tag = "Shooter";
            player.m_uiSlotIndex = m_numberOfShooters + 2;
            player.m_isOnline = true;
            m_numberOfShooters++;
            UpdateSlotsNameUI();
            Debug.Log(player.identity.name + " joined Shooter team");
        }
    }

    [Command(requiresAuthority = false)]
    private void ChangeName(string text, NetworkConnectionToClient player = null)
    {
        player.identity.name = text;
        Debug.Log(player.identity.name + " has joined the lobby!");
    }

    [Client]
    private void GoToSlotsMenu()
    {
        m_configureMenu.SetActive(false);
        m_slotsMenu.SetActive(true);
    }

    [Client]
    private void GoToConfigurationMenu()
    {
        m_configureMenu.SetActive(true);
        m_slotsMenu.SetActive(false);
    }
    [Server]
    public void UpdateSlotsNameUI()
    {
        foreach (var player in m_connectedPlayers)
        {
            RpcUpdateSlotsNameUI(player.m_uiSlotIndex, player.identity.name);
        }
    }
    [ClientRpc]
    private void RpcUpdateSlotsNameUI(int uiSlotIndex, string playerName)
    {
        m_nameSection[uiSlotIndex].text = playerName;
    }

    private void RemovePlayerSlotUI(NetworkConnectionToClient player)
    {
        m_nameSection[player.m_uiSlotIndex].text = "";
    }

    private void UpdateTeamCount(NetworkConnectionToClient player)
    {
        if(player.identity.tag == "Runner")
        {
            m_numberOfRunners--;
        }
        else
        {
            m_numberOfShooters--;
        }
    }
    public void CheckIfTeamsAreFull()
    {
        if(m_numberOfRunners == 2)
        {
            m_runnerButton.gameObject.SetActive(false);
        }
        else if (m_numberOfShooters == 2)
        {
            m_shooterButton.gameObject.SetActive(false);
        }
    }

    [Server]
    public void UpdatePlayerStatusUI()
    {
        foreach(var player in m_connectedPlayers)
        {
            if(player.m_isOnline)
            {
                m_readyStatus[player.m_uiSlotIndex].text = "Online";
                if(player.m_isReady)
                {
                    m_readyStatus[player.m_uiSlotIndex].text = "Ready";
                }
                RpcUpdatePlayerStatusUI(player.m_uiSlotIndex, m_readyStatus[player.m_uiSlotIndex].text);
            }
        }
    }

    [ClientRpc]
    private void RpcUpdatePlayerStatusUI(int index, string status)
    {
        m_readyStatus[index].text = status;
    }

    [Server]
    private void SetOfflineStatus(int index)
    {
        m_readyStatus[index].text = "Offline";
    }

    [ClientRpc]
    private void RpcSetOfflineStatus(int index)
    {
        m_readyStatus[index].text = "Offline";
    }
}
