using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MLAPI;
using System.Text;

namespace CGber
{
    public class GUIManager : NetworkBehaviour
    {
        // InputField
        [SerializeField] private InputField _keyInputField;
        [SerializeField] private InputField _nameInputField;

        [SerializeField] private GameObject _mainMenuGui;
        [SerializeField] private GameObject _leaveButton;

        private static Dictionary<ulong, PlayerData> _clientData;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

            string[] args = System.Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--server")
                {
                    _keyInputField.text = "0000";
                    _nameInputField.text = "Host";
                    Host();
                }

            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("esc");
                _leaveButton.SetActive(!_leaveButton.activeSelf);
            }

        }

        private void OnDestroy()
        {
            // Prevent error in the editor
            if (NetworkManager.Singleton == null) { return; }

            NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void Host()
        {
            Debug.Log("This is HOST");
            _clientData = new Dictionary<ulong, PlayerData>();
            _clientData[NetworkManager.Singleton.LocalClientId] = new PlayerData(_nameInputField.text);

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost();
        }



        public void Client()
        {
            Debug.Log("This is Client");
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                connectKey = _keyInputField.text,
                playerName = _nameInputField.text
            });

            byte[] playloadBytes = Encoding.ASCII.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = playloadBytes;
            NetworkManager.Singleton.StartClient();



        }

        public void Leave()
        {
            Debug.Log("Leave");
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.StopHost();
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StopClient();
            }
            else
            {
                Debug.Log("Quit CGber");
                Application.Quit();
            }

            _mainMenuGui.SetActive(true);
            _leaveButton.SetActive(false);

        }

        private void HandleServerStarted()
        {
            // Temporary workaround to treat host as client
            if (NetworkManager.Singleton.IsHost)
            {
                HandleClientConnected(NetworkManager.Singleton.ServerClientId);
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            // Are we the client that is connecting?
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log("Client Connected");
                _mainMenuGui.SetActive(false);
                //_leaveButton.SetActive(true);
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                _clientData.Remove(clientId);
            }

            // Are we the client that is disconnecting?
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _mainMenuGui.SetActive(true);
                _leaveButton.SetActive(false);
            }
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {

            string payload = Encoding.ASCII.GetString(connectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            bool approveConnection = connectionPayload.connectKey == _keyInputField.text;

            if (approveConnection)
            {
                _clientData[clientId] = new PlayerData(connectionPayload.playerName);
            }

            callback(true, null, approveConnection, null, null);
        }

        public static PlayerData? GetPlayerData(ulong clientId)
        {
            if(_clientData.TryGetValue(clientId, out PlayerData playerData))
            {
                return playerData;
            }
            Debug.Log("there is nothing");
            return null;
        }

    }
}


