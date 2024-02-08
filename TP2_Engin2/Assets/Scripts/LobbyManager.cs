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
        UpdateSlotsUI();
    }

    [Client]
    public void WaitForConfig()
    {
        GoToConfigurationMenu();
        PlayerConfig();
    }

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
            m_numberOfRunners++;
            Debug.Log(player.identity.name + " joined Runner team");
            UpdateSlotsUI();
        }
    }

    [Command(requiresAuthority = false)]
    private void JoinShooterTeam(string newName, NetworkConnectionToClient player = null)
    {
        if (m_numberOfShooters < 2)
        {
            player.identity.tag = "Shooter";
            player.m_uiSlotIndex = m_numberOfShooters + 2;
            m_numberOfShooters++;
            Debug.Log(player.identity.name + " joined Shooter team");
            UpdateSlotsUI();
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
    private void UpdateSlotsUI()
    {
        foreach (var player in m_connectedPlayers)
        {
            RpcUpdateSlotUI(player.m_uiSlotIndex, player.identity.name);
        }
    }
    [ClientRpc]
    private void RpcUpdateSlotUI(int uiSlotIndex, string playerName)
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
}
