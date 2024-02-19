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
    private TMP_Text m_readyText;
    [SerializeField]
    private TMP_Text m_vsText;
    [SyncVar]
    private int m_numberOfRunners = 0;
    [SyncVar]
    private int m_numberOfShooters = 0;
    [SyncVar]
    private float m_timer = 4;

    [SerializeField]
    private NetManagerCustom m_networkManager;
    private bool m_changeSceneCalled = false;

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

    private void Update()
    {
        CheckIfTeamsAreFull();
        UpdatePlayerStatusUI();
        UpdateSlotsNameUI();

        if(Input.GetKeyDown(KeyCode.Space))
            CmdSetReadyStatus();
        if(LobbyIsReady() && !m_changeSceneCalled)
        {
            StartTimer();
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
        RemovePlayerSlotUI(player.m_uiSlotIndex);
        UpdateTeamCount(player);
        SetOfflineStatus(player.m_uiSlotIndex);
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
            player.m_tag = "Runner";
            player.m_uiSlotIndex = m_numberOfRunners;
            player.m_isOnline = true;
            m_numberOfRunners++;
            if(player.m_name == "")
                player.m_name = "Runner " + m_numberOfRunners;
        }
    }

    [Command(requiresAuthority = false)]
    private void JoinShooterTeam(string newName, NetworkConnectionToClient player = null)
    {
        if (m_numberOfShooters < 2)
        {
            player.m_tag = "Shooter";
            player.m_uiSlotIndex = m_numberOfShooters + 2;
            player.m_isOnline = true;
            m_numberOfShooters++;
            if (player.m_name == "")
                player.m_name = "Shooter " + m_numberOfShooters;
        }
    }

    [Command(requiresAuthority = false)]
    private void ChangeName(string text, NetworkConnectionToClient player = null)
    {
        player.m_name = text;
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
            if (player.m_tag == "Runner" || player.m_tag == "Shooter")
            {
                RpcUpdateSlotsNameUI(player.m_uiSlotIndex, player.m_name);
            }
        }
    }
    [ClientRpc]
    private void RpcUpdateSlotsNameUI(int uiSlotIndex, string playerName)
    {
        m_nameSection[uiSlotIndex].text = playerName;
    }

    private void RemovePlayerSlotUI(int index)
    {
        m_nameSection[index].text = "";
    }

    private void UpdateTeamCount(NetworkConnectionToClient player)
    {
        if(player.m_tag == "Runner")
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
        if (m_shooterButton == null)
        {
            return;
        }
        
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
        RpcSetOfflineStatus(index);
    }

    [ClientRpc]
    private void RpcSetOfflineStatus(int index)
    {
        m_readyStatus[index].text = "Offline";
    }

    [Command(requiresAuthority =false)]
    public void CmdSetReadyStatus(NetworkConnectionToClient player = null)
    {
        SetReadyStatus(player);
    }

    private void SetReadyStatus(NetworkConnectionToClient player) 
    {
        DisableReadyText(player);
        player.m_isReady = true;
    }

    [TargetRpc]
    private void DisableReadyText(NetworkConnectionToClient player)
    {
        m_readyText.gameObject.SetActive(false);
    }

    private bool LobbyIsReady()
    {
        if (m_numberOfRunners == 0 || m_numberOfShooters == 0)
        {
            m_vsText.text = "Vs";
            m_timer = 4;
            return false;
        }

        if (m_connectedPlayers.Count == 0)
        {
            return false;
        }

        int numberOfReady = 0;

        foreach(var player in m_connectedPlayers)
        {
            if(player.m_isReady)
            {
                numberOfReady++;
            }
        }

        return (numberOfReady == m_connectedPlayers.Count);
    }

    [ClientRpc]
    private void StartTimer()
    {
        if(m_timer <= 0)
        {            
            //LobbyNetworkManager.singleton.autoCreatePlayer = false;
            m_networkManager.autoCreatePlayer = false;
            ChangeScene();
            return;

        }
        m_timer -= Time.deltaTime;
        m_vsText.text = ((int)m_timer).ToString();
    }

    //[Server]
    private void ChangeScene()
    {        
        if (!isServer)
        {
            Debug.Log("blocked");
            return;
        }

        //Debug.Log("Change scene");
        //LobbyNetworkManager.singleton.ServerChangeScene("MainLevel");
        m_changeSceneCalled = true;
        m_networkManager.ServerChangeScene("MainLevel");
    }

    public List<NetworkConnectionToClient> GetList()
    {
        return m_connectedPlayers;
    }
}
