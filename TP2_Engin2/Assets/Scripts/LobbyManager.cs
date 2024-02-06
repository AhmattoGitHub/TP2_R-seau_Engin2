using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
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
    private TMP_Text[] m_uiTextSlots;
    [SerializeField]
    private GameObject m_configureMenu;
    [SerializeField]
    private List<NetworkConnectionToClient> m_connectedPlayers = new List<NetworkConnectionToClient>();
    [SerializeField]
    private GameObject m_slotsMenu;
    [SerializeField]
    private Button m_runnerButton;
    [SerializeField]
    private Button m_shooterButton;
    [SerializeField]
    private LobbyNetworkManager m_networkManager;

    private bool m_selectedRunner = false;
    private bool m_selectedShooter = false;

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
    }

    [Client]
    public void WaitForConfig()
    {

        GoToConfigurationMenu();
        PlayerConfig();
    }

    private void PlayerConfig()
    {
        m_runnerButton.onClick.AddListener(() => JoinRunnerTeam(m_nameField.text)); 
        m_shooterButton.onClick.AddListener(() => JoinShooterTeam(m_nameField.text));
        m_runnerButton.onClick.AddListener(() => GoToSlotsMenu());
        m_shooterButton.onClick.AddListener(() => GoToSlotsMenu());
        m_nameField.onEndEdit.AddListener((text) => ChangeName(text));
    }

    [Command(requiresAuthority = false)]
    private void JoinRunnerTeam(string newName, NetworkConnectionToClient player = null)
    {
        player.identity.tag = "Runner";
        Debug.Log(player.identity.name + " " + player.connectionId + " joined Runner team");
    }

    [Command(requiresAuthority = false)]
    private void JoinShooterTeam(string newName, NetworkConnectionToClient player = null)
    {
        player.identity.tag = "Shooter";
        Debug.Log(player.identity.name + " " + player.connectionId + " joined Shooter team");
    }

    [Command(requiresAuthority = false)]
    private void ChangeName(string text, NetworkConnectionToClient player = null)
    {
        player.identity.name = text;
        Debug.Log(player.identity.name + " has joined the lobby!");
    }
    private void GoToSlotsMenu()
    {
        m_configureMenu.SetActive(false);
        m_slotsMenu.SetActive(true);
    }
    private void GoToConfigurationMenu()
    {
        m_configureMenu.SetActive(true);
    }
}
