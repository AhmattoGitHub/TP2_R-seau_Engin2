using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using kcp2k;

namespace Mirror.Examples.Common
{
    public class CanvasNetworkManagerHUD : MonoBehaviour
    {
        [SerializeField] private GameObject startButtonsGroup;
        [SerializeField] private GameObject statusLabelsGroup;

        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startServerOnlyButton;
        [SerializeField] private Button startClientButton;

        [SerializeField] private Button mainStopButton;
        [SerializeField] private TMP_Text mainStopButtonText;
        [SerializeField] private Button secondaryStopButton;
        [SerializeField] private TMP_Text statusText;

        [SerializeField] private TMP_InputField inputNetworkAddress;
        [SerializeField] private TMP_InputField inputTransportPort;

        [SerializeField] private KcpTransport transport;

        private void Start()
        {
            // Init the input field with Network Manager's network address.
            //inputNetworkAddress.text = NetworkManager.singleton.networkAddress;
            //inputTransportPort.text = transport.port.ToString();
            RegisterListeners();

            //RegisterClientEvents();

            CheckWebGLPlayer();
        }

        private void RegisterListeners()
        {
            // Add button listeners. These buttons are already added in the inspector.
            startHostButton.onClick.AddListener(OnClickStartHostButton);
            startServerOnlyButton.onClick.AddListener(OnClickStartServerButton);
            startClientButton.onClick.AddListener(OnClickStartClientButton);
            mainStopButton.onClick.AddListener(OnClickMainStopButton);
            secondaryStopButton.onClick.AddListener(OnClickSecondaryStopButton);

            // Add input field listener to update NetworkManager's Network Address
            // when changed.
            inputNetworkAddress.onValueChanged.AddListener(delegate { OnNetworkAddressChange(); });
            inputTransportPort.onValueChanged.AddListener(delegate { OnTransportPortChange(); });
        }

        // Not working at the moment. Can't register events.
        /*private void RegisterClientEvents()
        {
            NetworkClient.OnConnectedEvent += OnClientConnect;
            NetworkClient.OnDisconnectedEvent += OnClientDisconnect;
        }*/

        private void CheckWebGLPlayer()
        {
            // WebGL can't be host or server.
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                startHostButton.interactable = false;
                startServerOnlyButton.interactable = false;
            }
        }

        private void RefreshHUD()
        {
            if (!NetworkServer.active && !NetworkClient.isConnected)
            {
                StartButtons();
            }
            else
            {
                StatusLabelsAndStopButtons();
            }
        }

        private void StartButtons()
        {
            if (!NetworkClient.active)
            {
                statusLabelsGroup.SetActive(false);
                startButtonsGroup.SetActive(true);
            }
            else
            {
                ShowConnectingStatus();
            }
        }

        private void StatusLabelsAndStopButtons()
        {
            startButtonsGroup.SetActive(false);
            statusLabelsGroup.SetActive(true);

            // Host
            if (NetworkServer.active && NetworkClient.active)
            {
                statusText.text = $"<b>Host</b>: running via {Transport.active}";

                mainStopButtonText.text = "Stop Client";
            }
            // Server only
            else if (NetworkServer.active)
            {
                statusText.text = $"<b>Server</b>: running via {Transport.active}";

                mainStopButtonText.text = "Stop Server";
            }
            // Client only
            else if (NetworkClient.isConnected)
            {
                statusText.text = $"<b>Client</b>: connected to {NetworkManager.singleton.networkAddress} via {Transport.active}";

                mainStopButtonText.text = "Stop Client";
            }

            // Note secondary button is only used to Stop Host, and is only needed in host mode.
            secondaryStopButton.gameObject.SetActive(NetworkServer.active && NetworkClient.active);
        }

        private void ShowConnectingStatus()
        {
            startButtonsGroup.SetActive(false);
            statusLabelsGroup.SetActive(true);

            secondaryStopButton.gameObject.SetActive(false);

            statusText.text = "Connecting to " + NetworkManager.singleton.networkAddress + "..";
            mainStopButtonText.text = "Cancel Connection Attempt";
        }

        public void OnClickStartHostButton()
        {
            NetworkManager.singleton.networkAddress = inputNetworkAddress.text;
            transport.port = ushort.Parse(inputTransportPort.text);
            NetworkManager.singleton.StartHost();
        }

        public void OnClickStartServerButton()
        {
            NetworkManager.singleton.StartServer();
        }

        public void OnClickStartClientButton()
        {
            NetworkManager.singleton.networkAddress = inputNetworkAddress.text;
            transport.port = ushort.Parse(inputTransportPort.text);

            NetworkManager.singleton.StartClient();
            //ShowConnectingStatus();
        }

        public void OnClickMainStopButton()
        {
            if (NetworkClient.active)
            {
                NetworkManager.singleton.StopClient();
            }
            else
            {
                NetworkManager.singleton.StopServer();
            }
        }

        public void OnClickSecondaryStopButton()
        {
            NetworkManager.singleton.StopHost();
        }

        public void OnNetworkAddressChange()
        {
            //NetworkManager.singleton.networkAddress = inputNetworkAddress.text;
        }

        public void OnTransportPortChange()
        {
            //try
            //{
            //    transport.port = ushort.Parse(inputTransportPort.text);
            //}
            //catch
            //{
            //    Debug.Log("Invalid port format!");
            //}
        }

        private void Update()
        {
            RefreshHUD();
            //Debug.Log(NetworkManager.singleton.networkAddress + " " + transport.port.ToString());
        }

        /* This does not work because we can't register the handler.
        void OnClientConnect() {}

        private void OnClientDisconnect()
        {
            RefreshHUD();
        }
        */

        // Do a check for the presence of a Network Manager component when
        // you first add this script to a gameobject.
        private void Reset()
        {
#if UNITY_2021_3_OR_NEWER
            if (!FindAnyObjectByType<NetworkManager>())
                Debug.LogError("This component requires a NetworkManager component to be present in the scene. Please add!");
#else
        // Deprecated in Unity 2023.1
        if (!FindObjectOfType<NetworkManager>())
            Debug.LogError("This component requires a NetworkManager component to be present in the scene. Please add!");
#endif
        }
    }
}
