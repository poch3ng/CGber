using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MLAPI;
using System.Text;

namespace CGber
{
    public class MenuManager : MonoBehaviour
    {

        // InputField
        [SerializeField] private InputField _keyInputField;
        [SerializeField] private InputField _nameInputField;

        [SerializeField] private GameObject _mainMenuGui;
        [SerializeField] private GameObject _leaveButton;


        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

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
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost();
        }



        public void Client()
        {
            Debug.Log("This is Client");
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(_keyInputField.text);
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
                _mainMenuGui.SetActive(false);
                _leaveButton.SetActive(true);
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            // Are we the client that is disconnecting?
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _mainMenuGui.SetActive(true);
                _leaveButton.SetActive(false);
            }
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            string password = Encoding.ASCII.GetString(connectionData);
            bool approveConnection = password == _keyInputField.text;

            callback(true, null, approveConnection, null, null);
        }
    }
}


